using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public GameObject placeHolderItem;
    public GameObject placeableItem;
    GameObject tempItem;
    Camera cam;

    bool isShowing = false;

    public Material red;
    public Material green;

    [Range(0f, 1.0f)]
    public float colliderSize = 0.8f;

    // Start is called before the first frame update
    void Start() {
        GetRefs();
    }

    // Update is called once per frame
    void Update() {
        PlaceHolderControl();
    }

    void GetRefs() {
        cam = Camera.main;
    }

    void PlaceHolderControl() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            placeHolderItem.SetActive(true);
            placeHolderItem.transform.position = new Vector3(Mathf.Round(hit.point.x), 0.01f, Mathf.Round(hit.point.z));
            ItemPrevisualizer();
            RotateItem();
            if(CanPlace()) {
                SetItem();
            }
        } else {
            placeHolderItem.SetActive(false);
        }
    }

    void SetItem() {
        if(Input.GetMouseButtonDown(0)) {
            Instantiate(placeableItem,
                new Vector3(placeHolderItem.transform.position.x, placeableItem.transform.position.y, placeHolderItem.transform.position.z),  // La altura sera la el objeto que vamos a poner
                tempItem.transform.rotation);
        }
    }

    void RotateItem() {
        if(Input.GetMouseButtonDown(1)) {
            tempItem.transform.Rotate(0, 90, 0);
        }
    }

    // Mostramos el objeto que vamos a colocar y le cambiamos el color segun se pueda colocar o no
    void ItemPrevisualizer() {
        if(isShowing == false) {
            isShowing = true;
            tempItem = Instantiate(placeableItem, placeHolderItem.transform.position, placeableItem.transform.rotation, placeHolderItem.transform);
            tempItem.GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }

    bool CanPlace() {
        Collider[] colliders = Physics.OverlapBox(placeHolderItem.transform.position, Vector3.one * colliderSize / 2);
        if(colliders.Length == 1) {
            foreach(Collider collider in colliders) {
                if(collider.CompareTag("Floor")) {
                    placeHolderItem.GetComponentInChildren<MeshRenderer>().material = green;
                    return true;
                }
            }
        }
        placeHolderItem.GetComponentInChildren<MeshRenderer>().material = red;
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(placeHolderItem.transform.position, Vector3.one * colliderSize);
    }
}
