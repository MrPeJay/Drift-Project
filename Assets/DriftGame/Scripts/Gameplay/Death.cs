using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private bool isPlayer;

    private bool isDead = false;
    public Vector3 startPos;
    public Quaternion startRot;

    private bool toogled = false;

    private GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;

        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (!isPlayer) return;

        if (Input.GetButtonDown("Restart"))
        {
            if (!isDead)
            {
                DestroyMe();
            }
        }
    }

    private void DestroyMe()
    {
        GameManager.Instance.SpawnObject(manager.deathParticles,transform.position);
        GameManager.Instance.SpawnObject(manager.explosionSound,transform.position);
        StartCoroutine(MoveToDeathPos());

        if (isPlayer)
        {
            GetComponent<CarController>().hasCollided = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Death")
        {
            DestroyMe();
        }
    }

    private IEnumerator MoveToDeathPos()
    {
        ToogleScripts();
        isDead = true;
        transform.position = manager.deathPos;
        float time = manager.timeToRespawn;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPos;
        transform.rotation = startRot;

        isDead = false;
        ToogleScripts();

        GameManager.Instance.SpawnObject(manager.respawnParticles, transform.position);
        GameManager.Instance.SpawnObject(manager.respawnSound, transform.position);
    }

    private void ToogleScripts()
    {
        toogled = !toogled;

        ToogleCamera();
        ToogleCarController();
        ToogleRigidBody();
    }

    private void ToogleCamera()
    {
        if (!isPlayer) return;

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        TopDownFollow script = camera.GetComponent<TopDownFollow>();

        script.enabled = !script.enabled;
    }

    private void ToogleCarController()
    {
        if (!isPlayer) return;

        CarController controller = GetComponent<CarController>();
        controller.enabled = !controller.enabled;
    }

    private void ToogleRigidBody()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();

        rigid.constraints = toogled ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
    }
}
