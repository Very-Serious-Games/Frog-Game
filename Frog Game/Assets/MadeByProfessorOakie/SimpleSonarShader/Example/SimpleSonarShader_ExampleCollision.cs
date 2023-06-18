// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System;
using System.Collections;
using UnityEngine;


public class SimpleSonarShader_ExampleCollision : MonoBehaviour
{
    [SerializeField] float divisiorPotencia = 2;    

    
    public event EventHandler<Vector3> OnSonarLogic;

    public void PerformSonarLogic(Vector3 point, float force)
    {
        OnSonarLogic?.Invoke(this, point);

        // Start sonar ring from the specified point
        SimpleSonarShader_Parent parent = GetComponentInParent<SimpleSonarShader_Parent>();
        if (parent) parent.StartSonarRing(point, force / 10.0f);
    }
    
    void OnCollisionEnter(Collision collision)
    {

        SimpleSonarShader_Parent parent = GetComponentInParent<SimpleSonarShader_Parent>();
        if (parent) parent.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / divisiorPotencia);

    }
}
