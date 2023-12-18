using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeController : MonoBehaviour
{
    public enum CubeAction { Nothing, GoToTarget, Transition }

    public Transform target;
    public Color Color { get => material.color; set => material.color = value; }
    public CubeAction currentAction = CubeAction.GoToTarget;

    [Header("Springs")]
    [SerializeField] float positionStiffness;
    [SerializeField] float positionDamper;
    [SerializeField] float rotationStiffness, rotationDamper;
    [Header("Transition")]
    public float transitionTime;
    
    Rigidbody rb;
    Material material;
    float transitionTimer;
    Vector3 originalPosition;
    Quaternion originalRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        material = new Material(GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = material;
    }

    private void FixedUpdate()
    {
        switch (currentAction)
        {
            case CubeAction.Nothing:
                if (rb.isKinematic)
                    rb.isKinematic = false;
                break;
            case CubeAction.GoToTarget:
                FixedGoToTarget();
                break;
            case CubeAction.Transition:
                break;
        }
    }
    private void Update()
    {
        switch (currentAction)
        {
            case CubeAction.Nothing:
                break;
            case CubeAction.GoToTarget:
                break;
            case CubeAction.Transition:
                Transition();
                break;
        }
    }

    void FixedGoToTarget()
    {
        if (target == null)
            return;

        if (rb.isKinematic)
            rb.isKinematic = false;

        //Position
        //Spring
        Vector3 positionDifference = target.position - transform.position;
        Vector3 positionSpringForce = positionDifference * positionStiffness;
        rb.AddForce(positionSpringForce, ForceMode.Acceleration);
        //Damper
        Vector3 positionDamperForce = -rb.velocity * positionDamper;
        rb.AddForce(positionDamperForce, ForceMode.Acceleration);

        //Rotation
        //Spring
        Quaternion rotationChange = target.rotation * Quaternion.Inverse(rb.rotation);
        rotationChange.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180)
            angle -= 360;

        if (!Mathf.Approximately(angle, 0))
        {
            angle *= Mathf.Deg2Rad;
            Vector3 rotationDiff = axis * angle;
            Vector3 rotationSpringForce = rotationDiff * rotationStiffness;
            rb.AddTorque(rotationSpringForce, ForceMode.Acceleration);
        }

        //Damper
        Vector3 rotationDamperForce = -rb.angularVelocity * rotationDamper;
        rb.AddTorque(rotationDamperForce, ForceMode.Acceleration);
    }

    void Transition()
    {
        if (target == null)
            return;

        if (!rb.isKinematic)
        {
            GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
            transitionTimer = 0;
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        transform.position = Vector3.Lerp(originalPosition, target.position, transitionTimer / transitionTime);
        transform.rotation = Quaternion.Lerp(originalRotation, target.rotation, transitionTimer / transitionTime);

        transitionTimer += Time.deltaTime;
        if (transitionTimer >= transitionTime)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
            currentAction = CubeAction.GoToTarget;
            GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
        }
    }
}
