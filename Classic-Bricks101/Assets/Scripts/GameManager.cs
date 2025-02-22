﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//List all the possible gamestates
public enum GameState
{
    NotStarted,
    Playing,
    Completed,
    Failed
}

//Require an audio source for the object
[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    //Sounds to be played when entering one of the gamestates
    public AudioClip StartSound;
    public AudioClip FailedSound;

    private GameState currentState = GameState.NotStarted;

    //All the blocks found in this level, to keep track of how many are left
    private Brick[] allBricks;
    private Ball[] allBalls;
    private Paddle paddle;

    public float Timer = 0.0f;
    private int minutes;
    private int seconds;
    public string formattedTime;

    public Text feedback;
    public Text text;

    public GameObject restartButton;
    public GameObject mainMenuButton;
    public GameObject buttonBackground; //Panel


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        //Find all the blocks in this scene
        allBricks = FindObjectsOfType(typeof(Brick)) as Brick[];

        //Find all the balls in this scene
        allBalls = FindObjectsOfType(typeof(Ball)) as Ball[];

        paddle = GameObject.FindObjectOfType<Paddle>();

        print("Bricks:" +allBricks.Length);
        print("Ball:" + allBalls.Length);
        print("Paddle:" +paddle);

        //Change start text
        ChangeText("Click To Begin");

        //Prepare the start of the level
        SwitchState(GameState.NotStarted);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.NotStarted:
                //Change start text
                ChangeText("Click To Begin");

                //Check if the Player taps/clicks.
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchState(GameState.Playing);
                }
                break;

            case GameState.Playing:
                {
                    Timer += Time.deltaTime;
                    minutes = Mathf.FloorToInt(Timer/60F);
                    seconds = Mathf.FloorToInt(Timer-minutes *60);
                    formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

                    //Change start text
                    ChangeText("Time: "+formattedTime);

                    //Display Time
                    //print(formattedTime);

                    bool allBlocksDestroyed = false;

                    //Are there no balls left?
                    if (FindObjectOfType(typeof(Ball)) == null)
                    {
                        SwitchState(GameState.Failed);
                    }
                    if (allBlocksDestroyed)
                    {
                        SwitchState(GameState.Completed);
                    }
                }
                break;

            //Both cases do the same: restart the game
            case GameState.Failed:
                print("GameState Failed!");
                ChangeText("You Lose :(");

                break;

            case GameState.Completed:
                bool allBlocksDestroyFinal = false;

                //Destroy all the balls
                Ball[] others = FindObjectsOfType(typeof(Ball)) as Ball[];

                foreach (Ball other in others)
                {
                    Destroy(other.gameObject);
                }
                break;

        }
    }

    //Enable Button Script
    public void EnableButtons()
    {
        //Enable buttons for when the player loss
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
        buttonBackground.SetActive(true);
    }

    public void ChangeText(string text)
    {
        //Find Canvas and modify text
        GameObject canvas = GameObject.Find("Canvas");
        Text[] textValue = canvas.GetComponentsInChildren<Text>();
        textValue[0].text = text;
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            default:
            case GameState.NotStarted:
                break;

            case GameState.Playing:
                GetComponent<AudioSource>().PlayOneShot(StartSound);
                break;

            case GameState.Completed:
                GetComponent<AudioSource>().PlayOneShot(StartSound);
                break;

            case GameState.Failed:
                GetComponent<AudioSource>().PlayOneShot(FailedSound);
                break;
        }
    }
}
