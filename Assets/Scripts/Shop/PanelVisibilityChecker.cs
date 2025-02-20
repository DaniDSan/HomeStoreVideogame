using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelVisibilityChecker : MonoBehaviour {
    public RectTransform scrollView;
    ShopConfigurationPreview shopConfigurationPreview;

    private bool isVisible = false;

    void Start() {
        shopConfigurationPreview = GameObject.Find("ShopItemObjects").GetComponent<ShopConfigurationPreview>();
        scrollView = transform.GetComponentInParent<ScrollRect>().viewport;
    }

    void Update() {
        CheckPanelVisibility();
    }

    void CheckPanelVisibility() {
        Vector3[] viewportCorners = new Vector3[4];
        scrollView.GetWorldCorners(viewportCorners);
        float viewportLeftEdge = viewportCorners[0].x; // Borde izquierdo del scrollview "(nuestro contenedor)"

        Vector3[] panelCorners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(panelCorners);
        float panelLeftEdge = panelCorners[3].x; // Borde derecho del panel.

        if(panelLeftEdge < viewportLeftEdge) {
            if(isVisible) {
                isVisible = false;
                shopConfigurationPreview.ScrollLeft();
            }
        } else {
            if(!isVisible) {
                isVisible = true;
                shopConfigurationPreview.ScrollRight();
            }
        }
    }
}