﻿using System.Collections.Generic;
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
    

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();

    }

    public override void OnEpisodeBegin()
    {

        foreach(Rigidbody body in bodyParts)
        {
            body.angularVelocity = Vector3.zero;
            body.velocity = Vector3.zero;
            transform.position = new Vector3(0, 3.5f, 0);
            transform.rotation.SetEulerAngles(-90, 0, 0);
        }


        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        base.CollectObservations(sensor);
    }

    private void Update()
    {
        Pitch = Input.GetAxis("Vertical");
        Roll = Input.GetAxis("Horizontal");
        Yaw = Input.GetAxis("Yaw");


        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            thrustPercent += 0.01f;

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            thrustPercent -= 0.01f;

            
        }

        if (thrustPercent > 1)
        {
            thrustPercent = 1;
        }
        if (thrustPercent < 0)
        {
            thrustPercent = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    brakesTorque = brakesTorque > 0 ? 0 : 100f;
        //}

      // displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
       //displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
       //displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
       //displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";
    }

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
