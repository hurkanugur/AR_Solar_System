using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
