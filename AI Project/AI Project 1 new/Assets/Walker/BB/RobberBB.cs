using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberBB : MonoBehaviour
{
    public GameObject self;
    public bool fly;

    float pos = 0;

    public void Fly()
    {
        fly = true;
    }

    void Update()
    {
        
        if (fly)
        {
            self.transform.position = self.transform.position + new Vector3(0, pos, 0);
            pos += .5f;
        }
    }
}
