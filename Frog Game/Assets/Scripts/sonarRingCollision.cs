using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonarRingCollision : MonoBehaviour
{
    public Shader newShader; // Reference to the new shader

    private void OnTriggerEnter(Collider collider)
    {
        // Check if the collider has a Renderer component
        Renderer renderer = collider.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create a new material with the desired shader
            Material newMaterial = new Material(newShader);
            // Assign the new material to the renderer
            renderer.material = newMaterial;
        }
    }
}
