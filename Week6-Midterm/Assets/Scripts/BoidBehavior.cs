using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    //core variables for movement
    //going to be making our own physics system; going to move them on our own
    //we could use Rigidbodies for each prefab, but that's expensive) 
    Vector3 velocity;
    public float speed; 
    
    //all the steering behaviors are their own vector 3s
    Vector3 alignment, separation, cohesion, seeking, avoidance; 
    
    //we need a manager variable/reference because we need the listOfBoids list that we made
    public BoidManager myManager;

    void Start()
    {
        //set random velocity on the x,y,z vectors
        //change later though
        velocity = new Vector3((Random.value - 0.5f) * 2f, 
            (Random.value - 0.5f) * 2f, 
            (Random.value - 0.5f) * 2f);
        
        //makes initial direction face velocity; so the this.transform.up = top of the cylinder boid (can change this btw),
        //so the top will move in the direction of the velocity
        this.transform.forward = velocity.normalized;
    }

    //this will be the core loop of steering behavior
    void Update()
    {
       //stores our current position 
       Vector3 currentPos = this.transform.position;

       //initialize the behavior/steering vectors and set them to zero
       alignment = Vector3.zero;
       separation = Vector3.zero;
       cohesion = Vector3.zero;
       seeking = Vector3.zero;
       avoidance = Vector3.zero;
       
       for(var i = myManager.listOfBoids.Count - 1; i > -1; i--)
       {
           if (myManager.listOfBoids[i] == null)
               myManager.listOfBoids.RemoveAt(i);
       }

       //let's compare ourselves against all other boids
       foreach (BoidBehavior otherBoid in myManager.listOfBoids)
       {
           if (otherBoid == this) continue; //if the other boid is me, skip it 

           //the normalized velocity of the otherBoid is its facing direction (refer to the this.transform.forward in Start)
           alignment += otherBoid.velocity.normalized;
           
           //adds up all the positions of the otherBoids
           cohesion += otherBoid.transform.position;
           
           //for separation we introduce an if statement; directBetween = direction between this boid and otherBoid
           Vector3 directBetween = this.transform.position - otherBoid.transform.position; //direction between us and them

           // if we're too close
           if (directBetween.magnitude > 0 && directBetween.magnitude < myManager.separationDist)
           {
               //scales the distance; if you're really close, 
               //the closer you are to me, the more I try to move away
               separation += (directBetween.normalized / directBetween.magnitude * myManager.separationDist);
           }
       }

       //for the asteroids
       foreach (GameObject asteroid in myManager.listOfAsteroids)
       {
           Vector3 directBetween = this.transform.position - asteroid.transform.position;
           if (directBetween.magnitude > 0 && directBetween.magnitude < myManager.separationDist)
           {
               avoidance += (directBetween.normalized / directBetween.magnitude * myManager.separationDist);
           }
       }

       //does not need to be in the foreach loop because we're just going to one place
       //the target - the boids gives the vector for how the voids get to the target 
       seeking = myManager.target.position - this.transform.position;

       //we need to average the behavior vectors
       //we don't need to average "seeking" but we do add it to the newVelocity block below
       var divisor = myManager.listOfBoids.Count -1 <= 0 ? 1 : myManager.listOfBoids.Count - 1;
       alignment /= (divisor); //- 1); 
       cohesion /= (divisor); //- 1);
       cohesion  -= this.transform.position; //turn it into a direction (not entirely sure why)
       separation /= (divisor); //- 1);
       avoidance /= (divisor); //- 1);
       
       Vector3 newVelocity = Vector3.zero; 
       newVelocity += alignment * myManager.weight_alignment;
       newVelocity += cohesion * myManager.weight_cohesion;
       newVelocity += separation * myManager.weight_separation;
       newVelocity += seeking * myManager.weight_seeking; 
       newVelocity += avoidance * myManager.weight_avoidance; 

       //the lerp creates a loop/oscillation effect 
       velocity = Vector3.Lerp(velocity, newVelocity, Time.deltaTime * 0.2f); 
       
       //and here is where we limit the velocity (Limit function at the bottom of this script)
       //by setting it to the maxSpeed in the BoidManager script
       //velocity = Limit(velocity, myManager.maxSpeed);
       velocity = Vector3.ClampMagnitude(velocity, myManager.maxSpeed);

       //just like what we did in Start, face the way that we are going
       this.transform.forward = velocity.normalized; 
       
       //add our current position to the velocity and multiply the sum of this
       //as well as the speed with Time.deltaTime to make the boids move
       //Debug.Log(velocity);
       this.transform.position = currentPos + velocity * (Time.deltaTime * speed); 
    }
    
    //cap the speed with a clamping function because if the target gets far away, the boid speed increases at a drastic amount
    Vector3 Limit(Vector3 vectorToCLamp, float maxLength)
     {
         if (vectorToCLamp.magnitude > maxLength)
         {
             return vectorToCLamp.normalized * maxLength;
         }
         else
         {
             return vectorToCLamp; 
         }
     }
}
