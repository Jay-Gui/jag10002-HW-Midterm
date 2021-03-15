using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float forceAmount = 5; //public var for force amount

    public GameObject _GameManager; //access GameManager object

    private GameManager script; //access the script on the GameManager
    
    Rigidbody rb; //var for the Rigidbody

    //static variable means the value is the same for all the objects of this class type and the class itself
    public static PlayerController instance; //this static var will hold the Singleton

    //create the singleton for the player
    void Awake()
    {
        // if (instance == null)  //instance hasn't been set yet
        // {
        //     DontDestroyOnLoad(gameObject);  //Dont Destroy this object when you load a new scene
        //     instance = this; //set instance to this object
        // }
        // else //if the instance is already set to an object
        // {
        //     Destroy(gameObject);  //destroy this new object, so there is only ever one
        // }
    }

    void Start()
    {
        script = _GameManager.GetComponent<GameManager>(); //get the script off this gameObject
        rb = GetComponent<Rigidbody>();  //get the Rigidbody off of this gameObject
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) //if W is pressed
        {
            rb.AddForce(transform.forward * forceAmount);
            //rb.AddForce(Vector3.forward * forceAmount); //apply to the up multiplied by the "force" var
        }

        if (Input.GetKey(KeyCode.S)) //if S is pressed
        {
            rb.AddForce(-transform.forward * forceAmount);
            //rb.AddForce(Vector3.back * forceAmount); //apply to the up multiplied by the "force" var
        }

        if (Input.GetKey(KeyCode.A)) //if A is pressed
        {
            rb.AddForce(-transform.right * forceAmount);
            //rb.AddForce(Vector3.left * forceAmount); //apply to the up multiplied by the "force" var
        }

        if (Input.GetKey(KeyCode.D)) //if D is pressed
        {
            rb.AddForce(transform.right * forceAmount);
            //rb.AddForce(Vector3.right * forceAmount); //apply to the up multiplied by the "force" var
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            //Destroy(this.gameObject);
            script.UpdateHighScores(); //when you die, record the High Scores
            script.isGame = false; //also, we're not in the game anymore
            SceneManager.LoadScene(1);
        }
    }
}