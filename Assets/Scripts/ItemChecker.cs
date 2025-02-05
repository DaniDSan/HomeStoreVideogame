using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour {
    public Material red;
    public Material green;

    public string tagToCompare;

    public bool CanPlace() {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f)) {
            if(hit.collider.CompareTag(tagToCompare)) {
                GetComponent<MeshRenderer>().material = green;
                return true;
            } else {
                GetComponent<MeshRenderer>().material = red;
                return false;
            }
        } else {
            return false;
        }
    }
}
