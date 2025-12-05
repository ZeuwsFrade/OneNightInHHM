using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Настройки текста")]
    public TextMeshProUGUI buttonText;

    [Header("Эффекты")]
    public bool enableScaleEffect = true;
    public float highlightScale = 1.1f;

    private Vector3 originalScale;

    void Start()
    {
        // Находим текстовый компонент автоматически, если не задан
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Сохраняем оригинальный масштаб
        originalScale = buttonText.transform.localScale;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnhighlightText();
    }

    private void HighlightText()
    {
        if (buttonText != null)
        {
            if (enableScaleEffect)
            {
                buttonText.transform.localScale = originalScale * highlightScale;
            }
        }
    }

    private void UnhighlightText()
    {
        if (buttonText != null)
        {
            if (enableScaleEffect)
            {
                buttonText.transform.localScale = originalScale;
            }
        }
    }
}