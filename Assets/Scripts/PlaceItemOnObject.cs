using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemOnObject : MonoBehaviour {
    float startYPos;
    public bool canPlace;

    private void Start() {
        GetInitialPos();
    }

    void GetInitialPos() {
        startYPos = transform.position.y;
    }

    void Update() {
        AdjustPosToItem();
    }

    void AdjustPosToItem() {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 1f);
        foreach(RaycastHit hit in hits) {
            if(hit.collider.CompareTag("Table")) {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                canPlace = true;
                return;
            } else {
                transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
                canPlace = false;
            }
        }
    }

    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
    }
}
