using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public ItemChecker ic;
    public GameObject placeHolderItem;
    public GameObject placeableItem;
    Camera cam;

    // Start is called before the first frame update
    void Start() {
        GetRefs();
    }

    // Update is called once per frame
    void Update() {
        PlaceObject();
    }

    void GetRefs() {
        cam = Camera.main;
    }

    void PlaceObject() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            placeHolderItem.transform.position = new Vector3(Mathf.Round(hit.point.x) + 0.5f, 0.5f, Mathf.Round(hit.point.z) + 0.5f);
            if(ic.CanPlace() && Input.GetMouseButton(0)) {
                Instantiate(placeableItem, placeHolderItem.transform.position, Quaternion.identity);
            }
        }
    }
}
