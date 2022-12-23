using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AirplaneControllerIA : Agent
{
    [SerializeField]
    List<AeroSurface> controlSurfaces = null;
    [SerializeField]
    //List<WheelCollider> wheels = null;
    //[SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;

    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;
    //[SerializeField]
    //Text displayText = null;

    float thrustPercent;
    //float brakesTorque;

    AircraftPhysics aircraftPhysics;
    public List<Rigidbody> bodyParts;
    public int targetTime = 1000;

    [SerializeField]
    public RayPerceptionSensorComponent3D horizontalSensor;
    [SerializeField]
    public RayPerceptionSensorComponent3D verticalSensor;

    [SerializeField]
    List<Collider> airplaneColliders;

    float minHeight = 10;
    float maxHeight = 60;

    public float time;

    [SerializeField]
    CheckCollisions colCheck;

    [SerializeField]
    TextMesh rewardText;
    float reward = 0;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        horizontalSensor = GameObject.Find("horizontal rays").GetComponent<RayPerceptionSensorComponent3D>();
        verticalSensor = GameObject.Find("vertical rays").GetComponent<RayPerceptionSensorComponent3D>();
    }

    public override void OnEpisodeBegin()
    {
        print("restarting...");
        colCheck.Restart();
        reward = 0;
        transform.SetPositionAndRotation(new Vector3(0, 6, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        thrustPercent = 0;
        ////transform.rotation.eulerAngles.Set(-90, 0, 0);

        //foreach (Rigidbody body in bodyParts)
        //{
        //    body.angularVelocity = Vector3.zero;
        //    body.velocity = Vector3.zero;
        //    body.transform.rotation.eulerAngles.Set(-90, 0, 0);

        //}


        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
       
        //raysantamarias

        

        //yaw picth roll thrust
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(Yaw);
        sensor.AddObservation(Pitch);
        sensor.AddObservation(Roll);
        sensor.AddObservation(thrustPercent);

        RayPerceptionInput inputH = horizontalSensor.GetRayPerceptionInput();

        
        //sensor.AddObservation(horizontalSensor.GetRayPerceptionInput());

        //foreach (var thing in horizontalSensor)
        //{
        //    sensor.AddObservation(thing);
        //}



        //foreach (var thing in verticalSensor)
        //{
        //    sensor.AddObservation(thing);
        //}

        //base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
         Pitch = actions.ContinuousActions[0];
         Yaw = actions.ContinuousActions[1];
         //Roll = actions.ContinuousActions[2];
         thrustPercent += actions.ContinuousActions[2];

        if (thrustPercent > 1)
        {
            thrustPercent = 1;
        }
        if (thrustPercent < 0)
        {
            thrustPercent = 0;
        }

       

        //rewards
        {

            //rewards: above certiain time without fliyng / at more ore less at certain Y / not colliding with anything;
            //reward = 0;

            if (transform.position.y < minHeight || transform.position.y > maxHeight)
            {
                //bad boi
                reward -= 0.33f;
            }
            else
            {
                reward += 0.33f;
                //print("Position good " + reward);

            }


            if (time > 1000 * 60 * 15)
            {
                //bad boi
                //reward -= 1 / 3;
                EndEpisode();
            }

            if (colCheck.isColliding)
            {

                reward -= 0.33f;
                EndEpisode();
            }
            else
            {
                reward += 0.33f;
                //print("Collision good " + reward);
            }

            if(reward < -1)
            {
                reward = -1;
            }
            else if(reward > 1)
            {
                reward = 1;
            }

            //print((float)reward);
            

            SetReward(reward);
            //print("reward: " + reward);
            //base.OnActionReceived(actions);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionOut = actionsOut.ContinuousActions;
        //levantar morro
        continuousActionOut[0] = Input.GetAxis("VerticalKB");
        //girar morro izq to der
        continuousActionOut[1] = Input.GetAxis("Yaw");
        //thrust
        continuousActionOut[2] = Input.GetAxis("VerticalArr");

        Pitch = (float)continuousActionOut[0];
        Yaw = (float)continuousActionOut[1];

        //SetControlSurfecesAngles(continuousActionOut[0], Roll, continuousActionOut[1], Flap);
        thrustPercent += (float)continuousActionOut[2] * 0.01f;


        //base.Heuristic(actionsOut);)
    }

    private void Update()
    {
        rewardText.text = "reward: " + reward.ToString();
        
        //rewardText.text = "Yaw: " + Yaw.ToString();
    }
    //private void Update()
    //{
    //    Pitch = Input.GetAxis("Vertical");
    //    Roll = Input.GetAxis("Horizontal");
    //    Yaw = Input.GetAxis("Yaw");



    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        thrustPercent += 0.01f;

    //    }
    //    if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        thrustPercent -= 0.01f;


    //    }

    //    if (thrustPercent > 1)
    //    {
    //        thrustPercent = 1;
    //    }
    //    if (thrustPercent < 0)
    //    {
    //        thrustPercent = 0;
    //    }

    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        Flap = Flap > 0 ? 0 : 0.3f;
    //    }

    //    //if (Input.GetKeyDown(KeyCode.B))
    //    //{
    //    //    brakesTorque = brakesTorque > 0 ? 0 : 100f;
    //    //}

    //  // displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
    //   //displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
    //   //displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
    //   //displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";
    //}

    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
        //foreach (var wheel in wheels)
        //{
        //wheel.brakeTorque = brakesTorque;
        // small torque to wake up wheel collider
        //wheel.motorTorque = 0.01f;
        //}
    }

    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        //print("Plane Yaw"+Yaw.ToString());
        //print("Local Yaw" + yaw.ToString());
        foreach (var surface in controlSurfaces)
        {
            

            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                    break;
            }

           
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }
}
