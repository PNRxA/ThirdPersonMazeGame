using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool splash = true;
    private bool showMainMenu = false;
    private bool showSelectCharacterMenu = false;
    private bool showOptionsMenu = false;
    private bool inGame = false;
    public bool showPauseMenu = false;
    public GameObject menuCamera;
    public Texture2D splashTexture;
    private float scrH, scrW;
    public GameObject game1Scientist, game1Monster, game2Scientist, game2Monster;
    public GameObject selectCharacterPosition, mainMenuPosition;
    public GameObject game1ScientistSpawn, game1MonsterSpawn, game2ScientistSpawn, game2MonsterSpawn;
    public float speed = 10f;

    void Awake()
    {
        Invoke("EndSplash", 2f);
    }

    void FixedUpdate()
    {
        if (showSelectCharacterMenu)
        {
            float step = speed * Time.deltaTime;
            menuCamera.transform.position = Vector3.Lerp(menuCamera.transform.position, selectCharacterPosition.transform.position, step);
        }
        else
        {
            float step = speed * Time.deltaTime;
            menuCamera.transform.position = Vector3.Lerp(menuCamera.transform.position, mainMenuPosition.transform.position, step);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void OnGUI()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;

        if (splash)
        {
            Splash();
        }
        else if (showMainMenu && showOptionsMenu)
        {
            OptionsMenu();
        }
        else if (showMainMenu)
        {
            MainMenu();
        }
        else if (showSelectCharacterMenu)
        {
            SelectCharacterMenu();
        }
        else if (inGame && showPauseMenu && showOptionsMenu)
        {
            OptionsMenu();
        }
        else if (inGame && showPauseMenu)
        {
            PauseMenu();

        }
    }

    void Splash()
    {
        GUI.DrawTexture(new Rect(0, 0, scrW * 16, scrH * 9), splashTexture, ScaleMode.StretchToFill);
    }

    void EndSplash()
    {
        splash = false;
        showMainMenu = true;
    }

    void MainMenu()
    {
        GUI.Box(new Rect(scrW * 7, scrH * 2, scrW * 3, scrH), "Some Game");

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 4, scrW * 2, scrH), "Start"))
        {
            showMainMenu = false;
            showSelectCharacterMenu = true; ;
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 5.5f, scrW * 2, scrH), "Options"))
        {
            showOptionsMenu = true;
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 7, scrW * 2, scrH), "Quit"))
        {
            Application.Quit();
        }
    }

    void SelectCharacterMenu()
    {
        GUI.Box(new Rect(scrW * 7, scrH * 2, scrW * 3, scrH), "Select a character.");
        if (GUI.Button(new Rect(scrW * 4f, scrH * 5f, scrW * 2, scrH), "Scientist"))
        {
            showSelectCharacterMenu = false;
            StartGame(true);
        }

        if (GUI.Button(new Rect(scrW * 11f, scrH * 5, scrW * 2, scrH), "Monster"))
        {
            showSelectCharacterMenu = false;
            StartGame(false);
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 7, scrW * 2, scrH), "Back"))
        {
            showSelectCharacterMenu = false;
            showMainMenu = true;
        }
    }

    void OptionsMenu()
    {
        if (GUI.Button(new Rect(0, 0, scrW * 16, scrH * 9), "Back"))
        {
            showOptionsMenu = false;
        }
    }

    void PauseMenu()
    {
        GUI.Box(new Rect(scrW * 7, scrH * 2, scrW * 3, scrH), "Game Paused");

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 4, scrW * 2, scrH), "Resume"))
        {
            PauseGame();
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 5.5f, scrW * 2, scrH), "Options"))
        {
            showOptionsMenu = true;
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 7, scrW * 2, scrH), "Back to main menu"))
        {
            PauseGame();
            ToMenu();
        }
    }

    void StartGame(bool asScientist)
    {
        showMainMenu = false;
        inGame = true;
        menuCamera.SetActive(false);
        if (asScientist)
        {
            game1Scientist.SetActive(true);
            game1Monster.SetActive(true);
        }
        else
        {
            game2Scientist.SetActive(true);
            game2Monster.SetActive(true);
        }
    }

    void PauseGame()
    {
        showPauseMenu = !showPauseMenu;

        if (showPauseMenu)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void ToMenu()
    {
        showMainMenu = true;
        showSelectCharacterMenu = false;
        showOptionsMenu = false;
        inGame = false;
        showPauseMenu = false;

        menuCamera.SetActive(true);
        game1Scientist.transform.position = game1ScientistSpawn.transform.position;
        game1Scientist.SetActive(false);
        game1Monster.transform.position = game1MonsterSpawn.transform.position;
        game1Monster.SetActive(false);
        game2Scientist.transform.position = game2ScientistSpawn.transform.position;
        game2Scientist.SetActive(false);
        game2Monster.transform.position = game2MonsterSpawn.transform.position;
        game2Monster.SetActive(false);
    }
}
