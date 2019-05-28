using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownFollow : MonoBehaviour
{
    [SerializeField] private Vector3 distance;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        transform.position = player.position + distance;
        transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + distance;
    }
}
