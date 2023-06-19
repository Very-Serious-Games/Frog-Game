// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleSonarShader_ExampleCollision : MonoBehaviour
{
    public event EventHandler<Vector3> OnSonarLogic;

    [SerializeField] float divisorPotencia = 2;

    public GameObject sonarRingPrefab; // The sonar ring prefab

    private GameObject activeSonarRing; // Reference to the active sonar ring

    public void PerformSonarLogic(Vector3 point, float force)
    {
        if (activeSonarRing == null)
        {
            StartSonarRing(point, force / 10.0f);
            OnSonarLogic?.Invoke(this, point);

            // Start sonar ring from the specified point
            SimpleSonarShader_Parent parent = GetComponentInParent<SimpleSonarShader_Parent>();
            if (parent) parent.StartSonarRing(point, force / 10.0f);
        }
    }

    private void StartSonarRing(Vector3 position, float force)
    {
        activeSonarRing = Instantiate(sonarRingPrefab, position, Quaternion.identity);
        StartCoroutine(ExpandSonarRing(activeSonarRing, force));
    }

    private IEnumerator ExpandSonarRing(GameObject sonarRing, float force)
    {
        float radius = 0.0f;
        float expansionSpeed = force;

        while (radius < force)
        {
            radius += expansionSpeed * Time.deltaTime;
            sonarRing.transform.localScale = new Vector3(radius, sonarRing.transform.localScale.y, radius);
            yield return null;
        }

        Destroy(sonarRing);
        activeSonarRing = null; // Reset the active sonar ring reference

    }

    void OnCollisionEnter(Collision collision)
    {
        PerformSonarLogic(collision.contacts[0].point, collision.impulse.magnitude / divisorPotencia);
    }
}
