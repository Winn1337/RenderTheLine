using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    [Header("Flips")]
    private float rotation;
    [SerializeField] private float flips;
    [SerializeField] private float totalFlips;

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

        SpeedStats();
        AirTimeStats();
        FlipStats();
        UpdateUI();

        // Utility
        colliding = false;
        samples++;
    }

    private void SpeedStats()
    {
        speed = rigidBody.velocity.magnitude;
        topSpeed = Mathf.Max(speed, topSpeed);
        avgSpeed = AddToAverage(avgSpeed, samples, speed);
    }

    private void AirTimeStats()
    {
        if (colliding)
        {
            airTime = 0;
            return;
        }

        airTime += Time.fixedDeltaTime;
        totalAirTime += Time.fixedDeltaTime;
        topAirTime = Mathf.Max(airTime, topAirTime);
    }

    private void FlipStats()
    {
        //if at least one wheel is off the ground, start counting rotation
        if (!colliding)
        {
            rotation += rigidBody.angularVelocity * Time.fixedDeltaTime;
            float _180sThisFrame = (int)(Mathf.Abs(rotation) / 180f);
            Debug.Log(rotation + " " + _180sThisFrame);
            rotation -= 180f * _180sThisFrame * Mathf.Sign(rotation);
            flips += _180sThisFrame * 0.5f;
            totalFlips += _180sThisFrame * 0.5f;
        }
        else
        {
            rotation = 0f;
            flips = 0;
        }
    }
    private void UpdateUI()
    {
        textMesh.text = speed.ToString("00.0") + '\n' +
                        topSpeed.ToString("00.0") + '\n' +
                        avgSpeed.ToString("00.0") + '\n' +
                        airTime.ToString("00.0") + '\n' +
                        topAirTime.ToString("00.0") + '\n' +
                        totalAirTime.ToString("00.0") + '\n' +
                        flips.ToString("00.0") + '\n' +
                        totalFlips.ToString("00.0");
    }

    public void Restart()
    {
        speed = 0;
        topSpeed = 0;
        avgSpeed = 0;
        airTime = 0;
        topAirTime = 0;
        totalAirTime = 0;
        flips = 0;
        totalFlips = 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        colliding = true;
    }

    private float AddToAverage(float average, int size, float value) => (size * average + value) / (size + 1);
}
