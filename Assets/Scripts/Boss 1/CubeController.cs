using UnityEngine;

public class CubeController : MonoBehaviour
{
    public enum CubeAction { Nothing, GoToTarget }

    public Transform target;
    public Color Color { get => material.color; set => material.color = value; }
    public CubeAction currentAction = CubeAction.GoToTarget;

    [Header("Springs")]
    [SerializeField] float positionStiffness;
    [SerializeField] float positionDamper;
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
            case CubeAction.GoToTarget:
                GoToTarget();
                break;
        }
    }
    void GoToTarget()
    {
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
        Vector3 rotationDifference = new Vector3();
        {
            //x
            float x1 = transform.rotation.eulerAngles.x % 360, x2 = target.rotation.eulerAngles.x % 360;
            if (Mathf.Abs(x2 - x1) < Mathf.Abs(x2 - x1 + 360) && Mathf.Abs(x2 - x1) < Mathf.Abs(x2 - x1 - 360))
                rotationDifference.x = x2 - x1;
            else if (Mathf.Abs(x2 - x1 + 360) < Mathf.Abs(x2 - x1 - 360))
                rotationDifference.x = x2 - x1 + 360;
            else
                rotationDifference.x = x2 - x1 - 360;
            //y
            float y1 = transform.rotation.eulerAngles.y % 360, y2 = target.rotation.eulerAngles.y % 360;
            if (Mathf.Abs(y2 - y1) < Mathf.Abs(y2 - y1 + 360) && Mathf.Abs(y2 - y1) < Mathf.Abs(y2 - y1 - 360))
                rotationDifference.y = y2 - y1;
            else if (Mathf.Abs(y2 - y1 + 360) < Mathf.Abs(y2 - y1 - 360))
                rotationDifference.y = y2 - y1 + 360;
            else
                rotationDifference.y = y2 - y1 - 360;
            //z
            float z1 = transform.rotation.eulerAngles.z % 360, z2 = target.rotation.eulerAngles.z % 360;
            if (Mathf.Abs(z2 - z1) < Mathf.Abs(z2 - z1 + 360) && Mathf.Abs(z2 - z1) < Mathf.Abs(z2 - z1 - 360))
                rotationDifference.z = z2 - z1;
            else if (Mathf.Abs(z2 - z1 + 360) < Mathf.Abs(z2 - z1 - 360))
                rotationDifference.z =  z2 - z1 + 360;
            else
                rotationDifference.z = z2 - z1 - 360;
        }
        Vector3 rotationSpringForce = rotationDifference * rotationStiffness;
        rb.AddTorque(rotationSpringForce, ForceMode.Force);
        //Damper
        Vector3 rotationDamperForce = -rb.angularVelocity * rotationDamper;
        rb.AddTorque(rotationDamperForce, ForceMode.Force);
    }
}
