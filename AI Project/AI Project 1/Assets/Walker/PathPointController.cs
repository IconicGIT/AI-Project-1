using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathPointController : MonoBehaviour
{

    [SerializeField]
    PathCreator path;

    [SerializeField]
    float StartPosition;

    public float speed;

    float distanceTraveledInPath;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveledInPath += speed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(StartPosition + distanceTraveledInPath);
    }
}
