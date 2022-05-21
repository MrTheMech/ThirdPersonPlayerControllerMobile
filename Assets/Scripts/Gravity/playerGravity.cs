using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGravity : MonoBehaviour
{
    //references
    CharacterController characterController;
    playerMovement playerMovement;

    [Header("GroundCheck")]
    public bool isGrounded = true;
    [SerializeField] [Range(0.9f, 1.8f)] float g_groundCheckRadiusMultiplier;
    [SerializeField] [Range(-0.95f, 1.5f)] float g_groundCheckDistanceMultiplier;
    RaycastHit HitInfo;

    [Header("Jump")]
    public float jumpSpeed;
    public bool CanJumping = true;


    public FixedButton fixedButton;


    //temp data ignore
    public float jumpCooldown;


    [Header("Gravity")]
    public float gravity = -9.81f;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<playerMovement>();
    }

    private void Update()
    {

        



        if (fixedButton.Pressed)
        {
            if(Time.time > jumpCooldown)
            {
                jumpCooldown = Time.time + 1f;
            }
            gravity = jumpSpeed;
        }


        if (isGrounded)
        {
            playerMovement.m_IsGrounded = true;
        }
        else
        {
            playerMovement.m_IsGrounded = false;
        }


            
    }



    private void FixedUpdate()
    {
        isGrounded = isGroundedCheck();
    }

    public float tempSphereRadius;
    public Vector3 tempCharacterCentre;
    private bool isGroundedCheck()
    {
        
        float SpherecastRaduis = characterController.radius * g_groundCheckRadiusMultiplier;
        float SpherecastDistance = characterController.bounds.size.y - SpherecastRaduis * g_groundCheckDistanceMultiplier;
        tempCharacterCentre = characterController.center;
        tempSphereRadius = SpherecastRaduis;
        return Physics.SphereCast(characterController.bounds.center, SpherecastRaduis, Vector3.down, out HitInfo, SpherecastDistance);

    }
}
