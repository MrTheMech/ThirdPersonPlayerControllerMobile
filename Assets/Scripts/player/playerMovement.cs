using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerMovement : MonoBehaviour
{
    [Tooltip("Akka's Animation Controller")]
    public Animator akkaAnimator;

    [Tooltip("If The Scene is set to global check this boolean")] 
    public bool isGlobal;

    [Tooltip("Specify The Time Required to Smooth Player Rotations")]
    public float RotationSmoothingSpeed;

    [Tooltip("Specify The Speed Required for Player to Move")]
    public float PlayerMoveSpeed;

    //[Tooltip("Player's Rigidbody To apply Motion")]
    //public Rigidbody PlayerRigidbody;
    public CharacterController characterController;
    //Ignore these reference variable
    float currentVelocity;
    float currentSpeed;
    float speedVelocity;
    Transform cameraTransform;

    private float gravity = -9.81f;
    public float groundedGravity = -0.5f;

    public FloatingJoystick joystick;
    [SerializeField]
    bool isMobile; //Gets Mobile or PC from editor

    [HideInInspector]
    public Vector3 moveDir;


    [Header("Animation Related stuffs")]
    public Animator animator;

    //ignore these stuffs
    public float m_ForwardAmount;
    public float m_TurnAmount;
    public float m_JumpAmount;
    public float m_JumpLeg;
    public bool m_Crouching;
    public bool m_IsGrounded;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;

    }

    void Update()
    {


        Vector2 input;
        if (isMobile)
        {
            input = new Vector2(joystick.input.x, joystick.input.y);

        }
        else
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }


        Vector2 inputDir = input.normalized;

        //Debug.Log(inputDir);

        if (inputDir != Vector2.zero)
        {
            float Rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y ;

            //temp code for turn try


            transform.eulerAngles = Vector2.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, Rotation, ref currentVelocity, RotationSmoothingSpeed);

        }

        float targetSpeed = PlayerMoveSpeed * inputDir.magnitude;





        //.SetFloat("Forward",targetSpeed);


        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);



        moveDir = transform.forward * currentSpeed * Time.deltaTime;

        //gravity
        moveDir += Vector3.up * gravity * 2f * Time.deltaTime;
        if (!characterController.isGrounded)
        {
            
            gravity -= 9.81f * Time.deltaTime;
            m_JumpAmount = 2.5f;
        }
        else
        {
            gravity = 0;
        }

        //forward animation value
        m_ForwardAmount =  currentSpeed * 0.5f;

        characterController.Move(moveDir);
        animationUpdate();



        float RotationTurn = Mathf.Atan2(joystick.input.x, joystick.input.y + cameraTransform.eulerAngles.y);


        m_TurnAmount = ((RotationTurn - (-1)) / (1 - (-1)) * 100f / -10/100f ) * -1/1; 

        //Debug.Log(m_TurnAmount);

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        //PlayerRigidbody.velocity = transform.forward*currentSpeed;
    }


    public void animationUpdate()
    {
        animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Jump", m_JumpAmount, 0.1f,Time.deltaTime);
        animator.SetFloat("JumpLeg", m_JumpLeg);
        animator.SetBool("Crouch", m_Crouching);
        animator.SetBool("OnGround", m_IsGrounded);

        
    }





}
