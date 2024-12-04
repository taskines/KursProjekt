using UnityEngine;

public class CraneController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 60f;
    public float ropeAdjustmentSpeed = 0.5f;

    public Pendulum pendulumScript;

    [System.Obsolete]
    void Start()
    {
        
        GameObject pendulum = GameObject.FindObjectOfType<Pendulum>().gameObject;
        pendulumScript = pendulum.GetComponent<Pendulum>();
    }

    void Update()
    {
        // Cranecontroller
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // Rotation kontrollering
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Rep längd justering
        if (Input.GetKey(KeyCode.W))
        {
            pendulumScript.AdjustRopeLength(-ropeAdjustmentSpeed * Time.deltaTime); 
        }
        if (Input.GetKey(KeyCode.S))
        {
            pendulumScript.AdjustRopeLength(ropeAdjustmentSpeed * Time.deltaTime);
        }
    }
}