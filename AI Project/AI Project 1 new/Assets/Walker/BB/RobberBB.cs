using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberBB : MonoBehaviour
{
    public TextMesh infoText;
    public TextMesh agentTypeText;
    public GameObject camObject;

    public GameObject self;
    public GameObject target;
    public bool debugTarget;
    public bool fly;

    float pos = 0;

    public void Fly()
    {
        fly = true;
    }

    void Start()
    {
        agentTypeText.text = "Robber";
        infoText.text = "";
        agentTypeText.color = Color.red;
    }

    void Update()
    {
        if (debugTarget)
            Debug.DrawLine(transform.position, target.transform.position, Color.red);

        infoText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);
        agentTypeText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);

        if (fly)
        {
            self.transform.position = self.transform.position + new Vector3(0, pos, 0);
            pos += .5f;
        }
    }
}
