using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChuckMenues : MonoBehaviour
{
    [Header("Настройки изображений")]
    public RawImage targetImage;
    public Texture2D defaultSprite;
    public Texture2D[] switchSprites;

    [Header("Настройки анимации")]
    public float intervalBetweenAnimations = 30f;
    public float switchInterval = 0.1f;
    public float animationDuration = 2f;

    private bool isAnimating = false;

    void Start()
    {
        // Устанавливаем стандартное изображение
        if (targetImage != null && defaultSprite != null)
        {
            targetImage.texture = defaultSprite;
        }

        // Запускаем периодическую анимацию
        StartCoroutine(PeriodicAnimationRoutine());
    }

    private IEnumerator PeriodicAnimationRoutine()
    {
        while (true)
        {
            // Ждём 30 секунд перед следующей анимацией
            yield return new WaitForSeconds(intervalBetweenAnimations);

            // Запускаем анимацию смены изображений
            yield return StartCoroutine(SwitchImagesRoutine());
        }
    }

    private IEnumerator SwitchImagesRoutine()
    {
        isAnimating = true;
        float endTime = Time.time + animationDuration;

        while (isAnimating && Time.time < endTime)
        {
            if (switchSprites.Length > 0 && targetImage != null)
            {
                int randomIndex = Random.Range(0, switchSprites.Length);
                targetImage.texture = switchSprites[randomIndex];
            }
            yield return new WaitForSeconds(switchInterval);
        }

        if (targetImage != null && defaultSprite != null)
        {
            targetImage.texture = defaultSprite;
        }

        isAnimating = false;
    }

    //public void ForceStartAnimation()
    //{
    //    if (!isAnimating)
    //    {
    //        StopAllCoroutines();
    //        StartCoroutine(SwitchImagesRoutine());
    //        StartCoroutine(PeriodicAnimationRoutine());
    //    }
    //}

    public void StopAllAnimations()
    {
        StopAllCoroutines();
        isAnimating = false;
        if (targetImage != null && defaultSprite != null)
        {
            targetImage.texture = defaultSprite;
        }
    }
}
