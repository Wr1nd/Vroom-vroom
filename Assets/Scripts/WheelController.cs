using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    LogitechGSDK.LogiControllerPropertiesData properties;

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider rearRight;
    [SerializeField] WheelCollider rearLeft;

    [SerializeField] Transform frontWheels;
    [SerializeField] Transform rearWheels;

    public float acceleration = 500f;
    public float beakingForce = 300f;
    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;



    private void FixedUpdate()
    {

        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        //update acceleration with logitechs wheel throttle pedal
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            int throttle;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            throttle = rec.lY;
            currentAcceleration = acceleration * throttle / -32767;


        }

        if(Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = beakingForce;
        }
        else
            currentBreakForce = 0f;

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        rearRight.brakeTorque = currentBreakForce;
        rearLeft.brakeTorque = currentBreakForce;

        frontRight.steerAngle = currentTurnAngle;
        frontLeft.steerAngle = currentTurnAngle;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

    }

    void UpdateWheel(WheelCollider wheel, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheel.GetWorldPose(out pos, out rot);

        wheelTransform.rotation = rot;
        wheelTransform.position = pos;


    }

}
