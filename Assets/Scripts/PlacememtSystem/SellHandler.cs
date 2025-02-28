using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SellHandler : MonoBehaviour {
    [SerializeField]
    Image sellIconImg;
    private void Update() {
        sellIconImg.color = PlacementSystem.Instance.isSelling ? Color.green : Color.white;
        SellItem();
    }

    public void SellItem() {
        if(!PlacementSystem.Instance.isSelling) {
            return;
        }
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit)) {
                if(LayerMask.LayerToName(hit.collider.gameObject.layer) == "PlaceableObject") {
                    // TODO: restar dinero
                    print(hit.collider.GetComponentInParent<ItemData>().shopItemSO.price);
                    Destroy(hit.collider.GetComponentInParent<ItemData>().gameObject);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PlacementSystem.Instance.isSelling = false;
        }
    }

    public void ToggleSelling() {
        PlacementSystem.Instance.isSelling = !PlacementSystem.Instance.isSelling;
    }
}
