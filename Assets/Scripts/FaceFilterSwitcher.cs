using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceFilterSwitcher : MonoBehaviour
{
    public Material[] faceMaterials; // Drag and drop materials in the Inspector
    private ARFaceManager arFaceManager;

    private int currentFilterIndex = 0;

    void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();

        if (arFaceManager == null)
        {
            Debug.LogError("ARFaceManager not found! Make sure it's attached to AR Session Origin.");
            return;
        }

        arFaceManager.facesChanged += OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            ApplyMaterial(face);
        }
    }

    public void ChangeFilter(int filterIndex)
    {
        if (faceMaterials.Length == 0 || filterIndex >= faceMaterials.Length) return;

        currentFilterIndex = filterIndex;

        foreach (var face in arFaceManager.trackables)
        {
            ApplyMaterial(face);
        }
    }

    private void ApplyMaterial(ARFace face)
    {
        if (face != null && face.GetComponent<MeshRenderer>() != null)
        {
            face.GetComponent<MeshRenderer>().material = faceMaterials[currentFilterIndex];
        }
    }
}