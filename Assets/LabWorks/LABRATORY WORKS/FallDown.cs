using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        rigid.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        rigid.useGravity = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            rigid.useGravity = true;
        }
    }
}
