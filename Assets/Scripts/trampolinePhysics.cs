using UnityEngine;

public class TrampolinPhysics : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] originalVertices;
    private Vector3[] velocities;

    public float springConstant = 2000f; 
    public float damping = 3.0f;       
    public float gravity = -9.81f;      
    public float vertexMass = 1f;      

    private int numVerticesX;
    private int numVerticesY;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        originalVertices = new Vector3[vertices.Length];
        vertices.CopyTo(originalVertices, 0);
        triangles = mesh.triangles;

        velocities = new Vector3[vertices.Length];

        
        numVerticesX = Mathf.FloorToInt(Mathf.Sqrt(vertices.Length));
        numVerticesY = numVerticesX;
    }

    void Update()
    {
        ApplyForces();
        UpdateMesh();
    }

    
    void ApplyForces()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            if (IsFixedPoint(i)) continue; 

            Vector3 totalForce = Vector3.zero;

           
            Vector3[] neighbors = GetNeighbors(i);

           
            foreach (var neighbor in neighbors)
            {
                if (neighbor != Vector3.zero)
                {
                    Vector3 direction = neighbor - vertices[i];
                    float restLength = (originalVertices[System.Array.IndexOf(vertices, neighbor)] - originalVertices[i]).magnitude;
                    float currentLength = direction.magnitude;

                    // Hookes lag: F = -k * (x - x0)
                    float springForceMagnitude = springConstant * (currentLength - restLength);
                    Vector3 force = direction.normalized * springForceMagnitude;

                    totalForce += force;
                }
            }

            // Applicera gravitation (F = m * g)
            totalForce += new Vector3(0, gravity * vertexMass, 0);

            // Dämpa rörelsen för att simulera mer realistisk fysik
            velocities[i] += totalForce / vertexMass * Time.deltaTime; // F = m * a => a = F / m
        }
    }

    // Uppdatera vertikalens position baserat på hastigheten
    void UpdateMesh()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            if (!IsFixedPoint(i))
            {
                // Använd hastigheten för att uppdatera positionen
                vertices[i] += velocities[i] * Time.deltaTime;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();  // Uppdatera ytnormals för ljussättning
        mesh.RecalculateBounds();   // Uppdatera bounding box för meshen
    }

    // Hämta de närmaste grannarna för varje vertex (över, under, vänster, höger samt diagonalt)
    Vector3[] GetNeighbors(int index)
    {
        Vector3[] neighbors = new Vector3[8];
        int x = index % numVerticesX;
        int y = index / numVerticesX;

        int[] offsets = new int[] { -1, 1, -numVerticesX, numVerticesX, -numVerticesX - 1, numVerticesX + 1, -numVerticesX + 1, numVerticesX - 1 };

        for (int i = 0; i < offsets.Length; i++)
        {
            int neighborIndex = index + offsets[i];

            // Kontrollera om grannen ligger inom meshen
            if (neighborIndex >= 0 && neighborIndex < vertices.Length)
            {
                // Se till att vi inte läser utanför meshens gränser
                if (Mathf.Abs(neighborIndex % numVerticesX - x) <= 1 && Mathf.Abs(neighborIndex / numVerticesX - y) <= 1)
                {
                    neighbors[i] = vertices[neighborIndex];
                }
                else
                {
                    neighbors[i] = Vector3.zero;  // Ingen grannpunkt
                }
            }
            else
            {
                neighbors[i] = Vector3.zero;  // Ingen grannpunkt
            }
        }

        return neighbors;
    }

    // Kontrollera om en punkt är fast
    bool IsFixedPoint(int index)
    {
        // Fastgör de yttre punkterna (t.ex. de första och sista i varje rad)
        int x = index % numVerticesX;
        int y = index / numVerticesX;

        // Om punkten är på kanten (yttersta raden eller kolumnen), så är den fast
        return (x == 0 || x == numVerticesX - 1 || y == 0 || y == numVerticesY - 1);
    }
}