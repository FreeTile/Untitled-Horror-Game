using UnityEngine;

public class BloodEffectValues : MonoBehaviour
{
    public ParticleSystem bloodParticleSystem;

    void Start()
    {
        if (bloodParticleSystem == null)
            bloodParticleSystem = GetComponent<ParticleSystem>();

        // Основной модуль: задаёт общие параметры частиц
        var main = bloodParticleSystem.main;
        main.duration = 1.0f;                         // Длительность эмиссии (не влияет на одноразовый взрыв)
        main.loop = false;                            // Эффект однократный
        main.startLifetime = new ParticleSystem.MinMaxCurve(1.0f, 2.0f); // Время жизни частиц (от 1 до 2 секунд)
        main.startSpeed = new ParticleSystem.MinMaxCurve(2.0f, 4.0f);     // Скорость частиц (от 2 до 4 единиц)
        main.startSize = new ParticleSystem.MinMaxCurve(0.2f, 0.5f);      // Размер частиц
        main.startColor = new ParticleSystem.MinMaxGradient(Color.red, new Color(0.8f, 0, 0)); // Начальный цвет (от ярко-красного до темно-красного)
        main.gravityModifier = 0.8f;                    // Гравитация для имитации падения капель

        // Эмиссия: настройка взрывной эмиссии частиц
        var emission = bloodParticleSystem.emission;
        emission.rateOverTime = 0;                    // Постоянная эмиссия отключена
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 20, 30)    // В момент 0 выпускается от 20 до 30 частиц
        });

        // Модуль Shape: определяет форму и направление эмиссии
        var shape = bloodParticleSystem.shape;
        shape.angle = 25f;                          // Угол рассеивания частиц (25°)
        shape.radius = 0.2f;                          // Радиус источника эмиссии

        // Color over Lifetime: постепенное изменение цвета и прозрачности
        var colOverLifetime = bloodParticleSystem.colorOverLifetime;
        colOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.red, 0.0f),
                new GradientColorKey(new Color(0.5f, 0, 0), 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),       // Полностью непрозрачный в начале
                new GradientAlphaKey(0.0f, 1.0f)        // Полностью прозрачный к концу жизни частицы
            }
        );
        colOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
