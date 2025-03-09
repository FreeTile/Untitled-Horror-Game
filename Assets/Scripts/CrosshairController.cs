using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;       
    public Color defaultColor = new Color(1, 1, 1, 0.5f);      
    public Color interactableColor = new Color(1, 1, 1, 1f);   
    public float rayDistance = 100f;   
    public LayerMask interactableLayer; 

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Если луч попадает в объект на слое Interactable
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            crosshairImage.color = interactableColor;
        }
        else
        {
            crosshairImage.color = defaultColor;
        }
    }
}
