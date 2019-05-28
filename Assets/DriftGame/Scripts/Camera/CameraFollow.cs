using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;

    [SerializeField] private Vector3 distanceToObject;
    [SerializeField] private float maxFov;

    private float startFov;
    private Camera camera;
    private CarController car;
    private Transform parent;

    void Start()
    {
        parent = transform.parent;
        car = objectToFollow.GetComponent<CarController>();

        camera = GetComponent<Camera>();
        startFov = camera.fieldOfView;

        transform.localPosition += distanceToObject;
    }

    void Update()
    {
        transform.LookAt(parent.transform.position);

        camera.fieldOfView = Mathf.Lerp(startFov, maxFov, car.GetInverseLerpOfVelocity());
    }
}
