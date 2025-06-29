using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [Tooltip("Multiplier for all rotation speeds")]
    private const float ROTATION_SPEED_MULTIPLIER = 1f;

    [Tooltip("Realistic rotation speed in deg/sec")]
    public float rotationSpeed;

    [Tooltip("Axis of rotation (normalized)")]
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        transform.Rotate(rotationAxis.normalized, rotationSpeed * ROTATION_SPEED_MULTIPLIER * Time.deltaTime);
    }
}