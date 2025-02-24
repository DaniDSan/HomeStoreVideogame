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
        Vector3 adjustedSize = boxCollider.size * colliderSizeOffset;

        Collider[] colliders = Physics.OverlapBox(transform.position, adjustedSize / 2, transform.rotation);
        canPlace = false;

        if(colliders.Length > 0) {
            bool foundValidSurface = false;

            foreach(Collider collider in colliders) {
                if(CompareTag("UpperFloor")) {
                    if(collider.CompareTag("UpperFloor")) {
                        canPlace = false;
                        return;
                    } else {
                        foundValidSurface = true;
                    }
                } else if(CompareTag("Carpet")) {
                    if(collider.CompareTag("Carpet")) {
                        canPlace = false;
                        return;
                    } else {
                        foundValidSurface = true;
                    }
                } else if(collider.CompareTag("FloorBase") || collider.CompareTag("Floor") || collider.CompareTag("Carpet") || collider.CompareTag("UpperFloor")) {
                    foundValidSurface = true;
                } else {
                    canPlace = false;
                    return;
                }
            }
            canPlace = foundValidSurface;
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
