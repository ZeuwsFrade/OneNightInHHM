using UnityEngine;

public class SlowTrap : TrapBase
{
    [Header("Настройки замедления")]
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float slowDuration = 5f;
    [SerializeField] private bool affectMovementSpeed = true;
    [SerializeField] private bool affectJumpHeight = false;

    private PlayerTrapEffects playerEffects;

    protected override void ActivateTrapEffect()
    {
        if (playerObject == null) return;

        // Ищем или добавляем компонент эффектов на игрока
        playerEffects = playerObject.GetComponent<PlayerTrapEffects>();
        if (playerEffects == null)
        {
            playerEffects = playerObject.AddComponent<PlayerTrapEffects>();
        }

        // Применяем замедление
        playerEffects.ApplySlowEffect(slowFactor, slowDuration, affectMovementSpeed, affectJumpHeight);

        Debug.Log($"Игрок замедлен на {slowDuration} секунд!");
    }
}
