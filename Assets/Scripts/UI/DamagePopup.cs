using UnityEngine;
using TMPro;

namespace PROJ.UI
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI popupText;
        [SerializeField] private float moveSpeed = 60f;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color critColor = Color.yellow;
        [SerializeField] private float normalScale = 1f;
        [SerializeField] private float critScale = 1.5f;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private float timer;
        private Vector2 randomDirection;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void Initialize(float damage, bool isCritical = false)
        {
            if (popupText != null)
            {
                popupText.text = isCritical ? $"-{Mathf.Ceil(damage)}" : $"-{Mathf.Ceil(damage)}";
                popupText.color = isCritical ? critColor : normalColor;
            }

            transform.localScale = Vector3.one * (isCritical ? critScale : normalScale);

            // Tạo hướng bay ngẫu nhiên (lệch trái hoặc phải một chút để không bị thẳng hàng)
            float randomX = Random.Range(-0.5f, 0.5f);
            randomDirection = new Vector2(randomX, 1f).normalized;

            timer = fadeDuration;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                
                // Di chuyển theo hướng ngẫu nhiên
                rectTransform.anchoredPosition += randomDirection * (moveSpeed * Time.deltaTime);

                // Mờ dần
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = timer / fadeDuration;
                }

                if (timer <= 0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
