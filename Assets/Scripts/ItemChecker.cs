using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public Material red;
    public Material green;

    public LayerMask floorLayer;

    public bool CanPlace() {
        if(Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity, floorLayer)) {
            GetComponent<MeshRenderer>().material = green;
            return true;
        } else {
            GetComponent<MeshRenderer>().material = red;
            return false;
        }
    }
}
