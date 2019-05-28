using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [SerializeField] [Range(0,1)] float maxRotation = 0.3f;
    [SerializeField] private int rotationSpeed = 20;

    private CarController car;
    

    void Start()
    {
        car = GetComponentInParent<CarController>();
        transform.position = transform.parent.position;
    }

    void Update()
    {
        Debug.Log(transform.localRotation.y);
        RotateMe();
    }

    private void RotateMe()
    {
        if (transform.localRotation.y * car.GetTurnDirection() < maxRotation)
        {
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime * Mathf.Abs(car.GetLocalAngularVelocity()) *
                                           car.GetTurnDirection());
        }
    }
}
