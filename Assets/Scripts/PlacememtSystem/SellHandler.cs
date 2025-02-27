using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SellHandler : MonoBehaviour {
    private void Update() {
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
                    Destroy(hit.collider.GetComponentInParent<ItemData>().gameObject);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PlacementSystem.Instance.isSelling = false;
            gameObject.SetActive(false);
        }
    }

    public void ToggleSelling() {
        PlacementSystem.Instance.isSelling = !PlacementSystem.Instance.isSelling;
    }
}
