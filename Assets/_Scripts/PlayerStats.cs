using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float topSpeed;
    [SerializeField] private float avgSpeed;

    [Header("Air time")]
    [SerializeField] private float airTime;
    [SerializeField] private float topAirTime;
    [SerializeField] private float totalAirTime;

    private Rigidbody2D rigidBody;
    int samples;
    bool colliding;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!textMesh) return;
        if (!rigidBody.simulated) return;

        // Get speed stats
        speed = rigidBody.velocity.magnitude;
        topSpeed = Mathf.Max(speed, topSpeed);
        avgSpeed = AddToAverage(avgSpeed, samples, speed);

        // Get air time stats
        if (colliding)
        {
            airTime = 0;
        }
        else
        {
            airTime += Time.fixedDeltaTime;
            totalAirTime += Time.fixedDeltaTime;
            topAirTime = Mathf.Max(airTime, topAirTime);
        }

        // Update UI
        textMesh.text = speed.ToString("00.0") + '\n' +
                        topSpeed.ToString("00.0") + '\n' +
                        avgSpeed.ToString("00.0") + '\n' +
                        airTime.ToString("00.0") + '\n' +
                        topAirTime.ToString("00.0") + '\n' +
                        totalAirTime.ToString("00.0");

        // Utility
        colliding = false;
        samples++;
    }

    public void Restart()
    {
        speed = 0;
        topSpeed = 0;
        avgSpeed = 0;
        airTime = 0;
        topAirTime = 0;
        totalAirTime = 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        colliding = true;
    }

    private float AddToAverage(float average, int size, float value) => (size * average + value) / (size + 1);
}
