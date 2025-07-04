using UnityEngine;

public class CamLookAt : MonoBehaviour
{
    public Transform[] targets;  

    private Vector3 initialOffset;  
    private bool offsetInitialized = false;

    void Start()
    {
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("CamLookAt: targets dizisi boş!");
            return;
        }

        Vector3 startCenter = GetCenterPoint();
        initialOffset = transform.position - startCenter;
        offsetInitialized = true;
    }

    void LateUpdate()
    {
        if (!offsetInitialized) return;

        Vector3 center = GetCenterPoint();

        transform.position = center + initialOffset;

        // Kamerayı merkeze doğru çevir
        transform.LookAt(center);
    }

    Vector3 GetCenterPoint()
    {
        Vector3 sum = Vector3.zero;
        foreach (Transform t in targets)
            sum += t.position;
        return sum / targets.Length;
    }
}