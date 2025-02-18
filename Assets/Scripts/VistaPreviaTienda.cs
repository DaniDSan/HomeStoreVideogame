using TMPro;
using UnityEngine;

public class VistaPreviaTienda : MonoBehaviour {
    public Transform posicionVistaPrevia; // Punto frente a la cámara
    private GameObject objetoActual; // Objeto actualmente en la vista previa

    public float velocidadRotacion = 20f;

    // Método para mostrar un nuevo prefab
    public void MostrarObjeto(GameObject prefab) {
        // Elimina el objeto actual si ya hay uno
        if(objetoActual != null) {
            Destroy(objetoActual);
        }

        // Instanciar el nuevo objeto frente a la cámara
        objetoActual = Instantiate(prefab, posicionVistaPrevia.position, Quaternion.identity);
        objetoActual.transform.SetParent(posicionVistaPrevia); // Opcional: para mantener jerarquía limpia

        // Ajustar escala y rotación si es necesario
        objetoActual.transform.localRotation = Quaternion.Euler(0, 180, 0); // Por ejemplo, mirar hacia la cámara
    }

    private void Update() {
        if(objetoActual != null) {
            objetoActual.transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
        }
    }
}