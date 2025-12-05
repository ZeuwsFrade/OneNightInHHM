using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Image itemIcon;
    public TMP_Text quantityText;
    public GameObject selectedHighlight;

    private InventoryItem currentItem;
    public int slotIndex { get; set;}


    void Start()
    {
        ClearSlot();
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.color = Color.white;
        }

        if (quantityText != null)
        {
            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
            quantityText.gameObject.SetActive(item.quantity > 1);
        }

    }

    public void ClearSlot()
    {
        currentItem = null;

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.color = new Color(0, 0, 0, 0);
        }

        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.gameObject.SetActive(false);
        }
    }

    public void SetSelected(bool isSelected, Color selectionColor)
    {
        if (selectedHighlight != null)
        {
            selectedHighlight.SetActive(isSelected);
        }

    }

}