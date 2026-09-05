using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PROJ.UI;

public class ItemAttributeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attributeNameText;
    [SerializeField] private TextMeshProUGUI attributeValueText;

    public void SetData(string attributeName, string attributeValue)
    {
        if (attributeNameText != null)
        {
            attributeNameText.text = attributeName;
        }

        if (attributeValueText != null)
        {
            attributeValueText.text = attributeValue;
        }
    }
}