using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBB : MonoBehaviour
{
    public GameObject cop;
    public void GetRobbed(GameObject robber)
    {
        print("Robber: " + robber.name);
        int rnd = UnityEngine.Random.Range(0, 100);

        if (rnd < 50)
        {
            print(gameObject.name + " noticied a robbery! Call the Cop!");

            cop.GetComponent<CopBB>().NotifyRobber(robber);
        }
        else
        {
            //print(gameObject.name + " was robbed without noticing.");
            

        }

    }
}
