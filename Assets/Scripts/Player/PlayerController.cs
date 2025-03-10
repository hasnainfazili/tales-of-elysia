using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 movementVector;
    private Vector3 cameraDirection;
    private bool isWeaponEquipped = false;
    public bool Blocking {get; private set;} = false;
    private float xInput, yInput;
    public float speed;
    public bool isAnxious = false;
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        EventsManager.instance.gameEvents.onAnxietyTriggered += Anxious;
        EventsManager.instance.miscEvents.onQuestLogToggled += DisableInput;
        EventsManager.instance.miscEvents.onCampfireActivated += DisableInput;
    }

    private void OnDisable()
    {
        EventsManager.instance.gameEvents.onAnxietyTriggered -= Anxious;
        EventsManager.instance.miscEvents.onQuestLogToggled -= DisableInput;
        EventsManager.instance.miscEvents.onCampfireActivated -= DisableInput;


    }
    private void DisableInput()
    {
        if(characterController.enabled)
        {
            characterController.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;

        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            characterController.enabled = true;
        }
    }
    void Update()
    {
        if(GameManager.instance.ArenaModePanel != null)
        {
            if( GameManager.instance.ArenaModePanel.activeInHierarchy)
            {
                return;
            }
        }
        if(BreathingQTE.instance.isActive || DialogueManager.GetInstance().dialogueIsPlaying || GameManager.instance.Paused)
        {
            return;
        }
        if(characterController.enabled)
        {
        if (Input.GetButtonDown("Fire1"))
        {
            isWeaponEquipped = true;
            EventsManager.instance.playerEvents.AttackPressed();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            EventsManager.instance.playerEvents.BlockPress();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isWeaponEquipped)
            {
                isWeaponEquipped = false;
                EventsManager.instance.playerEvents.WeaponSheathed();
            }
            else
            {
                isWeaponEquipped = true;
                EventsManager.instance.playerEvents.WeaponDrawn();
            }
        }

        // Calculate movement vector
        movementVector = new Vector3(xInput, 0, yInput) * moveSpeed;
        
        // Calculate speed for debugging
        speed = characterController.velocity.magnitude;
    }

    
    {
        Vector3 directionVector = new Vector3(xInput, 0, yInput).normalized * moveSpeed;
        if (directionVector != Vector3.zero)
        {
            HandleRotation();
        }
    }
        
    }
    private void Anxious()
    {
        moveSpeed = 14f;
        isAnxious = true;
    }

    void FixedUpdate()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical"); 

        if(characterController.enabled)
        {
            Movement();
            speed = characterController.velocity.magnitude;
        }
        Vector3 directionVector = new Vector3(xInput, 0, yInput).normalized * moveSpeed;
        if (directionVector != Vector3.zero)
        {
            HandleRotation();
        }
        
    }

    void Movement()
    {
        if (characterController.isGrounded)
        {
            // Convert movement vector to camera space
            cameraDirection = ConvertToCameraSpace(movementVector);
            characterController.Move(cameraDirection * Time.fixedDeltaTime);
        }
        else
        {
            // Apply gravity when not grounded
            cameraDirection.y += Physics.gravity.y * Time.fixedDeltaTime;
            characterController.Move(cameraDirection * Time.fixedDeltaTime);
        }
    }

 public Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZ = vectorToRotate.z * cameraForward;
        Vector3 cameraRightX = vectorToRotate.x * cameraRight;

        Vector3 rotatedCameraSpace = cameraForwardZ + cameraRightX;
        return rotatedCameraSpace;
    }
    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = cameraDirection.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = cameraDirection.z;

        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, .2f);
        
    }


}
