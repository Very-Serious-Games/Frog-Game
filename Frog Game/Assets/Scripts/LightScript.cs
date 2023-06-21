using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{

    public Light luz;

    [SerializeField] SimpleSonarShader_ExampleCollision shader;

    
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Frog")){
            luz.intensity = 13;
            shader.PerformVisionRestoredLogic();
            gameObject.SetActive(false);
        }
    }
}