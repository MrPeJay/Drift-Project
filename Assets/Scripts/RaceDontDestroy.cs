using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceDontDestroy : MonoBehaviour {

    public int data;
    public int selectedPlayer;


    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
