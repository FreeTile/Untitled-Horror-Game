using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace NoteSystem
{
    public class NoteDisableManager : MonoBehaviour
    {
        [SerializeField] private NotesInteractor noteInteractorScript =  null;

        [Header("Should persist?")]
        [SerializeField] private bool persistAcrossScenes = true;

        public static NoteDisableManager instance;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                if (persistAcrossScenes)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        public void ToggleRaycast(bool disable)
        {
            noteInteractorScript.enabled = disable;
        }

        public void DisablePlayer(bool isActive)
        {
            GameManager.Instance.switchControlSystem();
            NoteUIManager.instance.HideCursor(!isActive);
            noteInteractorScript.enabled = !isActive;
        }
    }
}
