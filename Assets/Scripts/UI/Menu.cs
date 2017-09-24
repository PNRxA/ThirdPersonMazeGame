using System;
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
    public bool win = false;
    public bool gameOver = false;
    public bool showPauseMenu = false;
    public GameObject menuCamera;
    public Texture2D splashTexture;
    private float scrH, scrW;
    public GameObject game1Scientist, game1Monster, game2Scientist, game2Monster;
    public GameObject selectCharacterPosition, mainMenuPosition;
    public GameObject game1ScientistSpawn, game1MonsterSpawn, game2ScientistSpawn, game2MonsterSpawn;
    public float speed = 10f;
    private float timeLimit = 300f;
    public static int health = 5;

    void Awake()
    {
        //After 2 seconds stop showing the splashscreen
        Invoke("EndSplash", 2f);
    }

    void FixedUpdate()
    {
        //Move the camera based on menu navigation
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
        game1Scientist.transform.position = game1ScientistSpawn.transform.position;
        game1Scientist.transform.rotation = game1ScientistSpawn.transform.rotation;
        game1Scientist.SetActive(false);
        game1Monster.transform.position = game1MonsterSpawn.transform.position;
        game1Monster.transform.rotation = game1MonsterSpawn.transform.rotation;
        game1Monster.SetActive(false);
        game2Scientist.transform.position = game2ScientistSpawn.transform.position;
        game2Scientist.transform.rotation = game2ScientistSpawn.transform.rotation;
        game2Scientist.SetActive(false);
        game2Monster.transform.position = game2MonsterSpawn.transform.position;
        game2Monster.transform.rotation = game2MonsterSpawn.transform.rotation;
        game2Monster.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Run the pause function when pressing escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (gameOver == true && showPauseMenu == false)
        {
            PauseGame();
        }
    }

    void OnGUI()
    {
        //Define the screen height and width for element placement
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;

        //Determine what menu to show based on booleans 
        if (gameOver && win)
        {
            GameOverScreen("You Win!");
        }
        else if (gameOver)
        {
            GameOverScreen("Game Over!");
        }
        else if (splash)
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
        else
        {
            HUD();
        }
    }

    void HUD()
    {
        GUI.Box(new Rect(scrW, scrH, scrW * 3f, scrH), timeLimit.ToString());
        timeLimit -= Time.deltaTime;

        GUI.Box(new Rect(scrW * 5, scrH, scrW * 2, scrH), "Lives " + health);
    }

    //Function for splash screen
    void Splash()
    {
        GUI.DrawTexture(new Rect(0, 0, scrW * 16, scrH * 9), splashTexture, ScaleMode.StretchToFill);
    }

    //Function to stop showing splash screen
    void EndSplash()
    {
        splash = false;
        showMainMenu = true;
    }

    //Function for main menu
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

    //Function for select character menu
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

    //Function for the options menu
    void OptionsMenu()
    {
        if (GUI.Button(new Rect(0, 0, scrW * 16, scrH * 9), "Back"))
        {
            showOptionsMenu = false;
        }
    }

    //Function for the pause menu 
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

    void GameOverScreen(string message)
    {
        GUI.Box(new Rect(scrW * 7, scrH * 2, scrW * 3, scrH), message);

        if (GUI.Button(new Rect(scrW * 6.5f, scrH * 4, scrW * 4, scrH), "Back to main menu"))
        {
            PauseGame();
            ToMenu();
        }
    }

    //Function to start the game
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

    //Function to pause the game
    void PauseGame()
    {
        showPauseMenu = !showPauseMenu;

        if (showPauseMenu)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //Function to go from playing the game back to the main menu
    void ToMenu()
    {
        health = 5;
        showMainMenu = true;
        showSelectCharacterMenu = false;
        showOptionsMenu = false;
        inGame = false;
        showPauseMenu = false;
        gameOver = false;
        win = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        showPauseMenu = false;
        timeLimit = 300f;

        menuCamera.SetActive(true);
        game1Scientist.transform.position = game1ScientistSpawn.transform.position;
        game1Scientist.transform.rotation = game1ScientistSpawn.transform.rotation;
        game1Scientist.SetActive(false);
        game1Monster.transform.position = game1MonsterSpawn.transform.position;
        game1Monster.transform.rotation = game1MonsterSpawn.transform.rotation;
        game1Monster.SetActive(false);
        game2Scientist.transform.position = game2ScientistSpawn.transform.position;
        game2Scientist.transform.rotation = game2ScientistSpawn.transform.rotation;
        game2Scientist.SetActive(false);
        game2Monster.transform.position = game2MonsterSpawn.transform.position;
        game2Monster.transform.rotation = game2MonsterSpawn.transform.rotation;
        game2Monster.SetActive(false);
    }
}
