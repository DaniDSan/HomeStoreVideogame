using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour {
    public bool canPlace;

    [Range(0f, 1.0f)]
    public float colliderSizeOffset = 0.8f;

    float colliderReducedSize = 0.001f;

    void Start() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 reduction = Vector3.one * colliderReducedSize;

        if(!boxCollider.CompareTag("FloorBase")) {
            if(boxCollider.size.x == 0) reduction.x = 0f;
            if(boxCollider.size.y == 0) reduction.y = 0f;
            if(boxCollider.size.z == 0) reduction.z = 0f;

            boxCollider.size -= reduction;
        }
    }

    private void Update() {
        if(CompareTag("WallObject")) {
            CheckWalls();
        } else {
            CheckCollisions();
        }
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

    void CheckWalls() {
        Debug.DrawRay(transform.position, -transform.forward * 0.1f, Color.blue);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.forward, 0.1f);

        if(hits.Length > 0) {
            foreach(RaycastHit hit in hits) {
                if(hit.collider.CompareTag("Wall")) {
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
