using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Behaviour Scriptable Object")]
    [SerializeField] private CarBehaviour behaviour;

    [Header("Sounds")] [SerializeField] private AudioSource engineSource;
    [SerializeField] private AudioSource tireSource;
    [SerializeField] private float dampingSpeed;

    [Header("Front Wheels")] [SerializeField] private List<Transform> frontWheelsX;
    [SerializeField] private List<Transform> frontWheelsY;
    [SerializeField] [Range(0,1)] private float maxFrontWheelRotation = 0.4f;
    [SerializeField] private float frontWheelRotationSpeed = 100;

    [Header("Back Wheels")] [SerializeField] private List<Transform> backWheels;
    [SerializeField] private float backWheeRotationMultiplier = 100;
    [SerializeField] private float backWheeRotationSlipForward = 1000;
    [SerializeField] private float backWheeRotationSlipBackwards = 250;
    [SerializeField] private float tireSpinMaxSpeed = 5f;

    [Header("Drifting")] [SerializeField] private float driftValue;
    [SerializeField] private TrailRenderer[] skidmarks;
    [SerializeField] private ParticleSystem driftSmoke;
    [SerializeField] private float maxDriftSmokeSize = 10;

    [Header("Burnout")] [SerializeField] private int burnoutTireSpin = 2000;
    [SerializeField] private float burnoutSmokeSize = 8;
    private float timer;

    [Header("Raycasts")] [SerializeField] private float offsetDistanceTire;
    [SerializeField] private float offsetDistanceCar;

    public bool hasCollided = false;

    private bool isTurning = false;
    private bool isUsingTorque = false;
    private bool forward = false;
    private bool backwards = false;

    private Rigidbody carBody;

    private IEnumerator returnWheels;

    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<GameManager>();

        engineSource.clip = behaviour.carEngineSound;
        engineSource.Play();
        tireSource.clip = manager.tireSqueel;

        tireSource.Play();

        carBody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        //Stop emmiting drift smoke
        var emmision = driftSmoke.emission;
        emmision.enabled = false;

        //Stop emmiting skidmarks
        foreach (var item in skidmarks)
            item.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        EngineSoundUpdate();
        TireSkidSoundUpdate();

        if (CheckIfGrounded())
        {
            if (Input.GetButton("Forward"))
            {
                AddForce(behaviour.acceleration);
                RotateWheels(backWheeRotationSlipForward, backWheels, false);
                forward = true;
                isUsingTorque = true;
            }
            else if (Input.GetButtonUp("Forward"))
            {
                forward = false;
                isUsingTorque = false;
            }

            if (Input.GetButton("Backwards"))
            {
                AddForce(-behaviour.acceleration);
                backwards = true;
                RotateWheels(-backWheeRotationSlipBackwards, backWheels, false);
                isUsingTorque = true;
            }
            else if (Input.GetButtonUp("Backwards"))
            {
                backwards = false;
                isUsingTorque = false;
            }


            if (Input.GetButton("Left"))
            {
                AddTorque(-behaviour.rotationSpeed);
                RotateFrontWheels(-1);
                isTurning = true;
            }
            else if (Input.GetButtonUp("Left"))
            {
                isTurning = false;
            }

            if (Input.GetButton("Right"))
            {
                AddTorque(behaviour.rotationSpeed);
                RotateFrontWheels(1);
                isTurning = true;
            }
            else if (Input.GetButtonUp("Right"))
            {
                isTurning = false;
            }
        }

        if (!isTurning)
        {
            RotateFrontWheelsBack();
        }

        ToogleSkidmarks();
        ToogleBackWheelSkidmarks();

        RotateWheels(backWheeRotationMultiplier * GetLocalVelocity(), frontWheelsX, false);
        RotateWheels(backWheeRotationMultiplier * GetLocalVelocity(), backWheels, backwards && forward);
    }

    private void EngineSoundUpdate()
    {
        engineSource.pitch = Mathf.Lerp(1, 3, GetInverseLerpOfVelocity());
    }

    private void TireSkidSoundUpdate()
    {
        if (CheckIfDrifting())
        {
            tireSource.volume = Mathf.Lerp(tireSource.volume,Mathf.InverseLerp(0, behaviour.topSpeed,
                Mathf.Abs((transform.forward - transform.InverseTransformDirection(carBody.velocity)).x)), dampingSpeed * Time.deltaTime);
        }
        else
            tireSource.volume = Mathf.Lerp(tireSource.volume,0, dampingSpeed * Time.deltaTime);
    }

    public bool CheckIfDrifting()
    {
        if (CheckIfGrounded())
            return Mathf.Abs((transform.forward - transform.InverseTransformDirection(carBody.velocity)).x) >
                   driftValue;

        return false;
    }

    public float GetDriftValue()
    {
        return Mathf.Abs((transform.forward - transform.InverseTransformDirection(carBody.velocity)).x);
    }

    private void ToogleSkidmarks()
    {
        RaycastHit hit;

        if (CheckIfDrifting())
        {
            for (int i = 0; i < frontWheelsY.Count; i++)
            {
                skidmarks[i].emitting = Physics.Raycast(frontWheelsY[i].position, -Vector3.up, out hit,
                    offsetDistanceTire);
            }
            for (int i = 0; i < backWheels.Count; i++)
            {
                skidmarks[i + 2].emitting = Physics.Raycast(backWheels[i].position, -Vector3.up, out hit,
                    offsetDistanceTire);
            }

            ToogleDriftSmoke(true);
        }
        else
        {
            foreach (var item in skidmarks)
                item.emitting = false;

            ToogleDriftSmoke(false);
        }
    }

    private void ToogleBackWheelSkidmarks()
    {
        if (GetLocalVelocity() < tireSpinMaxSpeed && isUsingTorque && CheckIfGrounded())
        {
            RaycastHit hit;

            for (int i = 0; i < 2; i++)
            {
                skidmarks[i + 2].emitting = Physics.Raycast(backWheels[i].position, -Vector3.up, out hit,
                    offsetDistanceTire);
            }

            ToogleDriftSmoke(true);

            tireSource.volume = Mathf.Lerp(tireSource.volume,GetInverseLerpOfVelocity(), dampingSpeed * Time.deltaTime);
        }
    }

    private void ToogleDriftSmoke(bool emmit)
    {
        var emmision = driftSmoke.emission;
        emmision.enabled = emmit;

        if (!CheckIfGrounded())
        {
            emmision.enabled = false;
            return;
        }

        if (!emmit) return;

        var main = driftSmoke.main;
        main.startSize = maxDriftSmokeSize * GetInverseLerpOfVelocity();
    }

    private bool CheckIfGrounded()
    {
        RaycastHit hit;

        return Physics.Raycast(transform.position + new Vector3(0,0.3f), -transform.up, out hit,
            offsetDistanceCar);
    }

    private void AddForce(int speed)
    {
        if (!(carBody.velocity.magnitude < behaviour.topSpeed)) return;

        Vector3 v3Force = speed * transform.forward * Time.deltaTime;
        carBody.AddForce(v3Force);
    }

    private void RotateFrontWheels(int direction)
    {
        foreach (var tire in frontWheelsY)
        {
            if (tire.localRotation.y * direction < maxFrontWheelRotation)
            {
                tire.Rotate(tire.up, frontWheelRotationSpeed * Time.deltaTime *
                                                    direction);
            }
        }
    }

    private void RotateFrontWheelsBack()
    {
        foreach (var tire in frontWheelsY)
        {
            if (tire.localRotation.y != 0)
            {
                if (tire.localRotation.y < 0)
                {
                    tire.Rotate(transform.up, frontWheelRotationSpeed * Time.deltaTime * GetInverseLerpOfVelocity());
                }
                else
                {
                    tire.Rotate(transform.up, frontWheelRotationSpeed * Time.deltaTime * GetInverseLerpOfVelocity() * -1);
                }
            }
        }
    }

    private void RotateWheels(float multiplier, List<Transform> wheels, bool burnout)
    {
        foreach (var tire in wheels)
        {
            if (burnout)
            {
                tire.Rotate(Time.deltaTime * burnoutTireSpin,0,0);

                tireSource.volume = Mathf.Lerp(tireSource.volume, 1, dampingSpeed * Time.deltaTime);

                timer += Time.deltaTime;
                var main = driftSmoke.main;
                var emission = driftSmoke.emission;
                emission.enabled = true;
                main.startSize = burnoutSmokeSize;
            }
            else
            {
                tire.Rotate(Time.deltaTime * multiplier * GetVelocity(), 0, 0);
                timer = 0;
            }
        }
    }

    private void AddTorque(int rotationSpeed)
    {
        if (!(carBody.angularVelocity.magnitude < behaviour.maxRotationVelocity)) return;

        carBody.AddTorque(
            transform.up * rotationSpeed * Time.deltaTime *
            Mathf.Clamp(Mathf.InverseLerp(0, behaviour.topSpeed, carBody.velocity.magnitude),0.25f,1));
    }

    public float GetLocalVelocity()
    {
        return transform.InverseTransformDirection(carBody.velocity).z;
    }

    public float GetVelocity()
    {
        return carBody.velocity.magnitude;
    }

    public float GetInverseLerpOfVelocity()
    {
        return Mathf.InverseLerp(0, behaviour.topSpeed, GetVelocity());
    }

    public float GetAngularVelocity()
    {
        return carBody.angularVelocity.magnitude;
    }

    public float GetInverseLerpOfAngularVelocity()
    {
        return Mathf.InverseLerp(0, behaviour.maxRotationVelocity, GetAngularVelocity());
    }

    public float GetLocalAngularVelocity()
    {
        return transform.InverseTransformDirection(carBody.angularVelocity).y;
    }

    public float GetTurnDirection()
    {
        return Mathf.Sign(GetLocalAngularVelocity());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Road" && collision.transform.tag != "Ground")
        {
            hasCollided = true;
        }
    }
}
