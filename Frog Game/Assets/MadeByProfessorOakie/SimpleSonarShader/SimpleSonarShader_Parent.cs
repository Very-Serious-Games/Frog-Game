using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_Parent : MonoBehaviour
{
    [SerializeField] private float waveSizeMultiplier = 1.0f;
    [SerializeField] private float waveSpeedMultiplier = 1.0f;

    private Renderer[] objectRenderers;
    private static readonly Vector4 garbagePosition = new Vector4(-5000, -5000, -5000, -5000);
    private static int queueSize = 20;
    private Queue<Vector4> positionsQueue = new Queue<Vector4>(queueSize);
    private Queue<float> intensityQueue = new Queue<float>(queueSize);

    private void Start()
    {
        objectRenderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < queueSize; i++)
        {
            positionsQueue.Enqueue(garbagePosition);
            intensityQueue.Enqueue(-5000f);
        }
    }

    public void StartSonarRing(Vector4 position, float intensity)
    {
        position.w = Time.timeSinceLevelLoad;
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(position);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        foreach (Renderer r in objectRenderers)
        {
            if (r)
            {
                r.material.SetVectorArray("_hitPts", positionsQueue.ToArray());
                r.material.SetFloatArray("_Intensity", intensityQueue.ToArray());
                r.material.SetFloat("_WaveSizeMultiplier", waveSizeMultiplier);
                r.material.SetFloat("_WaveSpeedMultiplier", waveSpeedMultiplier);
            }
        }
    }
}
