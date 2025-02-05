using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour {
    public Material red;
    public Material green;

    public LayerMask detectingLayers;

    public bool CanPlace() {
        if(Physics.Raycast(transform.position + new Vector3(0,3,0), Vector3.down, out RaycastHit hit, 10f,detectingLayers)) {
            if(hit.collider.CompareTag("Floor")) {
                GetComponent<MeshRenderer>().material = green;
                return true;
            } else {
                GetComponent<MeshRenderer>().material = red;
                return false;
            }
        } else {
            GetComponent<MeshRenderer>().material = red;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 3, 0), Vector3.down * 10f, Color.blue);
    }
}
