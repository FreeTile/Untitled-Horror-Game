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

    // Свет в туалете
    public Light bathroomLight;
    // Целевой цвет - красный
    public Color targetLightColor = Color.red;
    // Время, за которое происходит переход цвета
    public float colorTransitionDuration = 1.0f;
    // Длительность эффекта (сколько времени будет свет красным)
    public float effectDuration = 20.0f;

    // Сохранение исходного цвета света
    private Color originalLightColor;

    public GameObject plateDroppedSound;
    public GameObject[] kitchenProps;
    public ParticleSystem bloodParticles;

    public override IEnumerator Event()
    {
        // Сохраняем исходный цвет света
        if (bathroomLight != null)
        {
            originalLightColor = bathroomLight.color;
        }

        // Обработка двери
        door = BathroomDoor.GetComponent<Door>();
        door.ProcessMove();
        BathroomDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
        door.isLocked = true;

        // Изменяем цвет света в туалете на красный
        if (bathroomLight != null)
        {
            bathroomLight.DOColor(targetLightColor, colorTransitionDuration);
        }

        // Запускаем эффекты, если есть (например, эффекты крови)
        if (bloodParticles != null)
        {
            bloodParticles.Play();
        }

        // Ждем, пока эффект длится effectDuration секунд
        yield return new WaitForSeconds(effectDuration);

        // Возвращаем исходный цвет света
        if (bathroomLight != null)
        {
            bathroomLight.DOColor(originalLightColor, colorTransitionDuration);
        }

        // Останавливаем эффекты крови
        if (bloodParticles != null)
        {
            bloodParticles.Stop();
        }

        // Действия на кухне (например, взрыв предметов)
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
