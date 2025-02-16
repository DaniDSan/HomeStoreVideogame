using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour {
    public bool canPlace;

    [Range(0f, 1.0f)]
    public float colliderSizeOffset = 0.8f;

    private void Update() {
        CheckCollisions();
    }

    void CheckCollisions() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        Vector3 colliderSize = boxCollider.size;
        Collider[] colliders = Physics.OverlapBox(transform.position, colliderSize * colliderSizeOffset / 2, transform.rotation);
        if(colliders.Length >= 1) {
            foreach(Collider collider in colliders) {
                if(collider.CompareTag("Floor")) {
                    canPlace = true;
                } else {
                    canPlace = false;
                    return;
                }
            }
        } else {
            canPlace = false;
        }
    }

    private void OnDrawGizmos() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 colliderSize = boxCollider.size;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, colliderSize * (colliderSizeOffset + 0.2f));
    }
}


