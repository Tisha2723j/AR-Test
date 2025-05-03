using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using System.Text;

[RequireComponent(typeof(ARPointCloudManager))]
public class ARPointCloudHandler : MonoBehaviour
{
    private ARPointCloudManager aRPointCloud;
    private HashSet<Vector3> pointsCloud = new HashSet<Vector3>(); // Store only unique points
    private string filePath;

    public TMPro.TMP_Text tMP_Text;

    private void Awake()
    {
        aRPointCloud = GetComponent<ARPointCloudManager>();
        filePath = Path.Combine(Application.persistentDataPath, "PointCloudData.txt");
        Debug.Log($"Point cloud data file path: {filePath}");

        // Clear the file only when the app is first opened
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
    }

    void OnEnable()
    {
        if (aRPointCloud != null)
        {
            aRPointCloud.pointCloudsChanged += OnPointCloudsChanged;
        }
    }

    void OnDisable()
    {
        if (aRPointCloud != null)
        {
            aRPointCloud.pointCloudsChanged -= OnPointCloudsChanged;
        }
    }

    void Update()
    {
        tMP_Text.text = $"Point Cloud Count: {pointsCloud.Count}";
    }

    void OnPointCloudsChanged(ARPointCloudChangedEventArgs eventArgs)
    {
        StringBuilder sb = new StringBuilder();
        int newPoints = 0;

        foreach (var pointCloud in eventArgs.updated)
        {
            if (pointCloud.positions.HasValue)
            {
                foreach (Vector3 point in pointCloud.positions.Value)
                {
                    if (pointsCloud.Add(point)) // Only add new unique points
                    {
                        sb.AppendLine($"{point.x},{point.y},{point.z}");
                        newPoints++;
                    }
                }
            }
        }

        if (newPoints > 0)
        {
            File.AppendAllText(filePath, sb.ToString());
            Debug.Log($"Added {newPoints} new points to: {filePath}");
        }
    }
}
