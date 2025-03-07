using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour {
    private GameObject lastHighlightedObject; // Último objeto resaltado
    private Outline lastOutline; // Último componente Outline

    public static OutlineHandler Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void OutlineCheck() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {
            if(LayerMask.LayerToName(hit.collider.gameObject.layer) == "PlaceableObject" || LayerMask.LayerToName(hit.collider.gameObject.layer) == "DefaultObject") {
                GameObject hitObject = hit.collider.gameObject;
                GameObject parentObject = hitObject.GetComponentInParent<ItemData>()?.gameObject;

                if(parentObject != null) {
                    if(parentObject != lastHighlightedObject) {
                        ResetOutline(); // Desactiva el anterior antes de activar el nuevo

                        Outline outline;
                        if(parentObject.TryGetComponent(out outline)) {
                            outline.enabled = true; // Activa el nuevo Outline
                            lastOutline = outline;
                            lastHighlightedObject = parentObject;
                        }
                    }
                }
            } else {
                ResetOutline();
            }
        } else {
            ResetOutline(); // Si no hay colisión con ningún objeto, desactiva el último resaltado
        }
    }

    public void ResetOutline() {
        if(lastOutline != null) {
            lastOutline.enabled = false;
            lastOutline = null;
            lastHighlightedObject = null;
        }
    }
}
