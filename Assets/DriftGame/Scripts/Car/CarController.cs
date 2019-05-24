using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Behaviour Scriptable Object")]
    [SerializeField] private CarBehaviour behaviour;

    [Header("Front Wheels")] [SerializeField] private List<GameObject> frontWheelsX;
    [SerializeField] private List<GameObject> frontWheelsY;
    [SerializeField] [Range(0,1)] private float maxFrontWheelRotation = 0.4f;
    [SerializeField] private float frontWheelRotationSpeed = 100;

    [Header("Back Wheels")] [SerializeField] private List<GameObject> backWheels;
    [SerializeField] private float backWheeRotationMultiplier = 100;
    [SerializeField] private float backWheeRotationSlipForward = 1000;
    [SerializeField] private float backWheeRotationSlipBackwards = 250;

    private Rigidbody carBody;

    private IEnumerator returnWheels;

    // Start is called before the first frame update
    void Start()
    {
        carBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Forward"))
        {
            AddForce(behaviour.acceleration);
            RotateWheels(backWheeRotationSlipForward,backWheels);
        }

        if (Input.GetButton("Backwards"))
        {
            AddForce(-behaviour.acceleration);
            RotateWheels(-backWheeRotationSlipBackwards,backWheels);
        }

        if (Input.GetButton("Left"))
        {
            AddTorque(-behaviour.rotationSpeed);
            RotateFrontWheels(-1);
        }

        else if (Input.GetButtonUp("Left"))
        {
            
        }

        if (Input.GetButton("Right"))
        {
            AddTorque(behaviour.rotationSpeed);
            RotateFrontWheels(1);
        }
        else if (Input.GetButtonUp("Right"))
        {
            
        }

        RotateWheels(backWheeRotationMultiplier * GetLocalVelocity(),frontWheelsX);
        RotateWheels(backWheeRotationMultiplier * GetLocalVelocity(),backWheels);
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
            if (tire.transform.localRotation.y * direction < maxFrontWheelRotation)
            {
                tire.transform.Rotate(transform.up, frontWheelRotationSpeed * Time.deltaTime *
                                                    Mathf.Abs(carBody.angularVelocity.y) *
                                                    direction);
            }
        }
    }

    private void RotateWheels(float multiplier, List<GameObject> wheels)
    {
        foreach (var tire in wheels)
        {
            tire.transform.Rotate(Time.deltaTime * multiplier * GetVelocity(),0,0);
        }
    }

    private void AddTorque(int rotationSpeed)
    {
        if (!(carBody.angularVelocity.magnitude < behaviour.maxRotationVelocity)) return;

        carBody.AddTorque(
            transform.up * rotationSpeed * Time.deltaTime *
            Mathf.InverseLerp(0, behaviour.topSpeed, carBody.velocity.magnitude));
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
        return carBody.velocity.magnitude;
    }

    public float GetInverseLerpOfAngularVelocity()
    {
        return Mathf.InverseLerp(0, behaviour.maxRotationVelocity, GetAngularVelocity());
    }
}
