using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    [SerializeField] public Transform ToFollow;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        if (!ToFollow) return;

        transform.position = Vector3.Lerp(transform.position, ToFollow.position + offset, lerpSpeed * Time.deltaTime);
    }
}
