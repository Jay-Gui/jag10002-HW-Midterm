using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
   //grab prefab of boid to spawn
    public GameObject boidPrefab;
    
    //create a boid list (boids need to check against all the other boids) / make a list of boid behaviors 
    public List<BoidBehavior> listOfBoids = new List<BoidBehavior>();
    
    //public List<GameObject> listOfPilots = new List<GameObject>();
    
    public List<GameObject> listOfAsteroids = new List<GameObject>();

    //declare a target for the boids to "seek" or, rather, move towards
    public Transform target; 
       
    //BOIDS STATS
    //amount of boids in the flock
    public int numberOfBoids;
    //max speed of boids and the distance between each (or how far the boids try and keep away from each other)
    public float maxSpeed, separationDist;
    
    //BOID SPECIAL SETTINGS/WEIGHTINGS: alignment, cohesion, separation, and seeking (also known as steering behaviors)
    //could add these up to average them, but sometimes it's better to have certain weightings be weaker than others
    //for instance, alignment doesn't really matter as much as separation
    //also, keep the cohesion value very low in the inspector
    //seeking literally denotes seeking the target
    public float weight_alignment, weight_cohesion, weight_separation, weight_seeking, weight_avoidance;

    void Awake()
    {
        //a for loop that spawns the numberOfBoids that we have set in the inspector
         for (int i = 0; i < numberOfBoids; i++)
         {
             SpawnBoid();
         }

         //listOfAsteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList(); 
    }

    //a function that spawns the boid prefabs
    void SpawnBoid()
    {
        //newBoid variable instantiates the prefabs:
        //At the same position as the BoidManager GameObject (e.g. this.transform.position),
        //Quaternions are used to represent rotations; .identity makes it so that the rotation vectors in the prefab's inspector are applied...
        //(so because I never edited the rotation vectors on the prefab, it'll spawn "standing straight" for lack of a better phrase,
        //this.transform is a rare fourth argument within Instantiate; it makes the BoidController the parent of the boid prefabs. 
        GameObject newBoid = Instantiate(boidPrefab, this.transform.position, Quaternion.identity, this.transform);
        //listOfPilots.Add(newBoid);

        //newBoidBehav is a BoidBehavior variable that grabs the similarly titled "BoidBehavior" script/component from a separate GameObject
        BoidBehavior newBoidBehav = newBoid.GetComponent<BoidBehavior>();
        //add the BoidBehavior script to the listOfBoids list
        listOfBoids.Add(newBoidBehav); 
        
        //whenever we create a boid it has a link back to the manager so that we can get
        //information off of said manager
        newBoidBehav.myManager = this;
        
        //make the speed from BoidBehavior script equal the maxSpeed here
        newBoidBehav.speed = maxSpeed; 
    }
}
