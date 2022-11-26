using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBB : MonoBehaviour
{
    public ApplesController appleController;

    public GameObject currentApple;

    public void GetNearestApple()
    {
        if (appleController.apples.Count > 0)
        {
            appleController.CleanAppleList();

            float nextAppleDistance = float.MaxValue;

            foreach (GameObject apple in appleController.apples)
            {
                if (apple && Vector3.Distance(transform.position, apple.transform.position) < nextAppleDistance) 
                {
                    currentApple = apple;
                }
            }
            
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
