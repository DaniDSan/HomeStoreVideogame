using TMPro;
using UnityEngine;

public class VistaPreviaTienda : MonoBehaviour {
    public Transform posicionVistaPrevia; // Punto frente a la c�mara
    private GameObject objetoActual; // Objeto actualmente en la vista previa

    public float velocidadRotacion = 20f;

    // M�todo para mostrar un nuevo prefab
    public void MostrarObjeto(GameObject prefab) {
        // Elimina el objeto actual si ya hay uno
        if(objetoActual != null) {
            Destroy(objetoActual);
        }

        // Instanciar el nuevo objeto frente a la c�mara
        objetoActual = Instantiate(prefab, posicionVistaPrevia.position, Quaternion.identity);
        objetoActual.transform.SetParent(posicionVistaPrevia); // Opcional: para mantener jerarqu�a limpia

        // Ajustar escala y rotaci�n si es necesario
        objetoActual.transform.localRotation = Quaternion.Euler(0, 180, 0); // Por ejemplo, mirar hacia la c�mara
    }

    private void Update() {
        if(objetoActual != null) {
            objetoActual.transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
        }
    }
}