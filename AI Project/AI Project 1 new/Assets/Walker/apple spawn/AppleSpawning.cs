using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawning : MonoBehaviour
{
    public int timer_ref;
    public int timer;
    public GameObject apple;
    public ApplesController applesController;
    public float appleSpawnRadius;

    int applesSpawned;

    // Start is called before the first frame update
    void Start()
    {
        var part = timer_ref / 4;
        timer = timer_ref/2 + (int)Random.Range(-part, part);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            
            timer--;
        }
        else
        {
            GameObject newApple = GameObject.Instantiate(apple, transform.position + new Vector3(Random.Range(-appleSpawnRadius, appleSpawnRadius), 10, Random.Range(-appleSpawnRadius, appleSpawnRadius)), Quaternion.identity);
            newApple.name = ("Apple " + applesSpawned);
            applesSpawned++;
            applesController.UpdateApples();
            var part = timer_ref / 3;
            timer = timer_ref + (int)Random.Range(-part, part);
        }
    }
}
