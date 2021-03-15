using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "FirePoint")
        {
            Destroy(gameObject);
            GameManager.instance.score++;
            print("Score: " + GameManager.instance.score);
        }
    }
}
