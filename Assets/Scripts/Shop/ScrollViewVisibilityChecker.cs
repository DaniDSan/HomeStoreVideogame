using UnityEngine;

public class PanelVisibilityChecker : MonoBehaviour {
    public RectTransform scrollViewViewport; // Asigna el RectTransform del �rea visible (Viewport)
    public RectTransform scrollViewContent;  // Asigna el contenido del ScrollView

    void Start() {
        // Opcionalmente, puedes inicializar aqu� si es necesario
        CheckPanelVisibility();
    }

    void Update() {
        // Comprobamos si el panel est� visible dentro del ScrollView
        CheckPanelVisibility();
    }

    void CheckPanelVisibility() {
        if(IsPanelVisibleInScrollView(scrollViewContent, GetComponent<RectTransform>(), scrollViewViewport)) {
            Debug.Log($"{gameObject.name} est� visible dentro del ScrollView");
        } else {
            Debug.Log($"{gameObject.name} est� fuera de la vista del ScrollView");
        }
    }

    bool IsPanelVisibleInScrollView(RectTransform content, RectTransform panel, RectTransform viewport) {
        // Obtener las esquinas del viewport (�rea visible)
        Vector3[] viewportCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);

        // L�mites del viewport
        float minX = viewportCorners[0].x;
        float maxX = viewportCorners[2].x;
        float minY = viewportCorners[0].y;
        float maxY = viewportCorners[2].y;

        // Obtener las esquinas del panel
        Vector3[] panelCorners = new Vector3[4];
        panel.GetWorldCorners(panelCorners);

        // Verificar si alguna esquina del panel est� dentro de los l�mites del ScrollView
        foreach(Vector3 corner in panelCorners) {
            if(corner.x >= minX && corner.x <= maxX && corner.y >= minY && corner.y <= maxY) {
                return true; // Al menos una esquina est� visible
            }
        }

        return false; // Si todas las esquinas est�n fuera de la vista
    }
}