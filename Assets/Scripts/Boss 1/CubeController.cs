using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public enum CubeAction { Nothing, StayInPosition }

    public Transform target;
    public Color Color { get => material.color; set => material.color = value; }
    public CubeAction currentAction = CubeAction.StayInPosition;

    [Header("Springs")]
    [SerializeField] float positionStiffness, positionDamper;
    [SerializeField] float rotationStiffness, rotationDamper;

    Rigidbody rb;
    Material material;

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
                break;
            case CubeAction.StayInPosition:
                StayInPosition();
                break;
        }
    }
    void StayInPosition()
    {
        //Position
        Vector3 diff = target.position - transform.position;

    }
}
