using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")] public float timeToRespawn = 3;
    [Header("Spawn")] public Transform spawnPoint;

    [Header("Particles")]
    public GameObject deathParticles;
    public GameObject respawnParticles;

    [Header("Sounds")]
    public GameObject explosionSound;
    public GameObject respawnSound;

    public AudioClip tireSqueel;

    [Header("Death")]
    public Vector3 deathPos = new Vector3(-100, -100, -100);

    [Header("Cars")] public List<GameObject> cars;

    private void Awake()
    {
        RaceDontDestroy decision = GameObject.FindGameObjectWithTag("Decision").GetComponent<RaceDontDestroy>();

        Instantiate(cars[decision.selectedPlayer], spawnPoint.localPosition,spawnPoint.localRotation);
    }

    public void SpawnObject(GameObject @object, Vector3 position)
    {
        if (@object != null)
        {
            Instantiate(@object, position, Quaternion.identity);
        }
    }
}
