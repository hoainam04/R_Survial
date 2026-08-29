using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(-10f, 15f, -10f);

    [Header("Follow")]
    [SerializeField] private float followSpeed = 8f;

    [Header("Zoom")]
    [SerializeField] private float zoom = 1f;

    private Quaternion initialRotation;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: Target is missing!");
            enabled = false;
            return;
        }

        transform.position = target.position + offset * zoom;
        transform.rotation = initialRotation;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset * zoom;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime);

        transform.rotation = initialRotation;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetZoom(float value)
    {
        zoom = Mathf.Max(0.1f, value);
    }
}