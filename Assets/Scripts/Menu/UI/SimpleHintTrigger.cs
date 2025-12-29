using UnityEngine;

public class SimpleHintTrigger : MonoBehaviour
{
    [Header("Hint Settings")]
    public string hintText = "Подсказка";
    public float showTime = 3f;            

    [Header("Trigger Settings")]
    public bool showOnEnter = true;      
    public bool hideOnExit = false;       
    public bool oneTimeOnly = false;       

    private SimpleHintUI hintUI;
    private bool wasShown = false;

    void Start()
    {
        hintUI = FindObjectOfType<SimpleHintUI>();

        if (hintUI == null)
        {
            Debug.LogError("SimpleHintUI не найден на сцене!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!showOnEnter) return;
        if (oneTimeOnly && wasShown) return;

        if (other.CompareTag("Player") && hintUI != null)
        {
            if (showTime > 0)
            {
                hintUI.ShowTempHint(hintText, showTime);
            }
            else
            {
                hintUI.ShowHint(hintText);
            }

            wasShown = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!hideOnExit) return;

        if (other.CompareTag("Player") && hintUI != null)
        {
            hintUI.HideHint();
        }
    }

    public void TriggerHint()
    {
        if (hintUI != null)
        {
            if (showTime > 0)
            {
                hintUI.ShowTempHint(hintText, showTime);
            }
            else
            {
                hintUI.ShowHint(hintText);
            }
        }
    }
}