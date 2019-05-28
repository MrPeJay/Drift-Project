using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DriftPoints : Singleton<DriftPoints>
{
    [SerializeField] private float timeToGoToMainMenu = 5;
    [SerializeField] private Text countDownText;
    [SerializeField] private Canvas endCanvas;
    public Text pointsInput, timeInput, pointHighScore, timeHighScore;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text timerText;
    [SerializeField] private Text pointField;
    [SerializeField] private Text tempPointField;
    [SerializeField] private float timeToCountDown = 3;
    [SerializeField] private float timeToAddPoints;
    private float startTime = 0;
    private float timer;

    private float time;

    public float points;
    public float tempPoints;

    private bool failed = false;

    private CarController player;

    public bool countDownComplete = false;

    private bool isStopped = false;
    private void Start()
    {
        pointHighScore.enabled = false;
        timeHighScore.enabled = false;

        endCanvas.enabled = false;
        canvas.enabled = false;
        countDownComplete = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        player.enabled = false;

        StartCoroutine(CountDown());
    }

    public void DisableCanvas()
    {
        isStopped = true;

        canvas.enabled = false;
        points += Mathf.RoundToInt(tempPoints);

        pointsInput.text = points.ToString();
        timeInput.text = timerText.text;

        if (!PlayerPrefs.HasKey("points"))
        {
            pointHighScore.enabled = true;
            PlayerPrefs.SetFloat("points",points);
        }
        else
        {
            if (PlayerPrefs.GetFloat("points") < points)
            {
                pointHighScore.enabled = true;
                PlayerPrefs.SetFloat("points", points);
            }
        }


        if (!PlayerPrefs.HasKey("time"))
        {
            timeHighScore.enabled = true;
            PlayerPrefs.SetFloat("time",time);
        }
        else
        {
            if (PlayerPrefs.GetFloat("time") > time)
            {
                timeHighScore.enabled = true;
                PlayerPrefs.SetFloat("time", time);
            }
        }

        endCanvas.enabled = true;
        StartCoroutine(GoToMainMenuAfterSeconds());
    }

    private IEnumerator GoToMainMenuAfterSeconds()
    {
        yield return new WaitForSeconds(timeToGoToMainMenu);
        GameObject Decision = GameObject.FindGameObjectWithTag("Decision");
        Destroy(Decision);
        SceneManager.LoadScene(0);
    }

    private IEnumerator CountDown()
    {
        float time = timeToCountDown;
        countDownText.text = "3";

        while (time > 0)
        {
            time -= Time.deltaTime;

            if (time < 2)
            {
                countDownText.text = "2";
            }
            if (time < 1)
            {
                countDownText.text = "1";
            }

            yield return 0;
        }

        countDownText.text = "GO";
        EnableCarControl();
        canvas.enabled = true;
        countDownComplete = true;

        StartCoroutine(disableCountDown());
    }

    private IEnumerator disableCountDown()
    {
        yield return new WaitForSeconds(2);

        countDownText.enabled = false;
    }

    private void EnableCarControl()
    {
        player.enabled = true;
    }

    private void Update()
    {
        if (countDownComplete)
        {
            if (player.CheckIfDrifting())
            {
                tempPointField.enabled = true;

                tempPoints += player.GetDriftValue() / 100 * player.GetVelocity();

                if (!failed)
                {
                    tempPointField.text = "+" + Mathf.RoundToInt(tempPoints);
                }

                StartCoroutine(StartTimer());
            }

            if (!isStopped)
            {
                time = Time.time - startTime;

                string minutes = ((int) time / 60).ToString();
                string seconds = (time % 60).ToString("f2");

                timerText.text = minutes + ":" + seconds;
            }
        }
    }

    private IEnumerator StartTimer()
    {
        float time = timeToAddPoints;

        while (time > 0)
        {
            time -= Time.deltaTime;

            yield return 0;

            if (player.hasCollided)
            {
                if (!failed)
                {
                    player.hasCollided = false;
                    StartCoroutine(ShowRedPoints());
                    yield break;
                }
            }

            if (player.CheckIfDrifting())
            {
                yield break;
            }
        }
        //DEFENCE
        points += Mathf.RoundToInt(Random.Range(tempPoints / 2, tempPoints));
        tempPoints = 0;

        tempPointField.text = tempPoints.ToString();
        tempPointField.enabled = false;

        pointField.text = points.ToString();
    }

    private IEnumerator ShowRedPoints()
    {
        failed = true;

        float time = timeToAddPoints;
        tempPointField.color = Color.red;

        while (time > 0)
        {
            time -= Time.deltaTime;

            yield return 0;
        }

        tempPointField.color = Color.white;
        tempPoints = 0;
        tempPointField.text = tempPoints.ToString();
        tempPointField.enabled = false;

        failed = false;
    }
}
