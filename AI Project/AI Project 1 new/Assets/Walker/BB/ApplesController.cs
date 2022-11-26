using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplesController : MonoBehaviour
{
    public List<GameObject> apples;

    public void UpdateApples()
    {
        GameObject[] applesTemp = GameObject.FindGameObjectsWithTag("apple");
        apples.Clear();
        apples = new List<GameObject>(applesTemp);
        CleanAppleList();
       
    }

    public void CleanAppleList()
    {
        foreach (GameObject apple in apples)
        {
            if (!apple)
            {
                apples.Remove(apple);
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
