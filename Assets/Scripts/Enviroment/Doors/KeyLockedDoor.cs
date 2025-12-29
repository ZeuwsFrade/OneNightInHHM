using UnityEngine;

public class KeyLockedDoor : LockedDoorController
{
    [Header("Key Settings")]
    public string requiredKeyName = "Key";
    public bool consumeKey = true;

    protected override bool CanUnlock()
    {
        Inventory inventory = Inventory.Instance;
        if (inventory == null)
        {
            Debug.LogWarning("Inventory не найден!");
            return false;
        }

        foreach (InventoryItem item in inventory.GetItems())
        {
            if (item.itemName == requiredKeyName)
            {
                if (consumeKey)
                {
                    inventory.RemoveItem(inventory.GetItems().IndexOf(item));
                }
                return true;
            }
        }

        return false;
    }

    public override void Interact()
    {
        if (isLocked)
        {
            bool unlocked = TryUnlock();
            if (unlocked)
            {
                base.Interact();
            }
        }
        else
        {
            base.Interact();
        }
    }
}