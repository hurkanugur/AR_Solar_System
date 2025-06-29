using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    // Rotation speed in degrees per second (realistic speeds are scaled)
    [Tooltip("Rotation speed in degrees per second")]
    public float rotationSpeed;

    // Axis of rotation
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        // Rotate around the axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}