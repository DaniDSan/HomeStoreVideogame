using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public GameObject placeHolderItem;
    public GameObject placeableItem;
    public GameObject visualRef;
    GameObject tempItem;
    Camera cam;

    bool isShowing = false;

    public Material red;
    public Material green;

    public Color colorRed;
    public Color colorGreen;

    [Range(0f, 1.0f)]
    public float colliderSizeOffset = 0.8f;

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
            visualRef.transform.Rotate(0, 0, 90);
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
        BoxCollider boxCollider = placeableItem.GetComponentInChildren<BoxCollider>();
        Vector3 colliderSize = boxCollider.size;
        visualRef.transform.localScale = new Vector3(visualRef.transform.localScale.x, colliderSize.z, visualRef.transform.localScale.z);
        Collider[] colliders = Physics.OverlapBox(tempItem.transform.position, colliderSize * colliderSizeOffset / 2, tempItem.transform.rotation);
        if(colliders.Length == 1) {
            foreach(Collider collider in colliders) {
                if(collider.CompareTag("Floor")) {
                    placeHolderItem.GetComponentInChildren<MeshRenderer>().material = green;
                    tempItem.GetComponentInChildren<MeshRenderer>().material.color = colorGreen; // Verde con transparencia
                    return true;
                }
            }
        }
        placeHolderItem.GetComponentInChildren<MeshRenderer>().material = red;
        tempItem.GetComponentInChildren<MeshRenderer>().material.color = colorRed; // Rojo con trnasparencia
        return false;
    }

    private void OnDrawGizmos() {
        BoxCollider boxCollider = placeableItem.GetComponentInChildren<BoxCollider>();
        Vector3 colliderSize = boxCollider.size;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(tempItem.transform.position, tempItem.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, colliderSize * colliderSizeOffset);
    }
}

