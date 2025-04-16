using UnityEngine;

namespace NoteSystem
{
    [RequireComponent(typeof(Camera))]
    public class NotesInteractor : MonoBehaviour
    {
        [Header("Raycast Features")]
        [SerializeField] private float rayLength = 5;
        private NoteInteractable viewableNote;
        private Camera _camera;

        void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void LateUpdate()
        {

            Vector3 origin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _camera.nearClipPlane));
            Ray ray = new Ray(origin, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                var noteItem = hit.collider.GetComponent<NoteInteractable>();
                if (noteItem != null)
                {
                    viewableNote = noteItem;
                    HighlightCrosshair(true);
                }
                else
                {
                    ClearExaminable();
                }
            }
            else
            {
                ClearExaminable();
            }

            if (viewableNote != null)
            {
                if (Input.GetKeyDown(NoteInputManager.instance.interactKey))
                {
                    viewableNote.ShowNote();
                }
            }
        }

        private void ClearExaminable()
        {
            if (viewableNote != null)
            {
                HighlightCrosshair(false);
                viewableNote = null;
            }
        }

        void HighlightCrosshair(bool on)
        {
            NoteUIManager.instance.HighlightCrosshair(on);
        }
    }
}
