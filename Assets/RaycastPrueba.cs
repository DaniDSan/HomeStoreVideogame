using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPrueba : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, 0.3f)) {
            if(hit.collider.CompareTag("Wall")) {
                print("Puede ponerse");
            }
        }
        Debug.DrawRay(transform.position, -transform.forward * 0.1f, Color.red);
    }
}
