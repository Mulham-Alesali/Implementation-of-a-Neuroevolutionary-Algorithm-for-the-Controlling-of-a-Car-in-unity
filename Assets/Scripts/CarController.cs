using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class controls the physical forces on the car
/// 
/// </summary>
public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public float HorizontalInput;
    public float VerticalInput;
    private float currentSteerAngle;
    private float Currentbreakforce { get; set; } = 1000;

    [SerializeField] private bool isBreaking;
    [SerializeField] private readonly float motorForce = 1000;
    [SerializeField] private float breakForce;
    [SerializeField] private readonly float  maxSteeringAngle = 30;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider
    , Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * HorizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    public float GetMotorForce()
    {
        return frontLeftWheelCollider.motorTorque;
    }

    private void HandleMotor()
    {
        if (!gameObject.GetComponent<Brain>().Surviving) return;

        frontLeftWheelCollider.motorTorque = VerticalInput * motorForce;
        frontRightWheelCollider.motorTorque = VerticalInput * motorForce;
        breakForce = isBreaking ? breakForce : 0f;

        if (isBreaking)
        {
            ApplyBreaking();
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0;
            frontLeftWheelCollider.brakeTorque = 0;
            rearLeftWheelCollider.brakeTorque = 0;
            rearRightWheelCollider.brakeTorque = 0;
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = Currentbreakforce;
        frontLeftWheelCollider.brakeTorque = Currentbreakforce;
        rearLeftWheelCollider.brakeTorque = Currentbreakforce;
        rearRightWheelCollider.brakeTorque = Currentbreakforce;
    }

    /// <summary>
    /// control the movement of the car
    /// </summary>
    /// <param name="input">input[0] control the steering
    /// , input[1] the force applied on the motor</param>
    public void Control(float[] input)
    {
        HorizontalInput = input[0];
        VerticalInput = input[1] > 0 ? input[1] : 0 ;
    }

    /// <summary>
    /// Cancel all the forces applied to the vehicle
    /// </summary>
    public void RemoveAllForces()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }


}
