using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Animation Settings")]
    public float openAngle = 90f;
    public float animationTime = 1f;
    public bool isOpen = false;

    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.E;

    private Transform pivotTransform; // Ссылка на родительский pivot
    private Quaternion closedRotation; // Rotation в закрытом состоянии
    private Quaternion openRotation;   // Rotation в открытом состоянии
    private float animationProgress = 0f;
    private bool isAnimating = false;

    void Start()
    {
        // Получаем ссылка на родительский объект (DoorPivot)
        pivotTransform = transform.parent;
        if (pivotTransform == null)
        {
            Debug.LogError("DoorController: Дверь должна быть дочерним объектом pivot точки!");
            return;
        }

        closedRotation = pivotTransform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (isAnimating && pivotTransform != null)
        {
            animationProgress += Time.deltaTime / animationTime;

            // Интерполируем между правильными состояниями
            if (isOpen)
            {
                // Открываем: от closedRotation к openRotation
                pivotTransform.localRotation = Quaternion.Slerp(closedRotation, openRotation, animationProgress);
            }
            else
            {
                // Закрываем: от openRotation к closedRotation
                pivotTransform.localRotation = Quaternion.Slerp(openRotation, closedRotation, animationProgress);
            }

            if (animationProgress >= 1f)
            {
                isAnimating = false;
                animationProgress = 0f;

                // Фиксируем конечное положение
                pivotTransform.localRotation = isOpen ? openRotation : closedRotation;
            }
        }
    }

    public void Interact()
    {
        isOpen = !isOpen;
        isAnimating = true;
        animationProgress = 0f;
    }

    // Для отладки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Transform targetTransform = pivotTransform != null ? pivotTransform : transform;
        Gizmos.matrix = targetTransform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}