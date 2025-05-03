using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class PointCloudDataManager : MonoBehaviour
{
    public string fileName = "PointCloudData.txt"; // File name in StreamingAssets
    public float scale = 5f; // Scale factor for visualization
    public ParticleSystem pointCloudParticles; // Assign a Particle System in the Inspector

    private void Start()
    {
        LoadAndVisualizePointCloud();
    }

    void LoadAndVisualizePointCloud()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

        foreach (string line in lines)
        {
            string[] values = line.Trim().Split(',');
            if (values.Length == 3)
            {
                if (float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y) &&
                    float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
                {
                    Vector3 position = new Vector3(x, y, z) * scale;

                    ParticleSystem.Particle particle = new ParticleSystem.Particle
                    {
                        position = position,
                        startSize = 0.05f,  // ✅ Ensure visibility
                        startColor = Color.white, // ✅ Change color if needed
                        remainingLifetime = Mathf.Infinity // ✅ Prevents auto-destruction
                    };

                    particles.Add(particle);
                }
            }
        }

        if (particles.Count == 0)
        {
            Debug.LogError("No valid point cloud data found in the file!");
            return;
        }

        // Apply particles to the Particle System
        pointCloudParticles.SetParticles(particles.ToArray(), particles.Count);
        Debug.Log("Point cloud visualization completed with " + particles.Count + " points.");
    }
}
