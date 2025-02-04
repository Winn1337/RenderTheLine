using UnityEngine;

public class DynamicZoom : MonoBehaviour
{
    [Header("Dynamic Zoom Settings")]
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float maxSpeed = 10f;

    [Header("Manual Zoom Settings")]
    [SerializeField] private float manualZoomSpeed = 5f;
    [SerializeField] private float manualZoomMin = 1f;
    [SerializeField] private float manualZoomMax = 20f;

    private Camera cam;
    private float targetZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
    }

    private void Update()
    {
        HandleManualZoom();
        ApplyDynamicZoom();
    }

    private void HandleManualZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetZoom -= scroll * manualZoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, manualZoomMin, manualZoomMax);
        }
    }

    private void ApplyDynamicZoom()
    {
        float speed = playerBody.velocity.magnitude;
        float dynamicZoom = Mathf.Lerp(minZoom, maxZoom, Mathf.InverseLerp(minSpeed, maxSpeed, speed));
        float finalZoom = Mathf.Clamp(targetZoom, Mathf.Min(dynamicZoom, manualZoomMin), Mathf.Max(dynamicZoom, manualZoomMax));

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, finalZoom, Time.deltaTime * 5f);
    }
}