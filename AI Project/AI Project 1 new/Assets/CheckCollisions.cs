using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisions : MonoBehaviour
{
    public bool isColliding = false;
    public void Restart()
    {
        isColliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
       if (!other.CompareTag("beeTarget") == true)
        {
            isColliding = true;
            //print("Triggered with " + other.name);
        }
    }
}
