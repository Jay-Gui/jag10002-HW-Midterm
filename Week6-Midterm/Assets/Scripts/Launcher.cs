using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;

public class Launcher : MonoBehaviour
{
    //grab firePoint position(s)
    public Transform firePoint;
    
    //grab prefab rigidbody
    [FormerlySerializedAs("projectilePrefab")] 
    public Rigidbody prefabRB;
    
    //add a force value for launching the prefab
    [SerializeField]
    private float launchForce = 200000f;
    
    //grab the main camera
    public Camera playerCam; 
    
    //set the center of the screen
    private float x = Screen.width / 2;
    private float y = Screen.height / 2;
    
    void Update()
    {
        //if you left click, fire with prefab-launcher function
        if (Input.GetButtonDown("Fire1"))
        {
            LaunchProjectile(); 
        }
    }

    // create a prefab-launcher function
    void LaunchProjectile()
    {
        //raycast to the center of the screen
        var ray = playerCam.ScreenPointToRay(new Vector3(x, y));
        
        //instantiate prefab and add force value to it
        var projectileInstance = Instantiate(prefabRB, firePoint.position, firePoint.rotation);
        
            projectileInstance.AddForce(ray.direction * launchForce);
    }
}
