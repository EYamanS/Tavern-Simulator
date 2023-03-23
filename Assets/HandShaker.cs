using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandShaker : MonoBehaviour
{

    public Transform handBone;
    public float maxShakeAmount = 0.1f;
    public float shakeSpeed = 5f;
    public float noiseScale = 0.5f;

    public bool doShake = false;
    
    private Vector3 originalPos;
    private float shakeAmount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = handBone.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (doShake)
        {
            // Calculate new shake amount
            shakeAmount = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * maxShakeAmount;

            // Apply shake to hand bone position
            Vector3 shakeVector = new Vector3(
                Mathf.PerlinNoise(Time.time * shakeSpeed, 1f) - 0.5f,
                Mathf.PerlinNoise(Time.time * shakeSpeed, 2f) - 0.5f,
                Mathf.PerlinNoise(Time.time * shakeSpeed, 3f) - 0.5f);
            handBone.localPosition = originalPos + shakeVector * shakeAmount * noiseScale;
        }

        else
        {
            // Reset hand position if not walking
            handBone.localPosition = originalPos;
        }
    }
}
