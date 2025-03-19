using UnityEngine;
using UnityEngine.UI;

public class ButtonSwapTexture : MonoBehaviour
{
    public new MeshRenderer renderer;
    public Material[] materials;
    public int currentIndex = 0;

    [SerializeField] private PuzzleScript puzzleScript;

    void Start()
    {
        if (renderer == null)
        {
            renderer = GetComponent<MeshRenderer>();
        }
    }

    public void Update()
    {
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10000, Color.magenta);

        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 10000f, LayerMask.GetMask("UI")))
        {

            if (hit.transform.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CycleAndChange();

                    if(puzzleScript != null)
                    {
                        Debug.Log("Puzzle Script is not null");
                        puzzleScript.CheckCombination();
                    }
                }
            }
        }
    }


    public void CycleAndChange()
    {
        if (renderer != null && materials.Length > 0)
        {
            currentIndex = (currentIndex + 1) % materials.Length;
            renderer.material = materials[currentIndex];
        }
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
