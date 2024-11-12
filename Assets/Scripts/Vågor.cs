using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterWaves : MonoBehaviour
{
    public float amplitude = 0.3f;  // Amplituden för vågorna
    public float frequency = 5f;  // Frekvensen för vågorna
    public float wavelength = 6f; // Våglängden mellan vågtopparna

    private Mesh mesh;
    private Vector3[] originalVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
    }

    void Update()
    {
        Vector3[] vertices = new Vector3[originalVertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            // Använd harmonisk svängning för y-koordinaten
            vertex.y = amplitude * Mathf.Sin((vertex.x + vertex.z) * 2 * Mathf.PI / wavelength + Time.time * frequency);
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Uppdaterar normaler för att anpassa ljus och skuggor
    }
}
