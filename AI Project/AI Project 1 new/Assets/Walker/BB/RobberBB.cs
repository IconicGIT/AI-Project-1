using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberBB : MonoBehaviour
{
    public GameObject self;
    bool fly;

    float spd = 0;

    public void Fly()
    {
        fly = true;
    }

    void Update()
    {
        
        if (fly)
        {
            self.transform.position = self.transform.position + new Vector3(0,spd,0);
            spd += .1f;
        }
    }
}
