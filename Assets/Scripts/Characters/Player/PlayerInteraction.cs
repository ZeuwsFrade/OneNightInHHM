using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public LayerMask interactionLayer;

    [Header("References")]
    public Transform itemDropPoint;

    [Header("Death Menu")]
    public string gameSceneName = "DeathMenu";


    private Camera playerCamera;
    private DoorController currentDoor;
    private CollectableItem currentCollectable;
    private Inventory inventory;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            Debug.LogWarning("PlayerInteraction: Камера не найдена на игроке, используется Main Camera");
        }
        GameObject inventoryManager = GameObject.Find("InventoryManager");
        if (inventoryManager != null)
        {
            inventory = inventoryManager.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogError("PlayerInteraction: Не найден InventoryManager на сцене!");
        }
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
            // Проверяем сначала на LockedDoorController
            LockedDoorController lockedDoor = hit.collider.GetComponent<LockedDoorController>();
            if (lockedDoor != null)
            {
                currentDoor = lockedDoor; // DoorController является родительским классом
                currentCollectable = null;
                return;
            }

            // Проверяем обычную дверь
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
                if (inventory.AddItem(currentCollectable))
                {
                    Debug.Log($"Подобран предмет: {currentCollectable.itemName}");
                }
                else
                {
                    Debug.Log("Не удалось подобрать предмет.");
                }
            }
        }
    }

    void OnGUI()
    {
        if (currentDoor != null)
        {
            string prompt = "Нажмите E чтобы взаимодействовать";

            LockedDoorController lockedDoor = currentDoor as LockedDoorController;
            if (lockedDoor != null && lockedDoor.isLocked)
            {
                if (lockedDoor is KeyLockedDoor keyDoor)
                {
                    prompt = $"Дверь заперта\nТребуется ключ: {keyDoor.requiredKeyName}";
                }
                else if (lockedDoor is ItemTriggerDoor itemDoor)
                {
                    prompt = $"Дверь заперта\nБросьте предмет с тегом '{itemDoor.requiredTag}' в зону";
                }
                else
                {
                    prompt = "Дверь заперта";
                }
            }

            ShowInteractionPrompt(prompt);
        }
        else if (currentCollectable != null)
        {
            ShowInteractionPrompt($"Нажмите E чтобы подобрать {currentCollectable.itemName}");
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

    public void Death()
    {
        StartCoroutine(LoadGameScene());
    }

    private System.Collections.IEnumerator LoadGameScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return null;
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game scene name is not set!");
        }
    }
}