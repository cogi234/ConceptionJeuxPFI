using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CoreMovement : MonoBehaviour
{
    public enum CoreAction { Nothing, GoToTarget, Transition }

    [Header("Transition/Physics stuff")]
    public Transform target;
    public CoreAction currentAction = CoreAction.Nothing;
    [Header("Springs")]
    [SerializeField] float positionStiffness;
    [SerializeField] float positionDamper;
    [SerializeField] float rotationStiffness, rotationDamper;
    [Header("Transition")]
    public float transitionTime;

    Rigidbody rb;
    float transitionTimer;
    Vector3 originalPosition;
    Quaternion originalRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (currentAction)
        {
            case CoreAction.Nothing:
                if (rb.isKinematic)
                    rb.isKinematic = false;
                break;
            case CoreAction.GoToTarget:
                FixedGoToTarget();
                break;
            case CoreAction.Transition:
                break;
        }
    }
    private void Update()
    {
        switch (currentAction)
        {
            case CoreAction.Nothing:
                break;
            case CoreAction.GoToTarget:
                break;
            case CoreAction.Transition:
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
        rb.AddForce(positionSpringForce, ForceMode.Force);
        //Damper
        Vector3 positionDamperForce = -rb.velocity * positionDamper;
        rb.AddForce(positionDamperForce, ForceMode.Force);

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
        rb.AddTorque(rotationDamperForce, ForceMode.Force);
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
            currentAction = CoreAction.GoToTarget;
            GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
        }
    }

}
