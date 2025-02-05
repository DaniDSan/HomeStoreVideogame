using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject placeableItem;
    Camera cam;
    public Material red;
    public Material green;
    // Start is called before the first frame update
    void Start()
    {
        GetRefs();
    }

    // Update is called once per frame
    void Update()
    {
        PlaceObject();
    }

    void GetRefs() {
        cam = Camera.main;
    }

    void PlaceObject() {
        CanPlace();
    }

    bool CanPlace() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            placeableItem.transform.position = new Vector3(Mathf.Round(hit.point.x), 0.1f, Mathf.Round(hit.point.z));
            placeableItem.GetComponent<MeshRenderer>().materials[0] = red;
        } else {
            placeableItem.GetComponent<MeshRenderer>().materials[0] = green;
        }
        return true;
    }
}
