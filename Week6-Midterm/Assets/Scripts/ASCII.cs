using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class ASCII : MonoBehaviour
{
    // Remember: ascii are codes for letters; takes the form of characters on the keyboard 
    
    public float xOffset; //move along the x in inspector
    public float yOffset; //move along the y in inspector 
    public float zOffset;
    
    public GameObject player;
    public GameObject wallA, wallB;
    public GameObject asteroid;
    public GameObject boids; 
    public GameObject prize;

    //variable for the current player
    public GameObject currentPlayer;

    //variable for the start position of the player
    private Vector3 startPos; 

    //name of the level file
    public string fileName;

    //current level variable
    public int currentLevel = 0;

    //property that wraps up currentLevel
    //when the level changes, we load that specific level 
    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            LoadLevel();
        }
    }

    //empty game object that holds the level
    public GameObject level; 
    
    void Start()
    {
        LoadLevel(); //call the LoadLevel function in start
    }

    // function that creates a level based on the ASCII Text file 
    void LoadLevel()
    {
        Destroy(level); //destroy the current level
        level = new GameObject("Level"); //create a new level gameObject
        
        //build up a new level path based on the current level file
        string current_file_path = //build the path to the level file
            Application.dataPath +
            "/Levels/" +
            fileName.Replace("Num", currentLevel + "");
           

        //instead of reading a chunk of strings, we read each individual element of an array
        string[] fileLines = File.ReadAllLines(current_file_path);

        //loops through each line
        for (int z = 0;
            z < fileLines.Length;
            z++) // chose y because reading down lines is like reading down the y axis of a page
        {
            //go to each line in a row
            string lineText = fileLines[z];

            //convert lines into an array of characters
            char[] characters = lineText.ToCharArray(); //create characters array

            //loop through each character 
            for (int x = 0; x < characters.Length; x++) //length of the characters in the file
            {
                //grabs the current character
                char c = characters[x];

                GameObject newObj; //creates a holder for all those characters

                //we use a switch function to switch between each character on the fly and instantiate game objects
                //(we use "c" for "character" in this case
                switch (c)
                {
                    case 'p':
                        newObj = Instantiate(player);
                        //if we don't have a current player
                        if (currentPlayer == null)
                        {
                            //then make this the current player
                            currentPlayer = newObj;
                        }
                        Debug.Log(startPos);
                        //startPos = new Vector3(x + xOffset, yOffset, -z + zOffset); //save this position to use for resetting the player
                        break;
                    case 'a':
                        newObj = Instantiate(wallA);
                        break;
                    case 'b':
                        newObj = Instantiate(wallB);
                        break;
                    case 'c':
                        newObj = Instantiate(asteroid);
                        break;
                    case 'd':
                        newObj = Instantiate(boids);
                        break;
                    // case '&':
                    //     newObj = Instantiate(prize);
                    //     break;
                    default:
                        newObj = null;
                        break;
                }

                //did we create a new object?
                if (newObj != null)
                {
                    if (!newObj.name.Contains("Player")) //if new object is not the player
                    {
                        newObj.transform.parent = level.transform; //parent it to the level
                    }
                    //if so, add the offsets that we insert into inspector to the x and y vectors to the position of the new object
                    //-y makes it so that what we put in the txt correlates with how things appear on the game screen
                    newObj.transform.position = new Vector3(x + xOffset, yOffset, -z + zOffset);
                }
            }
        }
    }

    //this resets the player position 
    public void ResetPlayer()
    {
        //player returns to the starting position
        currentPlayer.transform.position = startPos;
        Debug.Log(startPos);
    }
}

//TWO LEVELS, LEVEL0: ASTEROIDS SIT STILL AS YOU MOVE TO GOAL PLANET, LEVEL1: ASTEROID BOIDS MOVE TO YOU AS YOU MOVE TO GOAL PLANET
