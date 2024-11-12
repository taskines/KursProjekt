using UnityEngine;

public class CranePendulum3D : MonoBehaviour
{
    public GameObject ball; // Reference to the wrecking ball
    public Transform ropeAttachment; // Point on the crane where the rope is attached
    public float moveSpeed = 5f; // Speed at which the crane moves

    private Rigidbody ballRigidbody;
    private Rigidbody craneRigidbody;
    private HingeJoint hingeJoint;

    void Start()
    {
        // Initialize the Rigidbody components
        ballRigidbody = ball.GetComponent<Rigidbody>();
        craneRigidbody = GetComponent<Rigidbody>();

        if (craneRigidbody == null)
        {
            craneRigidbody = gameObject.AddComponent<Rigidbody>();
            craneRigidbody.isKinematic = true; // Crane should only move based on player input
        }

        // Add a HingeJoint to the ball to simulate swinging
        hingeJoint = ball.AddComponent<HingeJoint>();
        hingeJoint.connectedBody = craneRigidbody;

        // Set anchor point for the hinge on the ball
        hingeJoint.anchor = Vector3.zero; // Ball's local position

        // Set the connected anchor to the point on the crane where the rope is attached
        hingeJoint.connectedAnchor = ropeAttachment.localPosition;

        // Set hinge joint properties to simulate swinging
        hingeJoint.axis = Vector3.forward; // Allows rotation in the Z-axis (swinging motion)
        hingeJoint.useLimits = false; // Disable limits for free swinging
    }

    void Update()
    {
        // Move the crane left and right using arrow keys
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 craneMovement = new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);
        transform.Translate(craneMovement);
    }
}
