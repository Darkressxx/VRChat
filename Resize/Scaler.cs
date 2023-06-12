using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class Scaler : UdonSharpBehaviour
{
    // Constants for scaling
    private const float SCALE_FACTOR = 2f;
    private const float INVERSE_SCALE_FACTOR = 0.5f;

    // References to various components
    private VRCPickup pickup;               // The VRCPickup component attached to this object
    private Transform objectTransform;      // The transform of this object
    private Collider pickupCollider;        // The collider attached to this object
    private Transform scalerTransform;      // The transform of the scaler object
    private VRC_Pickup scalerPickup;        // The VRC_Pickup component attached to the scaler object

    // Variables for scaling synchronization
    [UdonSynced] private Vector3 syncedScale;    // The synchronized scale value
    [UdonSynced] private bool scaleToggle = false;  // Toggle for scaling state
    private float lastScalerY = 0.0f;         // The last recorded y position of the scaler
    private float scaleSensitivity = 0.05f;   // Sensitivity for detecting scaler movement

    private void Start()
    {
        // Get references to components
        objectTransform = transform;

        scalerTransform = objectTransform.Find("Scaler");
        scalerPickup = scalerTransform.GetComponent<VRC_Pickup>();
        scalerTransform.gameObject.SetActive(false);

        pickup = GetComponent<VRCPickup>();
        pickupCollider = GetComponent<Collider>();

        // Initialize the synced scale on the master client
        if (Networking.IsMaster)
        {
            syncedScale = objectTransform.localScale;
        }
    }

    public override void OnDeserialization()
    {
        // Update the local scale when synchronized scale changes
        objectTransform.localScale = syncedScale;
    }

    public override void OnPickup()
    {
        // Activate the scaler object and disable the pickup collider when picked up
        scalerTransform.gameObject.SetActive(true);
        pickupCollider.enabled = false;
    }

    public override void OnDrop()
    {
        // Deactivate the scaler object and enable the pickup collider when dropped
        scalerTransform.gameObject.SetActive(false);
        pickupCollider.enabled = true;
        scalerPickup.Drop();
    }

    private void HalveScale()
    {
        // Halve the object's scale and update the synced scale value
        if (scaleToggle)
        {
            objectTransform.localScale *= INVERSE_SCALE_FACTOR;
            syncedScale = objectTransform.localScale;
            scaleToggle = false;
            RequestSerialization();
        }
    }

    private void DoubleScale()
    {
        // Double the object's scale and update the synced scale value
        if (!scaleToggle)
        {
            objectTransform.localScale *= SCALE_FACTOR;
            syncedScale = objectTransform.localScale;
            scaleToggle = true;
            RequestSerialization();
        }
    }

    private void Update()
    {
        if (pickup.IsHeld)
        {
            // Check for input to halve or double the scale
            if (Input.GetKeyDown(KeyCode.Q)) HalveScale();
            if (Input.GetKeyDown(KeyCode.E)) DoubleScale();

            if (scalerPickup.IsHeld)
            {
                // Detect scaler movement and update the scale accordingly
                var scalerY = scalerTransform.localPosition.y;
                float diff = Mathf.Abs(scalerY - lastScalerY);

                if (diff >= scaleSensitivity)
                {
                    if (scalerY < lastScalerY) HalveScale();
                    else if (scalerY > lastScalerY) DoubleScale();
                    lastScalerY = scalerY;
                }
            }
            else
            {
                // Reset scaler position and y value if it's not held
                scalerTransform.localPosition = Vector3.zero;
                lastScalerY = 0.0f;
            }
        }
    }
}
