using UnityEngine;

public class GameOverScreamer : MonoBehaviour
{
    // Ссылка на объект монстра, который хотим включить
    public GameObject monster;

    // Сюда сохраним ссылку на Animator
    private Animator monsterAnimator;

    void Start()
    {
        if (monster != null)
        {
            // Берём компонент Animator на монстре
            monsterAnimator = monster.GetComponent<Animator>();
            // На всякий случай отключаем монстра (если он неактивен в сцене)
            monster.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Monster не задан в инспекторе!");
        }
    }

    // Вызывается, когда объект с тегом "Player" заходит в триггер
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("COllided");

        if (other.CompareTag("Player"))
        {
            Debug.Log("COllided");
            // Активируем монстра
            monster.SetActive(true);

            // Способ 1: Прямое воспроизведение анимации по имени состояния в Animator
            // Убедитесь, что анимационное состояние в контроллере действительно называется "Attack"
            monsterAnimator.Play("Attack");

            // Или способ 2: если в Animator Controller есть параметр-триггер
            // monsterAnimator.SetTrigger("AttackTrigger");
        }
    }
}
