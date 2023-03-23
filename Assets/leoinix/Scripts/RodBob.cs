using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RodBob : MonoBehaviour
{
    Rigidbody rb;
    public float bobTime = 2f * 1.25f;
    public float defaultBobTime = 2f * 1.25f;
    [SerializeField] Vector3 bobOffset = Vector3.up;
    bool bobbing = false;
    private Rod rod;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rod = transform.parent.GetComponent<Rod>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.TryGetComponent<FishPool>(out FishPool pool) && !bobbing)
        {
            if (pool.waterData.minRodLevel <= rod.RodLevel)
            {
                rb.isKinematic = true;
                Bob();

                rod.StartCoroutine("StartFishing",pool.waterData);
            }
        }
    }

    private void Bob()
    {
        bobbing = true;
        bobTime = bobTime * 0.8f;
        if (bobTime < 0.1f) { bobbing = false; rod.AbortFishing(); return; }
        transform.DOMove(transform.position + bobOffset, bobTime).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            transform.DOMove(transform.position - bobOffset, bobTime).SetEase(Ease.InOutBack).OnComplete(() => Bob());
        });
    }
}
