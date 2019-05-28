using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Laps : MonoBehaviour
{
    public Text lapText;
    // These Static Variables are accessed in "checkpoint" Script
    public Transform[] checkPointArray;
    public static Transform[] checkpointA;
    public static int currentCheckpoint = 0;
    public static int currentLap = 0;
    public Vector3 startPos;
    public int Lap;

    private int lapsToComplete;

    private bool isDisabled = false;

    private void Awake()
    {
        RaceDontDestroy decision = GameObject.FindGameObjectWithTag("Decision").GetComponent<RaceDontDestroy>();

        lapsToComplete = decision.data;
    }

    void Start()
    {
        startPos = transform.position;
        currentCheckpoint = 0;
        currentLap = 0;

    }

    void Update()
    {
        Lap = currentLap;
        checkpointA = checkPointArray;

        lapText.text = currentLap != 0 ? currentLap + "/" + lapsToComplete :
        1 + "/" + lapsToComplete;

        if (Lap-1 == lapsToComplete)
        {
            if (isDisabled) return;
            DriftPoints.Instance.DisableCanvas();
            isDisabled = true;
        }
    }

}
