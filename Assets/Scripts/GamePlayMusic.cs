using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayMusic : MonoBehaviour
{
    public AudioClip[] Music;
    private AudioSource Source;

    private bool found = false;

    private int songInt;
    private int musicLen;

    private DriftPoints driftpoints;

    private void Start()
    {
        driftpoints = GameObject.FindObjectOfType<DriftPoints>();

        Source = GetComponent<AudioSource>();
        musicLen = Music.Length;
        found = false;
        Invoke("GetObjects", 1);
    }

    private void Update()
    {
        if (driftpoints.countDownComplete)
        {
            if (!Source.isPlaying)
            {
                songInt = Random.Range(0, Music.Length);
                Source.clip = Music[songInt];
                Source.Play();
            }

            if (Input.GetButtonDown("NextSong"))
            {
                if (songInt + 1 >= Music.Length)
                {
                    songInt = 0;
                }
                else
                {
                    songInt++;
                }
                Source.clip = Music[songInt];
                Source.Play();
            }
            if (Input.GetButtonDown("PrevSong"))
            {
                if (songInt - 1 < 0)
                {
                    songInt = Music.Length - 1;
                }
                else
                {
                    songInt--;
                }
                Source.clip = Music[songInt];
                Source.Play();
            }

        }
    }
}
