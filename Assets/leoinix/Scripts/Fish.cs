using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void LaunchTowards(Transform LaunchTarget)
    {
        Vector3 delta = LaunchTarget.position - transform.position;
        float timeToTarget = Mathf.Sqrt(2 * delta.magnitude / Physics.gravity.magnitude);
        Vector3 initialVelocity = delta / timeToTarget - 0.5f * Physics.gravity * timeToTarget;
        rb.velocity = initialVelocity;
    }
}
