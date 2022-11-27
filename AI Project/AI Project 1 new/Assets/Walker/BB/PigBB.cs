using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBB : MonoBehaviour
{
    public TextMesh infoText;
    public TextMesh agentTypeText;
    public GameObject camObject;

    public ApplesController appleController;

    public bool debugApple;
    public GameObject currentApple;

    public void GetNearestApple()
    {
        if (appleController.apples.Count > 0)
        {
            appleController.CleanAppleList();

            float nextAppleDistance = Mathf.Infinity;

            foreach (GameObject apple in appleController.apples)
            {
                if (apple && Vector3.Distance(transform.position, apple.transform.position) < nextAppleDistance) 
                {
                    nextAppleDistance = Vector3.Distance(transform.position, apple.transform.position);
                    currentApple = apple;
                }
            }
            
        }
       
    }

    void Start()
    {
        agentTypeText.text = "PIG";
        infoText.text = "";
        agentTypeText.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentApple && debugApple)
            Debug.DrawLine(transform.position, currentApple.transform.position, Color.magenta);

        infoText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);
        agentTypeText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);
    }
}
