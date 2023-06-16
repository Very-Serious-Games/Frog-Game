using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform;
    private bool isRotating;

    private void Start()
    {
        isRotating = false;
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