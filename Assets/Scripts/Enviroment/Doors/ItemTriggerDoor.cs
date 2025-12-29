using UnityEngine;

public class ItemTriggerDoor : LockedDoorController
{
    [Header("Item Trigger Settings")]
    public string requiredTag = "Useful";
    public Transform triggerZone;
    public float triggerRadius = 2f;

    private GameObject currentTriggerItem;

    protected override bool CanUnlock()
    {
        Collider[] colliders = Physics.OverlapSphere(
            triggerZone.position,
            triggerRadius
        );

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(requiredTag))
            {
                currentTriggerItem = collider.gameObject;
                Debug.Log("Activated");
                return true;
            }
        }

        return false;
    }

    protected override void OnUnlocked()
    {
        base.OnUnlocked();

        if (currentTriggerItem != null)
        {
            Destroy(currentTriggerItem);
            currentTriggerItem = null;
        }

        OpenDoor();
    }

    void OnDrawGizmosSelected()
    {
        if (triggerZone != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(triggerZone.position, triggerRadius);
        }

        base.OnDrawGizmosSelected();
    }
}