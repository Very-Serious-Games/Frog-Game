using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonarRingCollision : MonoBehaviour
{
    private Outline collidedOutline;

    public void OnTriggerEnter(Collider collider)
    {
        collidedOutline = collider.GetComponent<Outline>();

        if (collidedOutline != null)
        {
            if (collider.tag != "Player")
            {
                collidedOutline.ActivateOutline();
            }
            else
            {
                Debug.Log("Player collision");
            }
        }
    }
}
