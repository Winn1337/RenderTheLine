using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicZoom : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Lerp(minZoom, maxZoom, Mathf.InverseLerp(minSpeed, maxSpeed, playerBody.velocity.magnitude));
    }
}
