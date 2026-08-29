using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour
{
    [Header("Thời gian tồn tại trước khi tự hủy (giây)")]
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}