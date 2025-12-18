using UnityEngine;

// Альтернатива, если у вас есть доступ к полям контроллера
public class PlayerTrapEffects : MonoBehaviour
{
    // Публичные поля для связи с контроллером
    [Header("Ссылки на контроллер")]
    [SerializeField] private MonoBehaviour playerController;

    // Имена полей для изменения (через Reflection)
    [Header("Имена полей контроллера")]
    [SerializeField] private string moveSpeedFieldName = "moveSpeed";

    private float originalMoveSpeed;
    private Coroutine currentEffectCoroutine;

    void Start()
    {
        if (playerController == null)
        {
            playerController = GetComponent<FirstPersonController>();
        }
    }

    public void ApplySlowEffect(float slowFactor, float duration, bool affectSpeed, bool affectJump)
    {
        if (currentEffectCoroutine != null)
        {
            StopCoroutine(currentEffectCoroutine);
        }

        currentEffectCoroutine = StartCoroutine(SlowEffectCoroutine(slowFactor, duration, affectSpeed, affectJump));
    }

    private System.Collections.IEnumerator SlowEffectCoroutine(float slowFactor, float duration, bool affectSpeed, bool affectJump)
    {
        // Сохраняем оригинальные значения
        SaveOriginalValues(affectSpeed, affectJump);

        // Применяем замедление
        ApplySlowValues(slowFactor, affectSpeed, affectJump);

        // Ждем
        yield return new WaitForSeconds(duration);

        // Восстанавливаем
        RestoreOriginalValues(affectSpeed, affectJump);

        currentEffectCoroutine = null;
    }

    private void SaveOriginalValues(bool saveSpeed, bool saveJump)
    {
        if (saveSpeed)
        {
            var field = playerController.GetType().GetField(moveSpeedFieldName);
            if (field != null)
            {
                originalMoveSpeed = (float)field.GetValue(playerController);
            }
        }
    }

    private void ApplySlowValues(float slowFactor, bool affectSpeed, bool affectJump)
    {
        if (affectSpeed)
        {
            var field = playerController.GetType().GetField(moveSpeedFieldName);
            if (field != null)
            {
                field.SetValue(playerController, originalMoveSpeed * slowFactor);
            }
        }
    }

    private void RestoreOriginalValues(bool restoreSpeed, bool restoreJump)
    {
        if (restoreSpeed)
        {
            var field = playerController.GetType().GetField(moveSpeedFieldName);
            if (field != null)
            {
                field.SetValue(playerController, originalMoveSpeed);
            }
        }
    }
}