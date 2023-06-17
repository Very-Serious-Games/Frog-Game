using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private bool isRotating;

    // Zoom Variables
    private float zoom;
    [SerializeField]private float zoomMultiplier;
    private float zoomSpeed = 20f;
    private float smoothTime = 0.25f;
    [SerializeField] private float minZoom = 5f;  // Limit the zoom range
    [SerializeField] private float maxZoom = 50f; // Limit the zoom range

    private void Start() {
        isRotating = false;
        zoom = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(-90f);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(90f);
        }

        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, zoom, ref zoomSpeed, smoothTime);

    }

    private void RotateCamera(float angle)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateCoroutine(angle));
        }
    }

    private System.Collections.IEnumerator RotateCoroutine(float angle)
    {
        isRotating = true;

        Quaternion startRotation = cameraTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y + angle, cameraTransform.eulerAngles.z);

        float duration = 0.5f; // Adjust this value to control the rotation speed
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            cameraTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.rotation = targetRotation;
        isRotating = false;
    }
}