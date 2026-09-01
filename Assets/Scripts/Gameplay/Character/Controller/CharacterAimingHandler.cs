using UnityEngine;
using PROJ.Attributes;

public class CharacterAimingHandler : MonoBehaviour
{
    [Header("Cấu Hình Nhắm Bắn (Aim Settings)")]
    [SerializeField] private bool useAutoAim = true;
    [SerializeField] private float autoAimRadius = 7f;
    [SerializeField] private LayerMask enemyLayer;

    private CharacterAttributeManager attributeManager;
    private Transform currentTargetEnemy;

    private void Awake()
    {
        attributeManager = GetComponent<CharacterAttributeManager>();
    }

    public Transform GetCurrentTargetEnemy(Transform characterTransform)
    {
        if (attributeManager != null && attributeManager.GetAttribute(AttributeType.AttackRange) != null)
        {
            autoAimRadius = attributeManager.GetAttribute(AttributeType.AttackRange).CurrentValue;
        }

        Collider[] closeEnemies = Physics.OverlapSphere(characterTransform.position, autoAimRadius, enemyLayer);
        if (closeEnemies.Length == 0) return null;

        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (var enemy in closeEnemies)
        {
            float dist = Vector3.Distance(characterTransform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestEnemy = enemy.transform;
            }
        }

        currentTargetEnemy = nearestEnemy;
        return currentTargetEnemy;
    }

    public Vector3 CalculateAimDirection(Transform characterTransform)
    {
        if (!useAutoAim)
        {
            // --- CHẾ ĐỘ PC: Nhắm bằng vị trí con trỏ chuột ---
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, characterTransform.position);

            if (playerPlane.Raycast(ray, out float enterDistance))
            {
                Vector3 targetPoint = ray.GetPoint(enterDistance);
                targetPoint.y = characterTransform.position.y; // Triệt tiêu độ lệch trục Y
                
                Vector3 dir = (targetPoint - characterTransform.position).normalized;
                return dir != Vector3.zero ? dir : characterTransform.forward;
            }
        }
        else
        {
            Transform target = GetCurrentTargetEnemy(characterTransform);
            if (target != null)
            {
                Vector3 enemyPos = target.position;
                enemyPos.y = characterTransform.position.y; // Triệt tiêu trục Y
                return (enemyPos - characterTransform.position).normalized;
            }
        }

        return characterTransform.forward;
    }
}
