using UnityEngine;

public class TeleportTrap : TrapBase
{
    [Header("Настройки телепорта")]
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private Vector3 teleportOffset = Vector3.zero;
    [SerializeField] private bool maintainPlayerRotation = false;
    [SerializeField] private bool fadeScreen = false;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Эффекты телепорта")]
    [SerializeField] private ParticleSystem teleportParticles;
    [SerializeField] private AudioClip teleportSound;

    protected override void PlayActivationEffects()
    {
        base.PlayActivationEffects();

        if (teleportParticles != null)
        {
            teleportParticles.Play();
        }

        if (teleportSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }
    }

    protected override void ActivateTrapEffect()
    {
        if (playerObject == null || teleportDestination == null)
        {
            Debug.LogError("TeleportTrap: Игрок или точка назначения не найдены!");
            return;
        }

        CharacterController controller = playerObject.GetComponent<CharacterController>();

        if (controller != null)
        {
            // Отключаем контроллер для телепортации
            controller.enabled = false;

            // Выполняем телепортацию
            ExecuteTeleport();

            // Включаем контроллер обратно
            controller.enabled = true;
        }
        else
        {
            // Если нет CharacterController, просто меняем позицию
            ExecuteTeleport();
        }

        Debug.Log($"Игрок телепортирован в {teleportDestination.position}");
    }

    private void ExecuteTeleport()
    {
        // Вычисляем конечную позицию
        Vector3 targetPosition = teleportDestination.position + teleportOffset;

        // Телепортируем игрока
        playerObject.transform.position = targetPosition;

        // Сохраняем или меняем вращение
        if (!maintainPlayerRotation)
        {
            playerObject.transform.rotation = teleportDestination.rotation;
        }

        // Эффект после телепортации
        if (teleportParticles != null)
        {
            ParticleSystem particles = Instantiate(teleportParticles, targetPosition, Quaternion.identity);
            Destroy(particles.gameObject, 3f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (teleportDestination != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, teleportDestination.position);
            Gizmos.DrawWireSphere(teleportDestination.position, 0.5f);
        }
    }
}