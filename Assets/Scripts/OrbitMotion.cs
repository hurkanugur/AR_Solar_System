using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    [Tooltip("Reference to the Sun object to orbit around")]
    public Transform sun;

    [Tooltip("Orbit speed in degrees per second")]
    public float orbitSpeed;

    [Tooltip("Orbit axis, usually Vector3.up for solar system")]
    public Vector3 orbitAxis = Vector3.up;

    void Update()
    {
        // Orbit the Sun
        transform.RotateAround(sun.position, orbitAxis, orbitSpeed * Time.deltaTime);
    }
}
