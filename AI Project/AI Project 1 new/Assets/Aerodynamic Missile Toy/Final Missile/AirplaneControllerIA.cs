using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AirplaneControllerIA : Agent
{

    public GameObject Décoller;
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

    [SerializeField]
    public int targetTime = 60;
    [SerializeField]
    public float timer = 0;
    [SerializeField]
    float lastEpisodeTime = 0;
    [SerializeField]
    float bestTime = 0;
    [SerializeField]
    float timeMean = 0;

    float addedTime = 0;

    [Range(-1, 1)]
    public float ThrustInput;
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

    [SerializeField]
    [Range(0, 1)]
    float thrustPercent;
    //float brakesTorque;

    AircraftPhysics aircraftPhysics;
    public List<Rigidbody> bodyParts;
    

    [SerializeField]
    public RayPerceptionSensorComponent3D horizontalSensor;
    [SerializeField]
    public RayPerceptionSensorComponent3D verticalSensor;

    [SerializeField]
    List<Collider> airplaneColliders;

    float minHeight = 10;
    float maxHeight = 60;

    
    [SerializeField]
    CheckCollisions colCheck;

    [SerializeField]
    TextMesh rewardText;

    float addedReward = 0;
    int rewardCount = 0;
    [SerializeField]
    float totalReward = 0;


    void CalculateTimeMean()
    {
        addedTime += timer;
        timeMean = addedTime / CompletedEpisodes;
    }

    //Normalize Reward
    void _AddReward(float amount)
    {
        rewardCount++;
        addedReward += amount;
        totalReward = addedReward / rewardCount;
    }

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        horizontalSensor = GameObject.Find("horizontal rays").GetComponent<RayPerceptionSensorComponent3D>();
        verticalSensor = GameObject.Find("vertical rays").GetComponent<RayPerceptionSensorComponent3D>();
    }

    public override void OnEpisodeBegin()
    {
        //print("restarting...");
        colCheck.Restart();

        addedReward = 0;
        rewardCount = 0;
        totalReward = 0;

        transform.SetPositionAndRotation(Décoller.transform.position + new Vector3(0, 5, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        thrustPercent = 0;

        if (timer > bestTime) bestTime = timer;
        lastEpisodeTime = timer;

        CalculateTimeMean();
        timer = 0;
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
        //print("aaaaaaaa");
        Pitch = actions.ContinuousActions[0];
        Yaw = actions.ContinuousActions[1];
        //Roll = actions.ContinuousActions[2];
        ThrustInput = actions.ContinuousActions[2] * 0.01f;

        

       

        //rewards
        {

            //rewards: above certiain time without fliyng / at more ore less at certain Y / not colliding with anything;
            //reward = 0;

            bool rewardHasBeenSet = false;

            if (transform.position.y > minHeight && transform.position.y < maxHeight)
            {
                _AddReward(0.2f);
            }
            else
            {
                _AddReward(-0.4f);

            }



            if (timer >= targetTime)
            {
                if (transform.position.y > minHeight && transform.position.y < maxHeight)
                {
                    _AddReward(1);
                    SetReward(totalReward);
                    rewardHasBeenSet = true;
                    EndEpisode();
                }
                else
                {
                    _AddReward(-1);
                    SetReward(totalReward);
                    rewardHasBeenSet = true;
                    EndEpisode();
                }
            }
            else
            {

                //the total reward obtained if the target time in the air is reached
                float rewardByTime = 0.4f;

                //reward obtained for a fragment of the target time in the air
                float rewardByTimeSegment = 0;

                //intervals at which the rewardByTime increases until reaches the target time
                float timeDivisions = 10;

                if (timer > 5 && transform.position.y < minHeight)
                {
                    _AddReward(-1);
                    SetReward(totalReward);
                    rewardHasBeenSet = true;
                    EndEpisode();
                }

                for (int i = 1; i < timeDivisions; i++)
                {
                    if (timer > targetTime / timeDivisions * i && timer < targetTime / timeDivisions * i + 1) rewardByTimeSegment = rewardByTime / timeDivisions * i;

                }

                if (transform.position.y > minHeight && transform.position.y < maxHeight)
                {
                    _AddReward(rewardByTimeSegment);
                }
                else
                {
                   _AddReward(rewardByTimeSegment * 1.2f);
                }
            }

            if (colCheck.isColliding)
            {

                _AddReward(-0.45f);

                SetReward(totalReward);
                rewardHasBeenSet = true;
                EndEpisode();
            }
            else
            {
                _AddReward(0.15f);
                //print("Collision good " + reward);
            }

            //if (reward < -1)
            //{
            //    reward = -1;
            //}
            //else if(reward > 1)
            //{
            //    reward = 1;
            //}

            //print((float)reward);


            if (!rewardHasBeenSet)
            {
                SetReward(totalReward);
            }
            
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

        Pitch = (float)continuousActionOut[0] * 0.05f;
        Yaw = (float)continuousActionOut[1] * 0.05f;

        //SetControlSurfecesAngles(continuousActionOut[0], Roll, continuousActionOut[1], Flap);
        thrustPercent += (float)continuousActionOut[2] * 0.01f;

        //base.Heuristic(actionsOut);)
    }

    private void Update()
    {
        rewardText.text = "reward: " + totalReward.ToString();
        timer += Time.deltaTime;
        //print("thrust input: " + thrustPercent);
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
        thrustPercent += ThrustInput;

        if (thrustPercent > 1)
        {
            thrustPercent = 1;
        }
        if (thrustPercent < 0)
        {
            thrustPercent = 0;
        }


        float multiplier = 4;
        SetControlSurfecesAngles(Pitch * multiplier, Roll * multiplier, Yaw * multiplier, Flap);
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
