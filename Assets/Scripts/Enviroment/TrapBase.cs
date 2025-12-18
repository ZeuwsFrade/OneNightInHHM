using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    [Header("Базовые настройки")]
    [SerializeField] protected string playerTag = "Player";
    [SerializeField] protected bool isOneTimeUse = true;
    [SerializeField] protected bool isActive = true;
    [SerializeField] protected float activationDelay = 0f;

    [Header("Визуальные эффекты")]
    [SerializeField] protected ParticleSystem activationEffect;
    [SerializeField] protected AudioClip activationSound;
    [SerializeField] protected AudioSource audioSource;

    protected bool hasBeenTriggered = false;
    protected GameObject playerObject;

    void Start()
    {
        // Настройка аудио
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Автоматическая настройка коллайдера триггера
        SetupTriggerCollider();
    }

    void SetupTriggerCollider()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
        }
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (hasBeenTriggered && isOneTimeUse) return;
        if (!other.CompareTag(playerTag)) return;

        playerObject = other.gameObject;
        StartCoroutine(ActivateTrap());
    }

    private System.Collections.IEnumerator ActivateTrap()
    {
        if (activationDelay > 0)
        {
            yield return new WaitForSeconds(activationDelay);
        }

        // Визуальные и звуковые эффекты
        PlayActivationEffects();

        // Вызываем абстрактный метод активации
        ActivateTrapEffect();

        hasBeenTriggered = true;

        // Деактивируем объект если одноразовый
        if (isOneTimeUse)
        {
            DeactivateTrap();
        }
    }

    protected virtual void PlayActivationEffects()
    {
        // Звуковой эффект
        if (activationSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(activationSound);
        }

        // Визуальный эффект
        if (activationEffect != null)
        {
            activationEffect.Play();
        }
    }

    protected virtual void DeactivateTrap()
    {
        isActive = false;

        // Отключаем визуально
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Отключаем коллайдер
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Уничтожаем через 3 секунды (после завершения эффектов)
        Destroy(gameObject, 3f);
    }

    // Абстрактный метод для реализации в дочерних классах
    protected abstract void ActivateTrapEffect();

    // Методы для управления ловушкой
    public void SetActive(bool active) => isActive = active;
    public void ResetTrap()
    {
        hasBeenTriggered = false;
        isActive = true;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = true;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = true;
    }
}