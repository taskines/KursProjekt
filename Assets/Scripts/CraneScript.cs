using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public LineRenderer arm;
    public GameObject pivot;

    public float damping = 0.99f; 
    public float gravity = -9.82f;

    public float angularVelocity = 0f;
    public float radius;
    public float angle = 0f;

    public Vector3 previousPivotPosition; 

    void Start()
    {
        arm = GetComponent<LineRenderer>();
        pivot = GameObject.Find("Pivot");

        // Beräkna repets längd baserat på avståndet mellan pendeln och pivotpunkten
        radius = Vector3.Distance(transform.position, pivot.transform.position);

        // Sätt startpositionen rakt under pivotpunkten
        transform.position = pivot.transform.position + new Vector3(0, -radius, 0);

        // Spara första position för pivotpunkten
        previousPivotPosition = pivot.transform.position;
    }

    void Update()
    {
        // Rita pendelarmen
        arm.SetPosition(0, pivot.transform.position);
        arm.SetPosition(1, transform.position);

        // Beräkna pivotens hastighet på grund av rörelse
        Vector3 pivotVelocity = (pivot.transform.position - previousPivotPosition) / Time.deltaTime;

        // Justera vinkelacceleration baserat på gravitation och pivotens rörelse
        float angularAcceleration = Mathf.Sin(angle) * gravity / radius;
        angularAcceleration -= pivotVelocity.x * Mathf.Cos(angle) / radius; // Påverkan av pivotens rörelse

        // Uppdatera vinkelhastigheten och tillämpa dämpning
        angularVelocity += angularAcceleration * Time.deltaTime;
        angularVelocity *= damping;

        // Uppdatera vinkeln
        angle += angularVelocity * Time.deltaTime;

        // Beräkna den nya positionen för pendeln
        float x = Mathf.Sin(angle) * radius + pivot.transform.position.x;
        float z = transform.position.z; // Behåll positionen på Z-axeln 
        float y = -Mathf.Cos(angle) * radius + pivot.transform.position.y;

        transform.position = new Vector3(x, y, z);

        // Uppdatera föregående pivotposition för nästa frame
        previousPivotPosition = pivot.transform.position;
    }

    // Funktion för att justera repets längd
    public void AdjustRopeLength(float adjustment)
    {
        radius = Mathf.Max(0.1f, radius + adjustment); // Se till att repet är positivt

        // Justera pendelns position för att matcha den nya rep-längden
        transform.position = pivot.transform.position + new Vector3(0, -radius, 0);
    }
}
