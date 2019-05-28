using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceChooseMenu : MonoBehaviour {

    //public GameObject Data;
    public RaceDontDestroy Decision;

    public int TotalMaps = 2;
    public int TotalCars = 6;

    public int selectedLaps = 2;
    public int selectedOP = 3;
    public int selectedLaps1 = 2;
    public int selectedOP1 = 3;
    public int selectedPlayer = 0;
    public int selectedPlayer1 = 1;
    public int selectedMapIndex = 0;

    public Text Laps;
    public Text Opponents;
    public Text LevelName;
    public Text CarName;

    public Canvas MAIN;
    public Canvas OPTIONS;
    public Canvas LEVELS;
    public Canvas CARS;
    public Canvas CREDITS;
    public Canvas CONFIRM;

    public Slider TopSlider;
    public Slider AccSlider;
    public Slider HanSLider;
    public Slider WeiSlider;

    public GameObject NextLev;
    public GameObject PrevLev;
    public GameObject NextCar;
    public GameObject PrevCar;

    public Image MAPIMAGE;
    public Image CARIMAGE;
    public Image MAPIMAGEFINAL;

    public Sprite[] MapImages;
    public Sprite[] CarImages;

    public string[] CarNames;
    public string[] MapNames;

    private bool MAINCANVAS = true;
    private bool OPTIONCANVAS = false;
    private bool LEVELCANVAS = false;
    private bool CARCANVAS = false;
    private bool CREDITCANVAS = false;
    private bool CONFIRMCANVAS = false;

    public GameObject MainPanel;
    public GameObject OptionPanel;
    public GameObject LevelPanel;
    public GameObject CarPanel;
    public GameObject ExitPanel;
    public GameObject LevelImagePanel;
    public GameObject CarLevelPanel;
    public GameObject CarImagePanel;

    private Animator MainAnimator;
    private Animator OptionAnimator;
    private Animator LevelAnimator;
    private Animator CarAnimator;
    private Animator ExitAnimator;
    private Animator LevelImage;
    private Animator CarLevelAnimator;
    private Animator CarImageAnimator;

    public CarBehaviour[] Behaviours;
    public Rigidbody[] CarBodies;

    public GameObject LoadingScreen;
    public Slider LoadingSlider;

    AsyncOperation async;

    private void Start()
    {
        Cursor.visible = true;

        selectedLaps = 2;
        selectedOP = 3;
        selectedPlayer = 0;
        selectedPlayer1 = 1;

        MainAnimator = MainPanel.GetComponent<Animator>();
        OptionAnimator = OptionPanel.GetComponent<Animator>();
        LevelAnimator = LevelPanel.GetComponent<Animator>();
        LevelImage = LevelImagePanel.GetComponent<Animator>();
        ExitAnimator = ExitPanel.GetComponent<Animator>();
        CarAnimator = CarPanel.GetComponent<Animator>();
        CarLevelAnimator = CarLevelPanel.GetComponent<Animator>();
        CarImageAnimator = CarImagePanel.GetComponent<Animator>();

        //TopSlider.value = Controllers[0].m_Topspeed;
        //AccSlider.value = Controllers[0].m_FullTorqueOverAllWheels;
        //HanSLider.value = Controllers[0].m_SteerHelper;
        WeiSlider.value = CarBodies[0].mass;

        CARIMAGE.sprite = CarImages[0];
        CarName.text = CarNames[0];

        MAPIMAGE.sprite = MapImages[0];
        LevelName.text = MapNames[0];


        MAINCANVAS = true;
    }

    private void Update()
    {
        Laps.text = selectedLaps.ToString();

        if (selectedMapIndex  == TotalMaps-1)
        {
            NextLev.SetActive(false);
        }
        else
        {
            NextLev.SetActive(true);
        }

        if (selectedPlayer == 0)
        {
            PrevCar.SetActive(false);
        }
        else
        {
            PrevCar.SetActive(true);
        }

        if (selectedPlayer == TotalCars-1)
        {
            NextCar.SetActive(false);
        }
        else
        {
            NextCar.SetActive(true);
        }

        if (selectedMapIndex == 0)
        {
            PrevLev.SetActive(false);
        }
        else
        {
            PrevLev.SetActive(true);
        }

        if (MAINCANVAS)
        {
            OPTIONCANVAS = false;
            LEVELCANVAS = false;
            CARCANVAS = false;
            CREDITCANVAS = false;
            CONFIRMCANVAS = false;

            MAIN.enabled = true;

            MainAnimator.Play("MAINSLIDEIN");
        }
        else
        {
            //MAIN.enabled = false;
            MainAnimator.Play("MAINSLIDEOUT");
        }

        if (OPTIONCANVAS)
        {
            OPTIONS.enabled = true;
            LEVELCANVAS = false;
            OptionAnimator.enabled = true;
            OptionAnimator.Play("OPTIONSLIDEIN");
            if (Input.GetButton("Cancel"))
            {
                MAINCANVAS = true;
                OptionAnimator.Play("OPTIONSLIDEOUT");
            }
        }
        else
        {
            //OPTIONS.enabled = false;
        }

        if (CONFIRMCANVAS)
        {
            CONFIRM.enabled = true;
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

        if (LEVELCANVAS)
        {
            LEVELS.enabled = true;
            CARCANVAS = false;
            LevelAnimator.enabled = true;
            LevelImage.enabled = true;
            LevelAnimator.Play("LEVELSLIDEIN");
            LevelImage.Play("LEVELIMAGESLIDEIN");
            if (Input.GetButtonDown("Cancel"))
            {
                MAINCANVAS = true;
                LevelAnimator.Play("LEVELSLIDEOUT");
                LevelImage.Play("LEVELIMAGESLIDEOUT");
            }
        }
        else
        {
            //LEVELS.enabled = false;
        }

        if (CARCANVAS)
        {
            CARS.enabled = true;
            MAPIMAGEFINAL.sprite = MapImages[selectedMapIndex];
            CarImageAnimator.enabled = true;
            CarLevelAnimator.enabled = true;
            CarAnimator.enabled = true;
            CarAnimator.Play("CARSLIDEIN");
            CarLevelAnimator.Play("CARLEVELSLIDEIN");
            CarImageAnimator.Play("CARIMAGESLIDEIN");
            if (Input.GetButtonDown("Cancel"))
            {
                LEVELCANVAS = true;
                CarAnimator.Play("CARSLIDEOUT");
                CarLevelAnimator.Play("CARLEVELSLIDEOUT");
                CarImageAnimator.Play("CARIMAGESLIDEOUT");
            }
        }
        else
        {
            //CARS.enabled = false;
        }
    }

    public void BackOption()
    {
        MAINCANVAS = true;
        OptionAnimator.Play("OPTIONSLIDEOUT");
    }

    public void BackLevel()
    {
        MAINCANVAS = true;
        LevelAnimator.Play("LEVELSLIDEOUT");
        LevelImage.Play("LEVELIMAGESLIDEOUT");
    }

    public void BackCar()
    {
        LEVELCANVAS = true;
        CarAnimator.Play("CARSLIDEOUT");
        CarLevelAnimator.Play("CARLEVELSLIDEOUT");
        CarImageAnimator.Play("CARIMAGESLIDEOUT");
    }

    public void OptionButton()
    {
        MAINCANVAS = false;
        OPTIONCANVAS = true;
    }

    public void LevelButton()
    {
        MAINCANVAS = false;
        LEVELCANVAS = true;
    }

    public void CarButton()
    {
        LEVELCANVAS = false;
        CARCANVAS = true;
        LevelAnimator.Play("LEVELSLIDEOUT");
        LevelImage.Play("LEVELIMAGESLIDEOUT");
        TopSlider.value = Behaviours[selectedPlayer].topSpeed;
        AccSlider.value = Behaviours[selectedPlayer].acceleration;
        HanSLider.value = Behaviours[selectedPlayer].rotationSpeed;
        WeiSlider.value = CarBodies[selectedPlayer].mass;
    }

    public void ExitButton()
    {
        MAINCANVAS = false;
        CONFIRMCANVAS = true;
    }

    public void YesButton()
    {
        Application.Quit();
    }

    public void NoButton()
    {
        MAINCANVAS = true;
        ExitAnimator.Play("EXITSLIDEOUT");
    }

    public int LapPluss()
    {
        if (selectedLaps1 < 10)
        {
            selectedLaps1++;
        }
        return selectedLaps1;
    }
    public void LapPlus()
    {
        if (selectedLaps < 10)
        {
            selectedLaps++;
        }
    }

    public int LapMinuss()
    {
        if (selectedLaps1 > 1)
        {
            selectedLaps1--;
        }
        return selectedLaps1;
    }
    public void LapMinus()
    {
        if (selectedLaps > 1)
        {
            selectedLaps--;
        }
    }
    public int OpPluss()
    {
        if (selectedOP1 < 9)
        {
            selectedOP1++;
        }
        return selectedOP1;
    }
    public void OpPlus()
    {
        if (selectedOP < 9)
        {
            selectedOP++;
        }
    }
    public int OpMinuss()
    {
        if (selectedOP1 >= 1)
        {
            selectedOP1--;
        }
        return selectedOP1;
    }
    public void OpMinus()
    {
        if (selectedOP >= 1)
        {
            selectedOP--;
        }
    }
    public void NextMap()
    {
        selectedMapIndex++;
        MAPIMAGE.sprite = MapImages[selectedMapIndex];
        LevelName.text = MapNames[selectedMapIndex];
    }

    public void PrevMap()
    {
        selectedMapIndex--;
        MAPIMAGE.sprite = MapImages[selectedMapIndex];
        LevelName.text = MapNames[selectedMapIndex];
    }

    public void NextCars()
    {
        if (selectedPlayer < TotalCars-1)
        {
            selectedPlayer++;
            CARIMAGE.sprite = CarImages[selectedPlayer];
            CarName.text = CarNames[selectedPlayer];
            TopSlider.value = Behaviours[selectedPlayer].topSpeed;
            AccSlider.value = Behaviours[selectedPlayer].acceleration;
            HanSLider.value = Behaviours[selectedPlayer].rotationSpeed;
            WeiSlider.value = CarBodies[selectedPlayer].mass;
        }
    }
    public void PrevCars()
    {
        if (selectedPlayer != 0)
        {
            selectedPlayer--;
            CARIMAGE.sprite = CarImages[selectedPlayer];
            CarName.text = CarNames[selectedPlayer];
            TopSlider.value = Behaviours[selectedPlayer].topSpeed;
            AccSlider.value = Behaviours[selectedPlayer].acceleration;
            HanSLider.value = Behaviours[selectedPlayer].rotationSpeed;
            WeiSlider.value = CarBodies[selectedPlayer].mass;
        }
    }
    public void RaceButton()
    {
        Decision.selectedPlayer = selectedPlayer;
        Decision.data = selectedLaps;

        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        LoadingScreen.SetActive(true);
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;

        while(async.isDone == false)
        {
            LoadingSlider.value = async.progress;
            if (async.progress == 0.9f)
            {
                LoadingSlider.value = 1f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}

