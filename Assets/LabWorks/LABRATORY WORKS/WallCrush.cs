using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCrush : MonoBehaviour
{
    [Range(-1,0)]
    public int side;

    public Rigidbody rigid;
    public int Force;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 force = new Vector3(side * Force, 0);

            rigid.AddForce(force, ForceMode.Acceleration);
        }
    }
}
