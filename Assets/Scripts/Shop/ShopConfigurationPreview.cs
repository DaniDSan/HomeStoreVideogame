using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopConfigurationPreview : MonoBehaviour {
    public Transform[] previewPositions; // Posicion para que se vea el objeto
    public List<GameObject> actaulPrefabs; // Objeto que mostraremos
    GameObject[] tempObjects;

    int nextItem;
    int prevItem;
    int nextTemplate = 0;
    int prevTemplate = 0;

    public float rotationVelocity = 40f;

    private void Awake() {
        tempObjects = new GameObject[previewPositions.Length];

        // El indice del siguiente item a msotrar sera el del final del numero de camaras que tenemos
        nextItem = previewPositions.Length - 1;
    }

    private void Update() {
        foreach(Transform previewPosition in previewPositions) {
            previewPosition.transform.Rotate(Vector3.up, rotationVelocity * Time.deltaTime);
        }
    }

    public void ShowPreview(GameObject prefab, int positionIndex) {
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

    public void ScrollRight() {
        // Si el anterior template es menor a 0 significa que no hay templates anteriores y salimos del metodo
        if(prevTemplate <= 0) {
            return;
        }
        // Hacemos un calculo para sacar el indice del prefab que tenemos que mostar como anterior
        prevItem = nextItem - previewPositions.Length;
        prevItem = (prevItem + actaulPrefabs.Count) % actaulPrefabs.Count;

        // Si el indice del item que tenemos que msotrar como siguiente tiene un valor por debajo de 0 le reiniciamos el valor al final de la lista, para que de vueltas ciclicas
        if(--nextItem < 0) {
            nextItem = actaulPrefabs.Count - 1;
        }
        nextTemplate--;
        ShowPreview(actaulPrefabs[prevItem], --prevTemplate % previewPositions.Length);
    }

    public void ScrollLeft() {
        // Hacemos que de vueltas de manera ciclica al array de items
        nextItem = (nextItem + 1) % actaulPrefabs.Count;
        // Indicamos cual es el item anterior que tiene que mostrar, aumentando su indice, porque cada vez que lancemos esto el item debera ser un anterior mas
        prevItem++;
        prevTemplate++;

        ShowPreview(actaulPrefabs[nextItem], nextTemplate++ % previewPositions.Length);
    }

    public void ResetIndex() {
        nextItem = previewPositions.Length - 1;
        nextTemplate = 0;
        prevItem = 0;
        prevTemplate = 0;
    }
}