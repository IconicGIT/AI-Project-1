using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopBB : MonoBehaviour
{
    public TextMesh infoText;
    public TextMesh agentTypeText;
    public GameObject camObject;

    public List<GameObject> robbersToApproach;
    public GameObject nextRobber;
    public bool debugTarget;


    public void NotifyRobber(GameObject robber)
    {
        //print("Cop robber: " + robber.name);
        nextRobber = robber;
        
        if (!robbersToApproach.Contains(robber))
            robbersToApproach.Add(robber);
    }
    public void ErradicateEntity(GameObject entity)
    {
        nextRobber = null;
        robbersToApproach.Remove(entity);
        entity.GetComponent<RobberBB>().Fly();
    }

    void Start()
    {
        agentTypeText.text = "Cop";
        agentTypeText.color = Color.blue;
    }
    void Update()
    {
        

        infoText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);
        agentTypeText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);

        infoText.text = "Wandering";
        infoText.color = Color.green;

        if (nextRobber)
        {
            if (debugTarget)
                Debug.DrawLine(transform.position, nextRobber.transform.position, Color.blue); 
            infoText.text = "Approaching Robber";
            infoText.color = Color.red;
        }


    }
}
