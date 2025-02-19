using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopConfigurationPreview : MonoBehaviour {
    public Transform[] previewPositions; // Posicion para que se vea el objeto
    public List<GameObject> actaulPrefabs; // Objeto que mostraremos
    GameObject[] tempObjects;

    int actualItem;
    int prevItem;
    int nextTemplate = 0;
    int prevTemplate = 0;

    public float rotationVelocity = 40f;

    private void Start() {
        tempObjects = new GameObject[previewPositions.Length];
        for(int i = 0; i < actaulPrefabs.Count; i++) {
            ShowPreview(actaulPrefabs[i], i);
        }

        actualItem = previewPositions.Length - 1;
    }

    private void Update() {
        // Rotar todos los objetos en sus posiciones
        foreach(Transform previewPosition in previewPositions) {
            previewPosition.transform.Rotate(Vector3.up, rotationVelocity * Time.deltaTime);
        }

        // Desplazarse a la derecha al presionar "E"
        if(Input.GetKeyDown(KeyCode.E)) {
            ScrollRight();
        }

        // Desplazarse a la izquierda al presionar "Q"
        if(Input.GetKeyDown(KeyCode.Q)) {
            ScrollLeft();
        }
    }

    void ShowPreview(GameObject prefab, int positionIndex) {
        if(positionIndex > previewPositions.Length - 1) {
            return;
        }
        // Destruir cualquier objeto existente en la posición
        if(tempObjects[positionIndex] != null) {
            Destroy(tempObjects[positionIndex]);
        }

        // Instanciar el nuevo objeto en la posición correspondiente
        tempObjects[positionIndex] = Instantiate(prefab, previewPositions[positionIndex].position, Quaternion.identity);
        tempObjects[positionIndex].transform.SetParent(previewPositions[positionIndex]);
        tempObjects[positionIndex].transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void ScrollRight() {
        if(prevTemplate <= 0) {
            return;
        }
        // Calcular el nuevo índice para el último objeto
        prevItem = actualItem - previewPositions.Length;
        print(prevItem);
        prevItem = (prevItem + actaulPrefabs.Count) % actaulPrefabs.Count;
        actualItem--;
        nextTemplate--;
        ShowPreview(actaulPrefabs[prevItem], --prevTemplate);
    }

    void ScrollLeft() {
        // Calcular el nuevo índice para el primer objeto
        actualItem = (actualItem + 1) % actaulPrefabs.Count;
        prevItem++;
        prevTemplate++;
        // Mostrar el nuevo objeto en la primera posición
        ShowPreview(actaulPrefabs[actualItem], nextTemplate++);
    }
}