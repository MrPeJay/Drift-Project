using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car Behaviour", menuName = "Car Behaviour")]
public class CarBehaviour : ScriptableObject
{
    public int topSpeed, acceleration, rotationSpeed, maxRotationVelocity;
}
