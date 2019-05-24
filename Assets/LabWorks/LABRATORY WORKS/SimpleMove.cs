using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [Range(1,10)]
    public float MoveSpeed;

    private Rigidbody rigid;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(MoveSpeed*Input.GetAxis("Horizontal")*Time.deltaTime,0f,MoveSpeed*Input.GetAxis("Vertical")*Time.deltaTime);
    }
}
