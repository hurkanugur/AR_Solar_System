using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    public float moonRotationSpeed = 0.608f;

    private Quaternion desiredGlobalRotation;

    void Start()
    {
        desiredGlobalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        desiredGlobalRotation *= Quaternion.Euler(0f, moonRotationSpeed * Time.deltaTime, 0f);
        transform.rotation = desiredGlobalRotation;
    }
}