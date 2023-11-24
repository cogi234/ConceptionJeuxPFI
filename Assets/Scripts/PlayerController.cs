using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionAsset inputAsset;
    [Header("Movement")]
    [SerializeField] float groundAcceleration;
    [SerializeField] float airAcceleration;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float initialJumpForce;
    [SerializeField] float continuousJumpForce;
    [Header("Camera")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [Header("Grounded")]
    [SerializeField] Vector3 groundedRaycastOrigin;
    [SerializeField] float groundedRaycastLength;
    [SerializeField] LayerMask groundedRaycastLayers;

    Rigidbody rb;
    Vector3 moveDirection;
    bool sprinting = false;
    bool jumping = false;
    float cameraAngle = 0;

    public bool Grounded {  get; private set; }

    private void Awake()
    {
        //Setup rigidbody
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Input setup
        InputActionMap playerInput = inputAsset.FindActionMap("Player");
        playerInput.FindAction("Move").performed += MoveCall;
        playerInput.FindAction("Move").canceled += MoveCall;
        playerInput.FindAction("Look").performed += LookCall;
        playerInput.FindAction("Attack");
        playerInput.FindAction("Jump").performed += (InputAction.CallbackContext action) => jumping = true;
        playerInput.FindAction("Jump").canceled += (InputAction.CallbackContext action) => jumping = false;
        playerInput.FindAction("Sprint").performed += (InputAction.CallbackContext action) => sprinting = true;
    }

    private void LookCall(InputAction.CallbackContext action)
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * mouseSensitivity * action.ReadValue<Vector2>().x));
        //We put a limit, so we can't look at the world upside down
        cameraAngle = Mathf.Clamp(cameraAngle - (Time.deltaTime * mouseSensitivity * action.ReadValue<Vector2>().y), -90, 90);
        cameraTransform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);
    }
    private void MoveCall(InputAction.CallbackContext action)
    {
        Vector2 temp = action.ReadValue<Vector2>();
        moveDirection = new Vector3(temp.x, 0, temp.y);
        if (moveDirection.z <= 0)
            sprinting = false;
    }

    private void FixedUpdate()
    {
        //Detect groundedness
        bool wasGrounded = Grounded;
        Grounded = Physics.SphereCast(new Ray(transform.position + groundedRaycastOrigin, Vector3.down), 0.475f, groundedRaycastLength, groundedRaycastLayers);
        if (Grounded && !wasGrounded)
            jumping = false;

        //Jump
        if (jumping)
        {
            if (Grounded)
                rb.AddForce(Vector3.up * initialJumpForce, ForceMode.Impulse);
            else
                rb.AddForce(Vector3.up * continuousJumpForce, ForceMode.Force);
        }

        //Move
        if (Grounded)
            rb.AddForce((transform.localToWorldMatrix * moveDirection.normalized) * groundAcceleration * (sprinting ? sprintMultiplier : 1), ForceMode.Acceleration);
        else
            rb.AddForce((transform.localToWorldMatrix * moveDirection.normalized) * airAcceleration * (sprinting ? sprintMultiplier : 1), ForceMode.Acceleration);

        Debug.Log(rb.velocity.magnitude);
    }
}
