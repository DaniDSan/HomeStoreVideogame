using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopConfigurationPreview : MonoBehaviour {
    public Transform[] previewPositions; // Posicion para que se vea el objeto
    public List<GameObject> actaulPrefabs; // Objeto que mostraremos

    public float rotationVelocity = 40f;


    private void Start() {
        for(int i = 0; i < actaulPrefabs.Count; i++) {
            ShowPreview(actaulPrefabs[i], i);
        }
    }

    public void ShowPreview(GameObject prefab, int i) {
        GameObject tempObject = Instantiate(prefab, previewPositions[i].position, Quaternion.identity);
        tempObject.transform.SetParent(previewPositions[i]);

        tempObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    private void Update() {
        foreach(Transform previewPosition in previewPositions) {
            previewPosition.transform.Rotate(Vector3.up, rotationVelocity * Time.deltaTime);
        }
    }
}