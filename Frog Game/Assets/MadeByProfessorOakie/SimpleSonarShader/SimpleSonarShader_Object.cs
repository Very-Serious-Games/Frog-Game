using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_Object : MonoBehaviour
{
    [SerializeField] private float waveSizeMultiplier = 1.0f;
    [SerializeField] private float waveSpeedMultiplier = 1.0f;

    private Renderer[] objectRenderers;
    private static readonly Vector4 garbagePosition = new Vector4(-5000, -5000, -5000, -5000);
    private static int queueSize = 20;
    private static Queue<Vector4> positionsQueue = new Queue<Vector4>(queueSize);
    private static Queue<float> intensityQueue = new Queue<float>(queueSize);
    private static bool needToInitQueues = true;
    private static Action ringDelegate;

    private void Start()
    {
        objectRenderers = GetComponentsInChildren<Renderer>();

        if (needToInitQueues)
        {
            needToInitQueues = false;
            for (int i = 0; i < queueSize; i++)
            {
                positionsQueue.Enqueue(garbagePosition);
                intensityQueue.Enqueue(-5000f);
            }
        }

        ringDelegate += SendSonarData;
    }

    public void StartSonarRing(Vector4 position, float intensity)
    {
        position.w = Time.timeSinceLevelLoad;
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(position);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        ringDelegate();
    }

    private void SendSonarData()
    {
        foreach (Renderer r in objectRenderers)
        {
            r.material.SetVectorArray("_hitPts", positionsQueue.ToArray());
            r.material.SetFloatArray("_Intensity", intensityQueue.ToArray());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float waveSize = collision.impulse.magnitude / 10.0f * waveSizeMultiplier;
        float waveSpeed = collision.impulse.magnitude / 10.0f * waveSpeedMultiplier;
        StartSonarRing(collision.contacts[0].point, waveSize);
    }

    private void OnDestroy()
    {
        ringDelegate -= SendSonarData;
    }
}
