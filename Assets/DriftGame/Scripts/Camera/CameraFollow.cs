using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject objectToFollow;

    [SerializeField] private Vector3 distanceToObject;
    [SerializeField] private float maxFov;
    [SerializeField] private float maxSideTurn;
    [SerializeField] private int damping;
    private float startFov;

    private Camera camera;

    private CarController car;
    // Start is called before the first frame update
    void Start()
    {
        car = objectToFollow.GetComponent<CarController>();

        camera = GetComponent<Camera>();
        startFov = camera.fieldOfView;

        gameObject.transform.position = objectToFollow.transform.position + distanceToObject;
        transform.LookAt(objectToFollow.transform.position);
    }

    void Update()
    {
        gameObject.transform.position = objectToFollow.transform.position + distanceToObject;
        transform.LookAt(objectToFollow.transform.position);
        /*
        var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, objectToFollow.transform.eulerAngles.y - 180, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * damping);
        */
        camera.fieldOfView = Mathf.Lerp(startFov, maxFov, car.GetInverseLerpOfVelocity());
    }
}
