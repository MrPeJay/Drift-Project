using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyUp : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField]
    private int forceUp;

    private Vector2 upForce;
    [SerializeField]
    private bool isFlying = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();

        upForce = new Vector2(0, forceUp);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isFlying = true;
        }
    }

    private void Update()
    {
        if (isFlying)
        {
            rigid.AddForce(upForce*Time.deltaTime);
        }
    }
}
