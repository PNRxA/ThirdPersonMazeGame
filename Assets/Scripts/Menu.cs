using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool splash = true;
    private bool showMainMenu = false;
	public GameObject player;
	public GameObject player2;
	public GameObject menuCamera;
    public Texture2D splashTexture;
    private float scrH, scrW;

    void Awake()
    {
        Invoke("EndSplash", 2f);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;

        if (splash)
        {
            Splash();
        }
        else if (showMainMenu)
        {
            MainMenu();
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
			menuCamera.SetActive(false);
			player.SetActive(true);
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 5.5f, scrW * 2, scrH), "Options"))
        {

        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 7, scrW * 2, scrH), "Quit"))
        {

        }
    }
}
