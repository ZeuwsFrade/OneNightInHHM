using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public int quantity;
    public int maxStackSize;
    public CollectableItem originalItem;
    public GameObject itemPrefab; // Префаб для создания объекта при выбрасывании

    public InventoryItem(CollectableItem item)
    {
        itemName = item.itemName;
        icon = item.itemIcon;
        quantity = 1;
        maxStackSize = item.maxStackSize;
        originalItem = item;
        itemPrefab = item.itemPrefab;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Inventory Settings")]
    public int maxSlots = 5;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform slotsContainer;
    public GameObject slotPrefab;

    [Header("Selection Settings")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    private List<InventoryItem> items = new List<InventoryItem>();
    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
    private int selectedSlot = 0;
    private Transform playerTransform;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeUI();
        UpdateUI();
        SelectSlot(0);

        // Находим игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
                break;
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            SelectSlot((selectedSlot - 1 + maxSlots) % maxSlots);
        }
        else if (scroll < 0)
        {
            SelectSlot((selectedSlot + 1) % maxSlots);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropSelectedItem();
        }
    }

    void InitializeUI()
    {
        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }
        slotUIs.Clear();

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
            slotUI.slotIndex = i;
            slotUIs.Add(slotUI);

        }
    }

    public bool AddItem(CollectableItem item)
    {
        foreach (InventoryItem invItem in items)
        {
            if (invItem.itemName == item.itemName && invItem.quantity < invItem.maxStackSize)
            {
                invItem.quantity++;
                item.Pickup();
                UpdateUI();
                return true;
            }
        }

        if (items.Count >= maxSlots)
        {
            Debug.Log("Инвентарь полон!");
            return false;
        }

        InventoryItem newItem = new InventoryItem(item);
        items.Add(newItem);
        item.Pickup();
        UpdateUI();
        return true;
    }

    void SelectSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < maxSlots)
        {
            if (selectedSlot >= 0 && selectedSlot < slotUIs.Count)
            {
                slotUIs[selectedSlot].SetSelected(false, normalColor);
            }

            selectedSlot = slotIndex;
            slotUIs[selectedSlot].SetSelected(true, selectedColor);

            Debug.Log($"Выбран слот {selectedSlot + 1}");
        }
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Count)
        {
            items[slotIndex].quantity--;

            if (items[slotIndex].quantity <= 0)
            {
                items.RemoveAt(slotIndex);
            }

            UpdateUI();
        }
    }

    void DropSelectedItem()
    {
        if (selectedSlot < items.Count && playerTransform != null)
        {
            InventoryItem itemToDrop = items[selectedSlot];

            // Получаем префаб из CollectableItem
            GameObject prefabToDrop = itemToDrop.itemPrefab;

            // Альтернативно: если префаб не назначен, пробуем загрузить по имени
            if (prefabToDrop == null && itemToDrop.originalItem != null)
            {
                prefabToDrop = itemToDrop.originalItem.GetItemPrefab();
            }

            if (prefabToDrop != null)
            {
                Vector3 dropPosition = playerTransform.position + playerTransform.forward * 2f + Vector3.up;
                GameObject droppedItem = Instantiate(prefabToDrop, dropPosition, Quaternion.identity);

                // Добавляем Rigidbody если его нет
                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = droppedItem.AddComponent<Rigidbody>();
                }

                // Бросаем предмет вперед
                rb.AddForce(playerTransform.forward * 5f, ForceMode.Impulse);

                Debug.Log($"Выброшен предмет: {itemToDrop.itemName}");
            }
            else
            {
                Debug.LogWarning($"У предмета {itemToDrop.itemName} нет префаба для выбрасывания");
            }

            RemoveItem(selectedSlot);
        }
    }

    void UpdateUI()
    {
        // Обновляем все слоты
        for (int i = 0; i < maxSlots; i++)
        {
            if (i < items.Count)
            {
                slotUIs[i].SetItem(items[i]);
            }
            else
            {
                slotUIs[i].ClearSlot();
            }

            // Обновляем выделение
            slotUIs[i].SetSelected(i == selectedSlot, i == selectedSlot ? selectedColor : normalColor);
        }
    }

    void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);
        }
    }

    public InventoryItem GetSelectedItem()
    {
        if (selectedSlot < items.Count)
        {
            return items[selectedSlot];
        }
        return null;
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }
}