using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class ARRaycastDetection : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text tMP_Text;
    [SerializeField] private GameObject objectToInst;
    private GameObject objectInst;
    private ARRaycastManager m_RaycastManager;
    private List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    private InputAction touchAction;

    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();

        // Create a new InputAction for touch detection (Button type)
        touchAction = new InputAction("touch", binding: "<Touchscreen>/primaryTouch/press");
        touchAction.Enable();
    }

    private void OnEnable()
    {
        touchAction.Enable();
    }

    private void OnDisable()
    {
        touchAction.Disable();
    }

    void Update()
    {
        // Check if the touch action is triggered
        if (!touchAction.WasPressedThisFrame())
            return;

        // Get touch position manually
        if (Touchscreen.current == null || Touchscreen.current.primaryTouch.position == null)
            return;

        Vector2 screenPoint = Touchscreen.current.primaryTouch.position.ReadValue();
        tMP_Text.text = $"Touch Position: {screenPoint}";

        if (m_RaycastManager.Raycast(screenPoint, m_Hits, TrackableType.AllTypes))
        {
            tMP_Text.text = "Raycast Hit!";

            if (m_Hits.Count > 0)
            {
                if (objectInst == null)
                {
                    objectInst = Instantiate(objectToInst, m_Hits[0].pose.position, m_Hits[0].pose.rotation);
                }
                else
                {
                    objectInst.transform.position = m_Hits[0].pose.position;
                }
            }
        }
        else
        {
            tMP_Text.text = "No Hit Detected";
        }
    }
}
