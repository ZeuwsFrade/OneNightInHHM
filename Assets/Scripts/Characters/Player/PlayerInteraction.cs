using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public LayerMask interactionLayer;

    [Header("References")]
    public Transform itemDropPoint;

    private Camera playerCamera;
    private DoorController currentDoor;
    private CollectableItem currentCollectable;
    private Inventory inventory;

    void Start()
    {
        // Находим камеру
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            Debug.LogWarning("PlayerInteraction: Камера не найдена на игроке, используется Main Camera");
        }

        // Находим Inventory через InventoryManager
        GameObject inventoryManager = GameObject.Find("InventoryManager");
        if (inventoryManager != null)
        {
            inventory = inventoryManager.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogError("PlayerInteraction: Не найден InventoryManager на сцене!");
        }

        // Если точка выброса не назначена, создаем её
        if (itemDropPoint == null)
        {
            GameObject dropPoint = new GameObject("ItemDropPoint");
            dropPoint.transform.parent = transform;
            dropPoint.transform.localPosition = new Vector3(0, 1, 2);
            itemDropPoint = dropPoint.transform;
        }
    }

    void Update()
    {
        CheckForInteractable();
        HandleInteractionInput();
    }

    void CheckForInteractable()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            DoorController door = hit.collider.GetComponent<DoorController>();
            CollectableItem collectable = hit.collider.GetComponent<CollectableItem>();

            if (door != null)
            {
                currentDoor = door;
                currentCollectable = null;
                return;
            }
            else if (collectable != null)
            {
                currentCollectable = collectable;
                currentDoor = null;
                return;
            }
        }

        currentDoor = null;
        currentCollectable = null;
    }

    void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (currentDoor != null)
            {
                currentDoor.Interact();
            }
            else if (currentCollectable != null && inventory != null)
            {
                // Подбираем предмет
                if (inventory.AddItem(currentCollectable))
                {
                    Debug.Log($"Подобран предмет: {currentCollectable.itemName}");
                }
                else
                {
                    Debug.Log("Не удалось подобрать предмет. Инвентарь может быть полон.");
                }
            }
        }
    }

    void OnGUI()
    {
        // Показ подсказки для взаимодействия
        if (currentDoor != null)
        {
            ShowInteractionPrompt("Нажмите E чтобы взаимодействовать");
        }
        else if (currentCollectable != null)
        {
            ShowInteractionPrompt($"Нажмите E чтобы подобрать {currentCollectable.itemName}");
        }

        // Показ выбранного предмета в правом верхнем углу
        if (inventory != null)
        {
            InventoryItem selectedItem = inventory.GetSelectedItem();
            if (selectedItem != null)
            {
                GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                boxStyle.alignment = TextAnchor.MiddleLeft;
                boxStyle.fontSize = 14;
                boxStyle.normal.textColor = Color.white;

                GUI.Box(new Rect(Screen.width - 210, 10, 200, 60),
                       $"Выбран: {selectedItem.itemName}");
            }
        }
    }

    void ShowInteractionPrompt(string text)
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 16;
        style.normal.textColor = Color.white;
        style.wordWrap = true;

        float width = 300;
        float height = 50;
        float x = (Screen.width - width) / 2;
        float y = Screen.height - 120;

        GUI.Box(new Rect(x, y, width, height), text, style);
    }
}