using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Menu navigation logic
    private bool splash = true;
    private bool showMainMenu = false;
    private bool showSelectCharacterMenu = false;
    private bool showOptionsMenu = false;
    private bool inGame = false;
    public bool win = false;
    public bool gameOver = false;
    public bool showPauseMenu = false;

    // Camera to see the menu screens
    public GameObject menuCamera;
    // Splash screen art
    public Texture2D splashTexture;
    // GUI screen height and screen width
    private float scrH, scrW;
    // Characters
    public GameObject game1Scientist, game1Monster, game2Scientist, game2Monster;
    // Move to position for the select character menu
    public GameObject selectCharacterPosition, mainMenuPosition;
    // Spawn points for AI and player
    public GameObject game1ScientistSpawn, game1MonsterSpawn, game2ScientistSpawn, game2MonsterSpawn;
    public float speed = 10f;

    // Time limit to complete the level
    private float timeLimit = 300f;

    // Player's health
    public static int health = 3;

    //Options menu member variables
    public AudioSource audi;
    public Vector2 resScrollPosition = Vector2.zero;
    public float audioSlider;
    //Resolution drop down menu will be set to screen resolution
    private string buttonName = Screen.width + "x" + Screen.height;
    private bool showResOptions = false;
    private bool fullscreenToggle;

    DoorMove[] doorMove;

    void Awake()
    {
        //After 2 seconds stop showing the splashscreen
        Invoke("EndSplash", 2f);
        //Grab all doors to be able to reset them when resetting level
        doorMove = FindObjectsOfType<DoorMove>();
        //Set audio source
        audi = GetComponent<AudioSource>();
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
        // Set player and AI to their spawn points
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

        // Set toggle to current fullscreen status
        fullscreenToggle = Screen.fullScreen;

        // If there are player prefs load them in
        if (PlayerPrefs.HasKey("volume"))
        {
            // Set audio
            audioSlider = PlayerPrefs.GetFloat("volume");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Run the pause function when pressing escape
        if (Input.GetKeyDown(KeyCode.Escape) && inGame)
        {
            PauseGame();
        }

        if (gameOver == true && showPauseMenu == false)
        {
            PauseGame();
        }

        if (audioSlider != audi.volume)
        {
            audi.volume = audioSlider;
        }
    }

    void OnGUI()
    {
        // Define the screen height and width for element placement
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;

        // Determine what menu to show based on booleans 
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
            if (showResOptions)
            {
                ResOptionsFunc();
            }
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
            if (showResOptions)
            {
                ResOptionsFunc();
            }
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
        // Display time remaining
        GUI.Box(new Rect(scrW, scrH, scrW * 3f, scrH), timeLimit.ToString());
        // Decrease time per second
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            gameOver = true;
        }
        // Displays health
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
            // Exit the game
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
            // Start game (as scientist)
            StartGame(true);
        }

        if (GUI.Button(new Rect(scrW * 11f, scrH * 5, scrW * 2, scrH), "Monster"))
        {
            showSelectCharacterMenu = false;
            // Start game (as other character)
            StartGame(false);
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 7, scrW * 2, scrH), "Back"))
        {
            showSelectCharacterMenu = false;
            showMainMenu = true;
        }
    }

    //Function for the options menu
    //Show options that the player can change
    void OptionsMenu()
    {
        GUI.Box(new Rect(scrW, scrH, scrW * 14, scrH * 6), "Options");

        GUI.Box(new Rect(scrW * 2, scrH * 2, scrW * 4, scrH * 4), "Graphics");

        GUI.Box(new Rect(scrW * 6, scrH * 2, scrW * 4, scrH * 4), "Sound");
        //Used to have more options... R.I.P
        GUI.BeginGroup(new Rect(scrW * 6.5f, scrH * 2.5f, scrW * 4, scrH * 4));
        audioSlider = GUI.HorizontalSlider(new Rect(0, scrH, 3 * scrW, .5f * scrH), audioSlider, 0f, 1f);
        GUI.EndGroup();

        GUI.Box(new Rect(scrW * 10, scrH * 2, scrW * 4, scrH * 4), "Screen");

        GUI.BeginGroup(new Rect(scrW * 10.5f, scrH * 2.5f, scrW * 3, scrH * 5f));

        if (GUI.Button(new Rect(0, 0, scrW * 3, scrH * .5f), buttonName))
        {
            // Show res dropdown menu
            showResOptions = !showResOptions;
        }

        // Only show fullscreen toggle when resolution dropdown isn't shown (to avoid accidental toggling when in dropdown)
        if (!showResOptions)
        {
            fullscreenToggle = GUI.Toggle(new Rect(0, scrH, scrW * 3, scrH * .5f), fullscreenToggle, "Toggle Fullscreen");

            Screen.fullScreen = fullscreenToggle;
        }

        GUI.EndGroup();

        if (GUI.Button(new Rect(scrW * 7f, scrH * 8, scrW * 2, scrH), "Save & Back"))
        {
            // Go back to previous menu and also save options
            SaveOptions();
            showOptionsMenu = false;
            showResOptions = false;
        }
    }

    void ResOptionsFunc()
    {
        // Set up resolutions for button labels
        string[] res = new string[] { "1024×576", "1152×648", "1280×720", "1280×800", "1366×768", "1440×900", "1600×900", "1680×1050", "1920×1080", "1920×1200", "2560×1440", "2560×1600", "3840×2160" };

        // Set up resolution values to set (TODO could be improved)
        int[] resW = new int[] { 1024, 1152, 1280, 1280, 1366, 1440, 1600, 1680, 1920, 1920, 2560, 2560, 3840 };
        int[] resH = new int[] { 576, 648, 720, 800, 768, 900, 900, 1050, 1080, 1200, 1440, 1600, 2160 };

        // Create GUI style solid black (kek) for scrollable resolutions
        Texture2D black = new Texture2D(1, 1);
        black.SetPixel(1, 1, Color.black);
        GUIStyle solidBlack = new GUIStyle();
        solidBlack.normal.background = black;


        // Group for the drop down menu
        GUI.BeginGroup(new Rect(scrW * 10.5f, scrH * 3, scrW * 3, scrH * 4));

        resScrollPosition = GUI.BeginScrollView(new Rect(0, 0, scrW * 3, scrH * 4), resScrollPosition, new Rect(0, 0, scrW * 2.6f, scrH * 13));

        GUI.Box(new Rect(0, 0, scrW * 3, scrH * 13), "", solidBlack);

        for (int i = 0; i < 13; i++)
        {
            if (GUI.Button(new Rect(0, scrH * i, scrW * 2.7f, scrH), res[i]))
            {
                // Set resolution based on which button was pressed (array[i] name, array[i] width, array[i] height)
                Screen.SetResolution(resW[i], resH[i], Screen.fullScreen);
                buttonName = res[i];
                showResOptions = false;
            }
        }

        GUI.EndScrollView();

        GUI.EndGroup();
    }

    // Save all options
    void SaveOptions()
    {
        PlayerPrefs.SetFloat("volume", audioSlider);
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
            // Go back to main menu and reset everything
            ToMenu();
        }
    }

    void GameOverScreen(string message)
    {
        GUI.Box(new Rect(scrW * 7, scrH * 2, scrW * 3, scrH), message);

        if (GUI.Button(new Rect(scrW * 6.5f, scrH * 4, scrW * 4, scrH), "Back to main menu"))
        {
            PauseGame();
            // Go back to main menu and reset everything
            ToMenu();
        }
    }

    //Function to start the game
    void StartGame(bool asScientist)
    {
        showMainMenu = false;
        inGame = true;
        // Turn off menu carmera and activate player in chosen level
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
        // Toggle pause menu
        showPauseMenu = !showPauseMenu;

        // Un/freeze time, un/lock cursor and un/hide cursor
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
        // Set EVERYTHING back to default / starting values
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

        //Reset science doors
        foreach (var door in doorMove)
        {
            door.Close();
        }
    }
}
