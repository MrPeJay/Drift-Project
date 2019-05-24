using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public Vector3 PushDirection;

    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            rigid.AddForce(PushDirection,ForceMode.Force);
        }
    }
}
