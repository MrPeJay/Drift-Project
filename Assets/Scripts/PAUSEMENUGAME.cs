using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PAUSEMENUGAME : MonoBehaviour {

    public bool GameIsPaused = false;

    private GameObject Decision;

    public GameObject MainPanel;
    public GameObject ExitPanel;
    public GameObject RestartPanel;

    private Animator MainAnimator;
    private Animator ExitAnimator;
    private Animator RestartAnimator;

    public Canvas MAIN;
    public Canvas EXIT;
    public Canvas RESTART;

    private bool MAINCANVAS = true;
    private bool EXITCANVAS = false;
    private bool RESTARTCANVAS = false;

    // Use this for initialization
    void Start()
    {
        MainAnimator = MainPanel.GetComponent<Animator>();
        ExitAnimator = ExitPanel.GetComponent<Animator>();
        RestartAnimator = RestartPanel.GetComponent<Animator>();

        Decision = GameObject.FindGameObjectWithTag("Decision");

        MAINCANVAS = false;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameIsPaused)
            {
                if (MAINCANVAS)
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }

        if (GameIsPaused)
        {
            Cursor.visible = true;
            if (MAINCANVAS)
            {
                EXITCANVAS = false;
                RESTARTCANVAS = false;

                MAIN.enabled = true;
                MainAnimator.enabled = true;
                MainAnimator.Play("MAINSLIDEIN");
            }
            else
            {
                //MAIN.enabled = false;
                MainAnimator.Play("MAINSLIDEOUT");
            }

            if (EXITCANVAS)
            {
                EXIT.enabled = true;
                ExitAnimator.enabled = true;
                ExitAnimator.Play("EXITSLIDEIN");
                if (Input.GetButtonDown("Cancel"))
                {
                    MAINCANVAS = true;
                    ExitAnimator.Play("EXITSLIDEOUT");
                }
            }
            else
            {
                //CONFIRM.enabled = false;
            }

            if (RESTARTCANVAS)
            {
                RESTART.enabled = true;
                RestartAnimator.enabled = true;
                RestartAnimator.Play("EXITSLIDEIN");
                if (Input.GetButtonDown("Cancel"))
                {
                    MAINCANVAS = true;
                    RestartAnimator.Play("EXITSLIDEOUT");
                }
            }
        }
        else
        {
            Cursor.visible = false;
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;

        GameIsPaused = true;
        MAINCANVAS = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        MAINCANVAS = false;
        MainAnimator.Play("MAINSLIDEOUT");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Destroy(Decision);
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        MAINCANVAS = false;
        RESTARTCANVAS = true;
    }

    public void ExitButton()
    {
        MAINCANVAS = false;
        EXITCANVAS = true;
    }


    public void RestartNo()
    {
        MAINCANVAS = true;
        RestartAnimator.Play("EXITSLIDEOUT");
    }

    public void RestartYes()
    {
        Time.timeScale = 1f;
        MAINCANVAS = false;
        SceneManager.LoadScene(1);
    }

    public void ExitNo()
    {
        MAINCANVAS = true;
        ExitAnimator.Play("EXITSLIDEOUT");
    }

    public void ExitYes()
    {
        Time.timeScale = 1f;
        MAINCANVAS = false;
        Destroy(Decision);
        SceneManager.LoadScene(0);
    }
}
