using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName = "Item";
    public Sprite itemIcon;
    public int maxStackSize = 1;
    public GameObject itemPrefab;

    [Header("Prefab Settings")]
    public string prefabPath;

    [Header("Visual Settings")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private Vector3 startPosition;
    private float randomOffset;

    private GameObject cachedPrefab;

    void Start()
    {
        if (string.IsNullOrEmpty(prefabPath))
        {
            prefabPath = $"Prefabs/{itemName}";
        }
        GetItemPrefab();
        itemPrefab = Resources.Load<GameObject>(prefabPath);
    }

    public GameObject GetItemPrefab()
    {
        if (cachedPrefab == null && !string.IsNullOrEmpty(prefabPath))
        {
            cachedPrefab = Resources.Load<GameObject>(prefabPath);
            if (cachedPrefab == null)
            {
                Debug.LogWarning($"Не найден префаб по пути: {prefabPath}");
            }
        }
        return cachedPrefab;
    }


    public void Pickup()
    {
        // Визуальные эффекты
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        Destroy(gameObject);
    }
}