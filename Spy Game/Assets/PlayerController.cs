using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cameraReference;
    [SerializeField] Animator animator;
    // [SerializeField] private float speed = 10f;        // How fast the player can move
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float speedSmoothVelocity = 0f;
    [SerializeField] private float speedSmoothTime = 0.1f;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float gravity = 3f;
    private bool isCrouching = false;

    [SerializeField] private Transform mainCameraTransform;

    [SerializeField] private CharacterController characterController;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mainCameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        // TODO: everything (!)
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // animator.SetFloat("horizontalInput", horizontalInput);
        HandleCrouching(); 
        MoveCharacter();
    }

    private void HandleCrouching()
    {
         if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching; 
            animator.SetBool("IsCrouching",isCrouching);
        }
    }

    private void MoveCharacter()
    {
        Vector2 movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ); // get Axis? 
        Vector3 forwardVector = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forwardVector.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (forwardVector * movementInput.y + right * movementInput.x).normalized;
        Vector3 gravityVector = Vector3.zero;

        // applies gravity
        if (!characterController.isGrounded)
        {
            gravityVector.y -= gravity;
        }

        if (desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
        }

        float targetSpeed = walkSpeed * movementInput.magnitude;

        // check for running
        if (Input.GetButton("Sprint") && !isCrouching)
        {
            targetSpeed = runSpeed * movementInput.magnitude;
            animator.SetFloat("MovementSpeed", 1f * movementInput.magnitude, speedSmoothTime, Time.deltaTime);
        }
        else
        {

            animator.SetFloat("MovementSpeed", 0.5f * movementInput.magnitude, speedSmoothTime, Time.deltaTime);
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        characterController.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
        characterController.Move(gravityVector * Time.deltaTime);
    }

    void FixedUpdate()
    {
        /*     
             // right is shorthand for (1,0,0) or the x value            
             // forward is short for (0,0,1) or the z value 
             Vector3 direction = (cameraReference.right * Input.GetAxis("Horizontal")) + (cameraReference.forward * Input.GetAxis("Vertical"));

             direction.y = 0;//Keeps character upright against slight fluctuations

             if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
             {
                 //rotate from this /........to this............../.........at this speed 
                 rigidBody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                 rigidBody.velocity = transform.forward * speed;

             }
     */
    }
}






// using UnityEngine;
// using System.Collections;

// public class PlayerController : MonoBehaviour {

//     public float playerSpeed;
//     public float sprintSpeed = 4f;
//     public float walkSpeed = 2f;
//     public float mouseSensitivity = 2f;
//     public float jumpHeight = 3f;
//     private bool isMoving = false;
//     private bool isSprinting =false;
//     private float yRot;

//     private Animator anim;
//     private Rigidbody rigidBody;

//     // Use this for initialization
//     void Start () {

//         playerSpeed = walkSpeed;
//         anim = GetComponent<Animator>();
//         rigidBody = GetComponent<Rigidbody>();

//     }

//     // Update is called once per frame
//     void Update () {

//         yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
//         transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);

//         isMoving = false;

//         if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
//         {
//             //transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * playerSpeed);
//             rigidBody.velocity += transform.right * Input.GetAxisRaw("Horizontal") * playerSpeed;
//             isMoving = true;
//         }
//         if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
//         {
//             //transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * playerSpeed);
//             rigidBody.velocity += transform.forward * Input.GetAxisRaw("Vertical") * playerSpeed;
//             isMoving = true;
//         }

//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             transform.Translate(Vector3.up * jumpHeight);
//         }

//         if (Input.GetAxisRaw("Sprint") > 0f)
//         {
//             playerSpeed = sprintSpeed;
//             isSprinting = true;
//         }else if (Input.GetAxisRaw("Sprint") < 1f)
//         {
//             playerSpeed = walkSpeed;
//             isSprinting = false;
//         }

//         anim.SetBool("isMoving", isMoving);
//         anim.SetBool("isSprinting", isSprinting);

//     }
// }

