using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;  // Import TMP namespace

namespace MyARProject
{
    [RequireComponent(typeof(ARFaceManager))]
    public class ARFaceTracking : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI faceStatusText;  // TMP Text for UI messages
        [SerializeField] private Material faceMaterial1;
        [SerializeField] private Material faceMaterial2;
        [SerializeField] private GameObject hatPrefab;
        [SerializeField] private GameObject earsPrefab;
        [SerializeField] private Button button1;
        [SerializeField] private Button button2;
        [SerializeField] private Button hatButton;
        [SerializeField] private Button earsButton;

        private ARFaceManager arFaceManager;
        private Material currentMaterial;
        private GameObject spawnedHat = null;
        private GameObject spawnedEars = null;

        private void Awake()
        {
            arFaceManager = GetComponent<ARFaceManager>();
        }

        private void OnEnable()
        {
            arFaceManager.facesChanged += OnFacesChanged;
            button1.onClick.AddListener(() => ChangeFaceMaterial(faceMaterial1));
            button2.onClick.AddListener(() => ChangeFaceMaterial(faceMaterial2));
            hatButton.onClick.AddListener(ToggleHat);
            earsButton.onClick.AddListener(ToggleEars);
        }

        private void OnDisable()
        {
            arFaceManager.facesChanged -= OnFacesChanged;
            button1.onClick.RemoveAllListeners();
            button2.onClick.RemoveAllListeners();
            hatButton.onClick.RemoveAllListeners();
            earsButton.onClick.RemoveAllListeners();
        }

        private void OnFacesChanged(ARFacesChangedEventArgs args)
        {
            foreach (var face in args.added)
            {
                ApplyMaterialToFace(face);
                UpdateStatusText("Face detected!");
            }
            foreach (var face in args.updated)
            {
                ApplyMaterialToFace(face);
                UpdateStatusText("Face updated at " + System.DateTime.Now.ToString("hh:mm:ss tt"));
            }
            foreach (var face in args.removed)
            {
                UpdateStatusText("Face lost!");
            }
        }

        private void ApplyMaterialToFace(ARFace face)
        {
            if (face.GetComponent<MeshRenderer>() != null && currentMaterial != null)
            {
                face.GetComponent<MeshRenderer>().material = currentMaterial;
            }
        }

        private void ChangeFaceMaterial(Material newMaterial)
        {
            currentMaterial = newMaterial;
            foreach (var face in arFaceManager.trackables)
            {
                ApplyMaterialToFace(face);
            }
        }

        private void ToggleHat()
        {
            if (arFaceManager.trackables.count == 0) return;

            ARFace firstFace = null;
            foreach (var face in arFaceManager.trackables)
            {
                firstFace = face;
                break;
            }

            if (firstFace == null) return;

            if (spawnedHat == null)
            {
                spawnedHat = Instantiate(hatPrefab, firstFace.transform);
                spawnedHat.transform.localPosition = new Vector3(0, 0.15f, 0);
                spawnedHat.transform.localRotation = Quaternion.identity;
                UpdateStatusText("Hat added!");
            }
            else
            {
                spawnedHat.SetActive(!spawnedHat.activeSelf);
                UpdateStatusText(spawnedHat.activeSelf ? "Hat shown!" : "Hat hidden!");
            }
        }

        private void ToggleEars()
        {
            if (arFaceManager.trackables.count == 0) return;

            ARFace firstFace = null;
            foreach (var face in arFaceManager.trackables)
            {
                firstFace = face;
                break;
            }

            if (firstFace == null) return;

            if (spawnedEars == null)
            {
                spawnedEars = Instantiate(earsPrefab, firstFace.transform);
                spawnedEars.transform.localPosition = new Vector3(0, 0.1f, 0);
                spawnedEars.transform.localRotation = Quaternion.identity;
                UpdateStatusText("Ears added!");
            }
            else
            {
                spawnedEars.SetActive(!spawnedEars.activeSelf);
                UpdateStatusText(spawnedEars.activeSelf ? "Ears shown!" : "Ears hidden!");
            }
        }

        private void UpdateStatusText(string message)
        {
            if (faceStatusText != null)
            {
                faceStatusText.text = message;
            }
        }
    }
}