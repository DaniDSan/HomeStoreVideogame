using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public Material red;
    public Material green;

    public LayerMask detectingLayer;

    public bool CanPlace() {
        if(Physics.Raycast(transform.position + new Vector3(0f, 6f, 0f), Vector3.down, 10f, detectingLayer)) {
            GetComponent<MeshRenderer>().material = green;
            return true;
        } else {
            GetComponent<MeshRenderer>().material = red;
            return false;
        }
    }

    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position + new Vector3(0f, 6f, 0f), Vector3.down * 10f, Color.red);
    }
}
