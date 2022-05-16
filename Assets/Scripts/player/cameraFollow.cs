using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraFollow : MonoBehaviour
{
    #region AccessableVariables
    [Header("Camera Rotation")]
    [Tooltip("Sensitivity or Speed of Camera Rotation")]
    [SerializeField]
    private float RotationSensitivity = 8f;
    [Tooltip("Offset Value from Object to Camera")]
    [SerializeField]
    private float distanceFromCamera = 12f;
    [Tooltip("Camera Smoothness")]
    [SerializeField]
    private float cameraSmoothness = 0.9f;

    float RotX; //Reference Float - Player Input X
    float RotY; //Reference Float - Player Input Y
    Vector3 CurrentVelocity; //Reference Float for Camera Temp Velocity
    Vector3 targetRotation; //Reference Float for Camera Movement

    [SerializeField]
    bool isMobile; //Gets Mobile or PC from editor

    //Clamp Camera (Camera Bounds X axis)
    [Header("Clamp Camera")]

    [Tooltip("Minimum Clamp Camera (Camera Bounds X axis)")]
    [SerializeField]
    private float MinCameraRotationClamp = -60f;

    [Tooltip("Minimum Clamp Camera (Camera Bounds X axis)")]
    [SerializeField]
    private float MaxCameraRotationClamp = 60f;

    [Tooltip("The Target Where the Camera Should Rotate Around")]
    public Transform target;

    public FixedTouchField fixedTouchField;
    RaycastHit camHit;


    #endregion



    private void LateUpdate()
    {
        getPlayerInput();
        cameraClamp();
        cameraRotation();
    }

    private void Update()
    {
    }

    public void getPlayerInput()
    {
        if (isMobile)
        {
            float MobileSensitivity = RotationSensitivity / 40;
            RotX -= fixedTouchField.TouchDist.y * MobileSensitivity;
            RotY += fixedTouchField.TouchDist.x * MobileSensitivity;
        }
        else
        {
            RotX -= Input.GetAxis("Mouse Y") * RotationSensitivity;
            RotY += Input.GetAxis("Mouse X") * RotationSensitivity;
        }
    }
    public void cameraClamp()
    {
        RotX = Mathf.Clamp(RotX, MinCameraRotationClamp, MaxCameraRotationClamp);

    }

    public void cameraRotation()
    {
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(RotX, RotY), ref CurrentVelocity, cameraSmoothness);
        transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * distanceFromCamera;

    }



}
