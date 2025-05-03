using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARAnchorManager), typeof(ARRaycastManager))]
public class ARAnchorHandler : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;

    [SerializeField] private GameObject anchorPrefab; // Opaque anchor
    //[SerializeField] private GameObject AlphaPrefab; // Transparent anchor

    private GameObject anchor;
    private GameObject alpha;

    private Vector2 screenPoint;
    private List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    
    [SerializeField] private TMPro.TMP_Text tMP_Text;

    private NewControls newControls; // Input system reference

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();

        newControls = new NewControls();
    }

    // private void Start()
    // {
    //     // Ensure tempAnchor exists in the scene
    //     if (alpha == null && AlphaPrefab != null)
    //     {
    //         alpha = Instantiate(AlphaPrefab);
    //         alpha.SetActive(false); // Initially hidden
    //     }
    // }

    private void OnEnable()
    {
        newControls.Input.Touch.performed += ctx => PlaceAnchor();
        newControls.Enable();
    }

    private void OnDisable()
    {
        newControls.Input.Touch.performed -= ctx => PlaceAnchor();
        newControls.Disable();
    }

    private void Update()
    {
        tMP_Text.text = "";

        screenPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        // Raycast for surface detection
        if (raycastManager.Raycast(screenPoint, m_Hits, TrackableType.Planes))
        {
            if (m_Hits.Count > 0)
            {
                tMP_Text.text = "Tracking Surface";

                // Update tempAnchor position
                alpha.transform.position = m_Hits[0].pose.position;
                alpha.transform.rotation = m_Hits[0].pose.rotation;

                if (!alpha.activeSelf)
                    alpha.SetActive(true);
            }
        }
        else
        {
            tMP_Text.text = "No Surface Detected";
        }
    }

    private void PlaceAnchor()
    {
        if (m_Hits.Count > 0)
        {
            if (!anchor)
            {
                anchor = Instantiate(anchorPrefab, alpha.transform.position, alpha.transform.rotation);
                anchor.AddComponent<ARAnchor>();
            }
            else
            {
                anchor.transform.position = alpha.transform.position;
            }
        }
    }
}
