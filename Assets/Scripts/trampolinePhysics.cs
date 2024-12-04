using UnityEngine;

public class TrampolinPhysics : MonoBehaviour
{
    // Variabler för mesh och dess komponenter
    private Mesh mesh;
    private Vector3[] vertices, originalVertices, velocities;

    // Fysiska parametrar för trampolinen
    public float springConstant = 2000f, damping = 3.0f, gravity = -9.81f, vertexMass = 1f;

    // Antal vertikaler i X- och Y-led
    private int numVerticesX, numVerticesY;

    void Start()
    {
        // Hämta mesh-komponenten och dess hörn
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        // Kopiera originalpositionerna för hörnen
        originalVertices = (Vector3[])vertices.Clone();

        // Initiera hastighetsvektorer för alla hörn
        velocities = new Vector3[vertices.Length];

        // Beräkna antalet vertikaler i X- och Y-led (förutsatt kvadratisk mesh)
        numVerticesX = Mathf.FloorToInt(Mathf.Sqrt(vertices.Length));
        numVerticesY = numVerticesX;
    }

    void Update()
    {
        // Applicera krafter och uppdatera meshen varje bildruta
        ApplyForces();
        UpdateMesh();
    }

    void ApplyForces()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // Hoppa över fasta punkter
            if (IsFixedPoint(i)) continue;

            Vector3 totalForce = Vector3.zero;
            Vector3[] neighbors = GetNeighbors(i);

            // Beräkna kraft från grannpunkter
            foreach (var neighbor in neighbors)
            {
                if (neighbor == Vector3.zero) continue;

                Vector3 direction = neighbor - vertices[i];
                float restLength = (originalVertices[System.Array.IndexOf(vertices, neighbor)] - originalVertices[i]).magnitude;
                float springForceMagnitude = springConstant * (direction.magnitude - restLength);

                // Lägg till fjäderkraften
                totalForce += direction.normalized * springForceMagnitude;
            }

            // Lägg till gravitationens påverkan
            totalForce += new Vector3(0, gravity * vertexMass, 0);

            // Uppdatera hastigheten baserat på total kraft
            velocities[i] += totalForce / vertexMass * Time.deltaTime;
        }
    }

    void UpdateMesh()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // Uppdatera positionen för de icke-fasta punkterna
            if (!IsFixedPoint(i))
            {
                vertices[i] += velocities[i] * Time.deltaTime;
            }
        }

        // Uppdatera meshen med nya positioner
        mesh.vertices = vertices;
        mesh.RecalculateNormals();  // Uppdatera normals för korrekt ljusberäkning
        mesh.RecalculateBounds();   // Uppdatera bounding box för meshen
    }

    Vector3[] GetNeighbors(int index)
    {
        Vector3[] neighbors = new Vector3[8];
        int x = index % numVerticesX;
        int y = index / numVerticesX;

        // Offset för att hitta grannpunkter
        int[] offsets = { -1, 1, -numVerticesX, numVerticesX, -numVerticesX - 1, numVerticesX + 1, -numVerticesX + 1, numVerticesX - 1 };

        for (int i = 0; i < offsets.Length; i++)
        {
            int neighborIndex = index + offsets[i];

            // Kontrollera om grannen är inom meshen och inte ligger utanför dess gränser
            if (neighborIndex >= 0 && neighborIndex < vertices.Length && Mathf.Abs(neighborIndex % numVerticesX - x) <= 1)
            {
                neighbors[i] = vertices[neighborIndex];
            }
            else
            {
                neighbors[i] = Vector3.zero; // Ingen giltig grannpunkt
            }
        }

        return neighbors;
    }

    bool IsFixedPoint(int index)
    {
        // Kontrollera om punkten är på kanten och därmed fast
        int x = index % numVerticesX;
        int y = index / numVerticesX;
        return x == 0 || x == numVerticesX - 1 || y == 0 || y == numVerticesY - 1;
    }
}
