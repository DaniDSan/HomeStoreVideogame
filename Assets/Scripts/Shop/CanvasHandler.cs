using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject shopToggle;
    Animator shopToggleAnimator;

    public GameObject[] objectsToDisable; // Arrastra el GameObject a desactivar en el Inspector.

    [SerializeField]
    Sprite originalImg;
    [SerializeField]
    Sprite changedImg;

    public void OnPointerEnter(PointerEventData eventData) {
        if(objectsToDisable != null) {
            foreach(GameObject objectToDisable in objectsToDisable) {
                objectToDisable.SetActive(false); // Desactivar el objeto al pasar el ratón.
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(objectsToDisable != null) {
            if(PlacementSystem.Instance.isSelling) {
                objectsToDisable[0].SetActive(true);
            } else {
                objectsToDisable[0].SetActive(false);
            }
            foreach(GameObject objectToDisable in objectsToDisable) {
                objectToDisable.SetActive(true); // Opcional: Reactivar al salir del Canvas.
            }
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