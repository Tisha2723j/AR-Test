using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using System.Text;

[RequireComponent(typeof(ARFace))]
public class SaveFaceMeshData : MonoBehaviour
{
    private ARFace face;
    private string filePath;

    void Start()
    {
        face = GetComponent<ARFace>();
        face.updated += OnFaceUpdated;

        // Set file path to Unity's persistent data directory
        filePath = Path.Combine(Application.persistentDataPath, "FaceMeshData.txt");
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        Mesh faceMesh = GetComponent<MeshFilter>().mesh; // Get the face mesh

        if (faceMesh == null) return;

        Vector3[] vertices = faceMesh.vertices;
        Vector2[] uvs = faceMesh.uv;
        int[] indices = faceMesh.triangles;

        // Using StringBuilder for efficient writing
        StringBuilder sb = new StringBuilder();

        // Save Vertices
        sb.AppendLine("Vertices:");
        foreach (Vector3 v in vertices)
            sb.AppendLine($"{v.x},{v.y},{v.z}");

        // Save UVs
        sb.AppendLine("UVs:");
        foreach (Vector2 uv in uvs)
            sb.AppendLine($"{uv.x},{uv.y}");

        // Save Indices
        sb.AppendLine("Indices:");
        foreach (int index in indices)
            sb.AppendLine(index.ToString());

        // Write to file
        File.WriteAllText(filePath, sb.ToString());
        Debug.Log($"Face mesh data saved to: {filePath}");
    }

    void OnDestroy()
    {
        face.updated -= OnFaceUpdated;
    }
}