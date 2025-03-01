using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject shopToggle;
    Animator shopToggleAnimator;

    public GameObject objectToDisable; // Arrastra el GameObject a desactivar en el Inspector.

    public void OnPointerEnter(PointerEventData eventData) {
        if(objectToDisable != null) {
            objectToDisable.SetActive(false); // Desactivar el objeto al pasar el ratón.
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(objectToDisable != null) {
            objectToDisable.SetActive(true); // Opcional: Reactivar al salir del Canvas.
        }
    }

    void Start() {
        shopToggleAnimator = shopToggle.GetComponent<Animator>();
        shopToggle.GetComponentInChildren<Toggle>().onValueChanged.AddListener(ToggleShopAnimation);
    }

    public void ToggleShopAnimation(bool isOn) {
        shopToggleAnimator.SetBool("isOn", isOn);
    }
}