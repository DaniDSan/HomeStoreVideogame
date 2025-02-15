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
                new Vector3(tempItem.transform.position.x, placeableItem.transform.position.y, tempItem.transform.position.z),  // La altura sera la el objeto que vamos a poner
                tempItem.transform.rotation);
        }
    }

    void RotateItem() {
        if(Input.GetMouseButtonDown(1)) {
            placeHolderItem.transform.Rotate(0, 90, 0);
        }
    }

    // Mostramos el objeto que vamos a colocar y le cambiamos el color segun se pueda colocar o no
    void ItemPrevisualizer() {
        if(isShowing == false) {
            isShowing = true;
            tempItem = Instantiate(placeableItem, new Vector3(placeHolderItem.transform.position.x, placeableItem.transform.position.y, placeHolderItem.transform.position.z), placeableItem.transform.rotation, placeHolderItem.transform);
            BoxCollider[] boxColliders = tempItem.GetComponentsInChildren<BoxCollider>();
            if(boxColliders[0].size != Vector3.one) {
                AdjustPositionBySize(boxColliders[0]);
            } else {
                tempItem.transform.localPosition = Vector3.zero;
                visualRef.transform.localPosition = Vector3.zero;
            }
            visualRef.transform.localScale = new Vector3(boxColliders[0].size.x, boxColliders[0].size.z, 1f);
            foreach(BoxCollider boxCollider in boxColliders) { boxCollider.enabled = false; }
        }
    }

    bool CanPlace() {
        bool canPlace = false;
        ItemChecker[] itemCheckers = GetComponentsInChildren<ItemChecker>();
        for(int i = 0; i < itemCheckers.Length; i++) {
            if(!itemCheckers[i].canPlace) {
                canPlace = false; break;
            } else {
                canPlace = true;
            }
        }
        if(canPlace) {
            placeHolderItem.GetComponentInChildren<MeshRenderer>().material = green;
            tempItem.GetComponentInChildren<MeshRenderer>().material.color = colorGreen; // Verde con transparencia
            return true;
        }
        placeHolderItem.GetComponentInChildren<MeshRenderer>().material = red;
        tempItem.GetComponentInChildren<MeshRenderer>().material.color = colorRed; // Rojo con trnasparencia
        return false;
    }

    void AdjustPositionBySize(BoxCollider boxCollider) {
        Vector3 tempVect = Vector3.zero;
        if(boxCollider.size.x % 2 == 0) {
            tempVect.x = boxCollider.size.x / 2 - 0.5f;
        }
        if(boxCollider.size.z % 2 == 0) {
            tempVect.z = boxCollider.size.z / 2 - 0.5f;
        }

        tempItem.transform.localPosition = tempVect;
        visualRef.transform.localPosition = tempVect;
    }
}

