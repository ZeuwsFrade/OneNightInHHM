using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleHintUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject hintPanel;    // Панель подсказки
    public TMP_Text hintText;           // Текст подсказки

    [Header("Settings")]
    public float fadeSpeed = 5f;    // Скорость появления/исчезновения

    private CanvasGroup canvasGroup;
    private bool isShowing = false;

    void Start()
    {
        canvasGroup = hintPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = hintPanel.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        hintPanel.SetActive(false);
    }

    void Update()
    {
        if (isShowing && canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
        }
        else if (!isShowing && canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);

            if (canvasGroup.alpha <= 0f)
            {
                hintPanel.SetActive(false);
            }
        }
    }

    public void ShowHint(string text)
    {
        hintText.text = text;
        hintPanel.SetActive(true);
        isShowing = true;
    }

    public void HideHint()
    {
        isShowing = false;
    }

    public void ShowTempHint(string text, float time)
    {
        ShowHint(text);
        Invoke("HideHint", time);
    }
}