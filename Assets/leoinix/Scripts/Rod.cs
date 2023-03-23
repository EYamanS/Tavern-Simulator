using Cinemachine.Utility;
using leoinix.meyhanesimulator.items;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Rod : Item, IUsable
{
    public int RodLevel = 1;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ParticleSystem CaughtParticle;
    [SerializeField] RodBob rbBob;
    [SerializeField] float ThrowSpeed = 50f;
    [SerializeField] float verticalLookOffset = 2f;
    [SerializeField] Transform lineStart;


    private Vector3 initialPosition;
    private Vector3 initialVelocity;
    public int numSegments = 12;

    private float timeOfFlight;
    private float segmentTime;
    private float gravity = 9.81f;

    private bool isfishOnLine = false;    
    private Fish fishOnLine = null;

    [SerializeField] RodState rodState = RodState.None;

    public enum RodState
    {
        None, Aiming, Casted, Fishing, Caught
    }

    public void PrimaryUse()
    {
        if (lineRenderer.positionCount > 0 && rodState == RodState.Aiming)
        {

            ResetSecondary();
            lineRenderer.useWorldSpace = true;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, rbBob.transform.position);

            rbBob.bobTime = rbBob.defaultBobTime;
            rbBob.transform.forward = Camera.main.transform.forward.ProjectOntoPlane(Vector3.up).normalized;
            rbBob.gameObject.SetActive(true);
            rbBob.transform.parent = null;

            rbBob.GetComponent<Rigidbody>().AddForce(initialVelocity, ForceMode.Impulse);

            rodState = RodState.Casted;
        }
        else if (rodState == RodState.Fishing)
        {
            if (isfishOnLine) 
            {
                Fish caughtFish = Instantiate(fishOnLine, rbBob.transform.position + Vector3.up * 2, Quaternion.identity);
                fishOnLine = null;
                isfishOnLine = false;

                caughtFish.LaunchTowards(Camera.main.transform);
            }
            AbortFishing();
        }
    }

    public void SecondaryUse()
    {
        if (!(rodState == RodState.None || rodState == RodState.Aiming)) { return; }
        rodState = RodState.Aiming;

        initialVelocity = (Camera.main.transform.forward + (Camera.main.transform.up * verticalLookOffset)) * ThrowSpeed;
        initialPosition = rbBob.transform.position;

        float maxHeight = initialPosition.y + Mathf.Pow(initialVelocity.y, 2f) / (2f * gravity);
        float timeToMaxHeight = initialVelocity.y / gravity;
        float timeFromMaxHeight = Mathf.Sqrt(2f * maxHeight / gravity);

        // Calculate total time of flight
        timeOfFlight = timeToMaxHeight + timeFromMaxHeight;

        // Calculate segment time
        segmentTime = timeOfFlight / numSegments;
        // Calculate trajectory
        Vector3[] positions = new Vector3[numSegments + 1];
        for (int i = 0; i <= numSegments; i++)
        {
            float t = i * segmentTime;
            float x = initialPosition.x + initialVelocity.x * t;
            float y = initialPosition.y + initialVelocity.y * t - 0.5f * gravity * t * t;
            float z = initialPosition.z + initialVelocity.z * t;
            positions[i] = new Vector3(x, y, z);
        }

        // Set positions for LineRenderer
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
    public void ResetSecondary()
    {
        if (rodState == RodState.None || rodState == RodState.Aiming)
        {
            lineRenderer.positionCount = 0;
        }
    }

    IEnumerator StartFishing(WaterData waterData)
    { 
        if (rodState == RodState.Casted) 
        { 
            rodState = RodState.Fishing; 
            float waitTime = Random.Range(waterData.minWaitTime, waterData.maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Play "fish on line" sound here and particle maybe.
            isfishOnLine = true;
            fishOnLine = waterData.CatchableFishes[Random.Range(0, waterData.CatchableFishes.Length)];
            CaughtParticle.Play();
            yield return new WaitForSeconds(CaughtParticle.main.duration);
            AbortFishing();
        }
    }

    public void AbortFishing()
    { 
        StopAllCoroutines();
        rbBob.bobTime = 0;

        rodState = RodState.None;
        lineRenderer.positionCount = 0;

        rbBob.gameObject.SetActive(false);
        rbBob.transform.parent = transform;
        rbBob.transform.position = lineStart.position;
        rbBob.GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void Dropped()
    {
        Destroy(rbBob.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (rodState)
        {
            case RodState.None: break;
            case RodState.Aiming: break;
            case RodState.Casted:
                {
                    lineRenderer.SetPosition(1, rbBob.transform.position);
                    lineRenderer.SetPosition(0, lineStart.position);
                    break;
                }
            case RodState.Fishing: 
                {
                    lineRenderer.SetPosition(1, rbBob.transform.position);
                    lineRenderer.SetPosition(0, lineStart.position);
                    break;
                }

        }
    }
}
