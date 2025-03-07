using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SellHandler : MonoBehaviour {
    [SerializeField]
    Toggle toggleSell;

    [SerializeField]
    Sprite originalImg;
    [SerializeField]
    Sprite changedImg;

    public GameObject sellIconCursor;

    private void Update() {
        // sellIconImg.color = PlacementSystem.Instance.isSelling ? Color.green : Color.white;
        SellItem();
    }

    public void SellItem() {
        if(!PlacementSystem.Instance.isSelling) {
            sellIconCursor.SetActive(false);
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            if(LayerMask.LayerToName(hit.collider.gameObject.layer) == "PlaceableObject") {
                sellIconCursor.GetComponent<Image>().sprite = changedImg;
                if(Input.GetMouseButtonDown(0)) {
                    // TODO: restar dinero
                    AudioManager.instance.PlaySFX(AudioManager.instance.placementSoundsEffects.sellSFX);
                    print(hit.collider.GetComponentInParent<ItemData>().shopItemSO.price);
                    Destroy(hit.collider.GetComponentInParent<ItemData>().gameObject);
                }
            } else {
                sellIconCursor.GetComponent<Image>().sprite = originalImg;
            }
        } else {
            sellIconCursor.GetComponent<Image>().sprite = originalImg;
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PlacementSystem.Instance.isSelling = false;
            PlacementSystem.Instance.isSelectingKitchen = false;
            PlacementSystem.Instance.isSelectingBathroom = false;
            sellIconCursor.SetActive(false);
        }
        //ChangeIcon();
    }

    public void ToggleSelling(bool isOn) {
        PlacementSystem.Instance.isSelling = !PlacementSystem.Instance.isSelling;
        //ChangeIcon();
    }

    void ChangeIcon() {
        toggleSell.GetComponent<Image>().sprite = PlacementSystem.Instance.isSelling ? changedImg : originalImg;
    }
}
