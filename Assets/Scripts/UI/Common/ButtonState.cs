using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string buttonName;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private GameObject selectedBackground;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI BtnNameTxt;
    [SerializeField] private GameObject bgName;

    [Header("Colors")]
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color nonSelectedColor = Color.gray;

    private void Awake()
    {
        SetIconSprite(iconSprite);
        BtnNameTxt.text = buttonName;
    }
    public void SetIconSprite(Sprite sprite)
    {
        if (icon != null)
        {
            icon.sprite = sprite;
        }
    }
    public bool IsSelected { get; private set; }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;

        if (selectedBackground != null)
            selectedBackground.SetActive(selected);

        if (icon != null)
            icon.color = selected
                ? selectedColor
                : nonSelectedColor;

        if (bgName != null)
            bgName.SetActive(selected);

        if (BtnNameTxt != null)
            BtnNameTxt.text = selected
                ? buttonName
                : string.Empty;
    }

    public void Select()
    {
        SetSelected(true);
    }

    public void Deselect()
    {
        SetSelected(false);
    }
}