using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, Idatapersistant
{
    [SerializeField] InputActionAsset inputAsset;
    [Header("Movement")]
    [SerializeField] float groundAcceleration;
    [SerializeField] float airAcceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float groundStopDamping;
    [SerializeField] float groundStopTreshold;
    [SerializeField] float initialJumpForce;
    [SerializeField] float continuousJumpForce;
    [Header("Camera")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [Header("Grounded")]
    [SerializeField] Vector3 groundedRaycastOrigin;
    [SerializeField] float groundedRaycastLength;
    [SerializeField] LayerMask groundedRaycastLayers;
    [Header("Health")]
    [SerializeField] int maxHealth = 5;
    [SerializeField] Slider healthBar;
    float health;
    [Header("Stab")]
    [SerializeField] float stabRange = 1;

    Rigidbody rb;
    Vector3 moveDirection;
    bool sprinting = false;
    bool jumping = false;
    bool forceJump = false;
    float cameraAngle = 0;
    Animator animator;

    public bool Grounded { get; private set; }
    public bool ignoreGrounded = false;
    public bool immobile = false;

    private void Awake()
    {
        health = maxHealth;
        healthBar.value = health;
        GetComponent<DamageableComponent>().onDamage.AddListener(TakeDamage);

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Input setup
        InputActionMap playerInput = inputAsset.FindActionMap("Player");
        playerInput.FindAction("Move").performed += MoveCall;
        playerInput.FindAction("Move").canceled += MoveCall;
        playerInput.FindAction("Look").performed += LookCall;
        playerInput.FindAction("Attack").performed += (InputAction.CallbackContext action) => animator.SetTrigger("Swing");
        playerInput.FindAction("Jump").performed += (InputAction.CallbackContext action) => jumping = true;
        playerInput.FindAction("Jump").canceled += (InputAction.CallbackContext action) => jumping = false;
        playerInput.FindAction("Stab").performed += StabCall;
        playerInput.FindAction("Stab").canceled += StabStop;
        playerInput.FindAction("Sprint").performed += (InputAction.CallbackContext action) => sprinting = true;
    }

    private void OnDestroy()
    {
        InputActionMap playerInput = inputAsset.FindActionMap("Player");
        playerInput.FindAction("Move").performed -= MoveCall;
        playerInput.FindAction("Move").canceled -= MoveCall;
        playerInput.FindAction("Look").performed -= LookCall;
        playerInput.FindAction("Attack").performed -= (InputAction.CallbackContext action) => {
            if (immobile)
                return;
            animator.SetTrigger("Swing"); 
        };
        playerInput.FindAction("Jump").performed -= (InputAction.CallbackContext action) => {
            if (immobile)
                return;
            jumping = true; 
        };
        playerInput.FindAction("Jump").canceled -= (InputAction.CallbackContext action) => {
            if (immobile)
                return;
            jumping = false; 
        };
        playerInput.FindAction("Stab").performed -= StabCall;
        playerInput.FindAction("Stab").canceled -= StabStop;
        playerInput.FindAction("Sprint").performed -= (InputAction.CallbackContext action) => {
            if (immobile)
                return;
            sprinting = true;
        };
    }

    private void LookCall(InputAction.CallbackContext action)
    {
        if (immobile)
            return;
        transform.Rotate(new Vector3(0, Time.deltaTime * mouseSensitivity * action.ReadValue<Vector2>().x));
        //We put a limit, so we can't look at the world upside down
        cameraAngle = Mathf.Clamp(cameraAngle - (Time.deltaTime * mouseSensitivity * action.ReadValue<Vector2>().y), -90, 90);
        cameraTransform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);
    }
    private void MoveCall(InputAction.CallbackContext action)
    {
        if (immobile)
            return;
        Vector2 temp = action.ReadValue<Vector2>();
        moveDirection = new Vector3(temp.x, 0, temp.y);
        if (moveDirection.z <= 0)
            sprinting = false;
    }
    private void StabCall(InputAction.CallbackContext action)
    {
        if (immobile)
            return;
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, stabRange) && hit.rigidbody != null)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = hit.rigidbody;
            animator.SetBool("Stab", true);
        }
    }
    private void StabStop(InputAction.CallbackContext action)
    {
        if (immobile)
            return;
        Destroy(GetComponent<FixedJoint>());
        animator.SetBool("Stab", false);
    }

    private void FixedUpdate()
    {
        //Detect groundedness
        if (ignoreGrounded)
        {
            Grounded = false;
            ignoreGrounded = false;
        }
        else
        {
            bool wasGrounded = Grounded;
            Grounded = Physics.SphereCast(new Ray(transform.position + groundedRaycastOrigin, Vector3.down), 0.475f, groundedRaycastLength, groundedRaycastLayers);
            if (Grounded && !wasGrounded)
                jumping = false;
        }

        if (Grounded)
            rb.maxLinearVelocity = maxSpeed * (sprinting ? sprintMultiplier : 1);
        else
            rb.maxLinearVelocity = float.MaxValue;

        //Jump
        if (jumping)
        {
            if (Grounded || forceJump)
            {
                rb.AddForce(Vector3.up * initialJumpForce, ForceMode.Impulse);
                forceJump = false;
            }
            else if (GetComponent<FixedJoint>() != null)
            {
                Destroy(GetComponent<FixedJoint>());
                animator.SetBool("Stab", false);
                forceJump = true;
            }
            else
                rb.AddForce(Vector3.up * continuousJumpForce, ForceMode.Force);
        }

        //Move
        if (Grounded)
            rb.AddForce((transform.localToWorldMatrix * moveDirection.normalized) * groundAcceleration * (sprinting ? sprintMultiplier : 1), ForceMode.Acceleration);
        else
            rb.AddForce((transform.localToWorldMatrix * moveDirection.normalized) * airAcceleration * (sprinting ? sprintMultiplier : 1), ForceMode.Acceleration);

        if (Grounded && moveDirection.magnitude < groundStopTreshold)
        {
            Vector3 dampForce = -rb.velocity * groundStopDamping;
            dampForce.y = 0;
            rb.AddForce(dampForce);
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            health = Mathf.Max(health, 0);
            healthBar.value = health;

            if (health <= 0)
                StartCoroutine(OnDeath());
        }
    }

    IEnumerator OnDeath()
    {
        GameObject.Find("FadeToBlack").GetComponent<FadeToBlack>().enabled = true;

        yield return new WaitForSeconds(5);

        //Ici une fois mort, on restart du debut 
        GameObject.Find("SaveManager").GetComponent<Save>().NouvellePartie();
    }



    public void charger(SceneStat data)
    {
        health = data.VieJoueur;
        healthBar.value = health;
        gameObject.transform.position = data.PositionJoueur;
    }
    public void sauvegarde(ref SceneStat data)
    {
        data.VieJoueur = health;
        data.PositionJoueur = gameObject.transform.position;
    }
}