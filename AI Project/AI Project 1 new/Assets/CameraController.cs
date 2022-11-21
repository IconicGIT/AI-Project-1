using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float speed;

    [SerializeField]
    float rotSpeed;

    float yaw = 0.0f;
    float pitch = 0.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //print("pressing down");
            yaw += rotSpeed * Input.GetAxis("Mouse X");
            pitch -= rotSpeed * Input.GetAxis("Mouse Y");

        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(2 * speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(2 * -speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0, 2 * -speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(new Vector3(0, 2 * speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, 2 * speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, 2 * -speed * Time.deltaTime));
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }                                      
            if (Input.GetKey(KeyCode.Q))           
            {                                      
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
            }
        }
    }
}

