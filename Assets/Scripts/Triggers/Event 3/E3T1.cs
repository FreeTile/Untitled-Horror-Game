using System.Collections;
using UnityEngine;
using DG.Tweening;

public class E3T1 : EventHandler
{
    public float explosionForce = 20f;
    public float explosionRadius = 5f;
    public float upwardModifier = 1f;

    public Transform explosionCenter;
    public GameObject BathroomDoor;
    public Door door;

    public GameObject plateDroppedSound;
    public GameObject[] kitchenProps;

    // Новые поля для эффектов
    public ParticleSystem bloodParticles;          // Эффект крови (ParticleSystem)
    public Material bathroomWallMaterial;            // Материал стен туалета для перекраски
    public Color targetRedColor = Color.red;         // Желаемый красный цвет
    public float colorTransitionDuration = 1.0f;       // Время перехода цвета
    public float effectDuration = 20.0f;               // Длительность эффекта

    // Новые поля для дополнительного материала
    public Material additionalMaterial;            // Дополнительный материал, который будет меняться
    public Color targetAdditionalColor = Color.blue; // Целевой цвет для дополнительного материала (пример: синий)
    private Color additionalOriginalColor;           // Исходный цвет дополнительного материала

    private Color originalColor;                     // Исходный цвет материала стен

    public override IEnumerator Event()
    {
        // Сохраняем исходный цвет материалов
        if (bathroomWallMaterial != null)
            originalColor = bathroomWallMaterial.color;

        if (additionalMaterial != null)
            additionalOriginalColor = additionalMaterial.color;

        // Работа с дверью
        door = BathroomDoor.GetComponent<Door>();
        door.ProcessMove();
        BathroomDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
        door.isLocked = true;

        // Запуск страшных звуков (если необходимо)
        //if (scaryAudioSource != null)
        //{
        //    scaryAudioSource.Play();
        //}

        // Изменение цвета стен на кроваво-красный
        if (bathroomWallMaterial != null)
        {
            bathroomWallMaterial.DOColor(targetRedColor, colorTransitionDuration);
        }

        // Изменение цвета дополнительного материала
        if (additionalMaterial != null)
        {
            additionalMaterial.DOColor(targetAdditionalColor, colorTransitionDuration);
        }

        // Включаем эффекты крови
        if (bloodParticles != null)
        {
            bloodParticles.Play();
        }

        // Эффект длится effectDuration секунд
        yield return new WaitForSeconds(effectDuration);

        // Возвращаем исходный цвет стен
        if (bathroomWallMaterial != null)
        {
            bathroomWallMaterial.DOColor(originalColor, colorTransitionDuration);
        }

        // Возвращаем исходный цвет дополнительного материала
        if (additionalMaterial != null)
        {
            additionalMaterial.DOColor(additionalOriginalColor, colorTransitionDuration);
        }

        // Останавливаем эффекты крови
        if (bloodParticles != null)
        {
            bloodParticles.Stop();
        }

        // Останавливаем страшные звуки (при необходимости)
        //if (scaryAudioSource != null)
        //{
        //    scaryAudioSource.Stop();
        //}

        // Действия в кухне (например, взрыв предметов)
        foreach (GameObject prop in kitchenProps)
        {
            Rigidbody rb = prop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionCenter.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }
        }

        // Выключаем звук падения тарелки
        plateDroppedSound.SetActive(false);

        door.isLocked = false;
        yield return base.Event();
    }
}
