using System;
using System.Collections;
using UnityEngine;

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
            StartSonarRing(point, force);
            OnSonarLogic?.Invoke(this, point);

            // Start sonar ring from the specified point
            SimpleSonarShader_Parent parent = GetComponentInParent<SimpleSonarShader_Parent>();
            if (parent) parent.StartSonarRing(point, force);
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
        float expansionSpeed = 20.0f; // Adjust the expansion speed here

        SphereCollider collider = sonarRing.GetComponent<SphereCollider>();


        while (radius < force)
        {
            radius += expansionSpeed * Time.deltaTime;
            sonarRing.transform.localScale = new Vector3(radius, sonarRing.transform.localScale.y, radius);
            collider.radius = radius;

            if (radius >= force)
            {
                // Stop the expansion when the radius exceeds the force value
                sonarRing.transform.localScale = new Vector3(force, sonarRing.transform.localScale.y, force);
                collider.radius = force; // Set the collider radius to the final value
                break;
            }

            yield return null;
        }

        Destroy(sonarRing);
        activeSonarRing = null; // Reset the active sonar ring reference
    }

    void OnCollisionEnter(Collision collision)
    {
        PerformSonarLogic(collision.contacts[0].point, collision.impulse.magnitude / divisorPotencia);
        Debug.Log("sa chocao");
    }
}
