using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization; 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //static variable means the value is the same for all the objects of this class type and the class itself
    public static GameManager instance; //this static var will hold the Singleton

    int currentLevel = 0;

    public int score;

    public Text text;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    //public int highScore = 0;
    List<int> highScores;

    //bool for whether or not we are in the game
    public bool isGame = true;

    //file to store high scores
    const string FILE_HIGH_SCORES = "/highScores.txt";

    //full path to high scores
    string FILE_PATH_HIGH_SCORES;

    //create the singleton for the GameManager
    void Awake()
    {
        if (instance == null) //instance hasn't been set yet
        {
            DontDestroyOnLoad(gameObject); //Dont Destroy this object when you load a new scene
            instance = this; //set instance to this object
        }
        else //if the instance is already set to an object
        {
            Destroy(gameObject); //destroy this new object, so there is only ever one
        }
    }

    void Start()
    {
        //create the full path to the high score file
        //using Application.dataPath, which will find
        //the correct location regardless of the platform
        //you are using
        FILE_PATH_HIGH_SCORES = Application.dataPath + FILE_HIGH_SCORES;

        //if this file does not exist,
        if (!File.Exists(FILE_PATH_HIGH_SCORES))
        {
            //then create the file
            File.Create(FILE_PATH_HIGH_SCORES);
        }
    }

    void Update()
    {
        //write the score to the canvas
        text.text = "Score: " + score;

        //if we're not in the game, display the high scores
        if (!isGame)
        {
            UpdateHighScores();
            
            string HighScoreString = "High Scores\n\n";
            
            for (int i = 0; i < highScores.Count; i++)
            {
                HighScoreString += highScores[i] + "\n";
            }

            text.text = HighScoreString; 
        }
    }

    public void UpdateHighScores()
        {
            //if there's no highScores list,
            if (highScores == null)
            {
                //then make the list
                highScores = new List<int>();

                //fileContents reads from the HIGH_SCORES file
                string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);

                //separate the high scores in the file with a pipe
                string[] fileScores = fileContents.Split(',');

                //print(fileScores.Length);

                for (int i = 0; i < fileScores.Length - 1; i++)
                {
                    highScores.Add(Int32.Parse(fileScores[i]));
                }
            }
            
            if (score > highScores.Max()) highScores.Add(score);

            // for (int i = 0; i < highScores.Count; i++)
            // {
            //     //if any new high scores are greater than previous high scores,
            //     if (score > highScores[i])
            //     {
            //         //insert that new high score into the list in the file
            //         highScores.Insert(i, score);
            //         break;
            //     }
            // }

            string saveHighScoreString = "";

            for (int i = 0; i < highScores.Count; i++)
            {
                saveHighScoreString += highScores[i] + " , ";
            }

            File.WriteAllText(FILE_PATH_HIGH_SCORES, saveHighScoreString);
        }
    }