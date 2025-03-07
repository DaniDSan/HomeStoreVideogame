using UnityEngine;

public class ZoneHandler : MonoBehaviour {
    [SerializeField]
    GameObject zoneVisualRef;

    [SerializeField] LayerMask layerMask;

    void Update() {
        if(Input.GetMouseButton(0)) {
            HandleZoneSelection();
        }

        if(Input.GetMouseButton(1)) // Clic derecho
        {
            SetToOther(); // Cambiar a Zone.Other solo si hay una zona seleccionada
        }
    }

    private void HandleZoneSelection() {
        if(!PlacementSystem.Instance.isSelectingKitchen && !PlacementSystem.Instance.isSelectingBathroom) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
        foreach(RaycastHit hit in hits) {
            if(((1 << hit.collider.gameObject.layer) & layerMask) != 0) {
                if(hit.collider.CompareTag("FloorBase")) {
                    ZoneChecker zoneChecker = hit.collider.GetComponentInParent<ZoneChecker>();
                    if(zoneChecker == null) return;

                    if(zoneChecker.zoneVisualInstance == null) {
                        zoneChecker.zoneVisualInstance = Instantiate(zoneVisualRef, zoneChecker.transform.position, Quaternion.identity);
                    }

                    if(PlacementSystem.Instance.isSelectingKitchen) {
                        zoneChecker.ActivateZone(Zone.Kitchen);
                    } else if(PlacementSystem.Instance.isSelectingBathroom) {
                        zoneChecker.ActivateZone(Zone.Bathroom);
                    }
                    return;
                }
            }
        }
    }

    public void ToggleKitchenSelection() {
        PlacementSystem.Instance.isSelectingKitchen = !PlacementSystem.Instance.isSelectingKitchen;
        PlacementSystem.Instance.isSelectingBathroom = false;
    }

    public void ToggleBathroomSelection() {
        PlacementSystem.Instance.isSelectingBathroom = !PlacementSystem.Instance.isSelectingBathroom;
        PlacementSystem.Instance.isSelectingKitchen = false;
    }

    private void SetToOther() {
        if(!PlacementSystem.Instance.isSelectingKitchen && !PlacementSystem.Instance.isSelectingBathroom) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
        foreach(RaycastHit hit in hits) {
            if(((1 << hit.collider.gameObject.layer) & layerMask) != 0) {
                if(hit.collider.CompareTag("FloorBase")) {
                    ZoneChecker zoneChecker = hit.collider.GetComponentInParent<ZoneChecker>();
                    if(zoneChecker != null) {
                        zoneChecker.ActivateZone(Zone.Other); // Activar Zone.Other
                    }

                    if(zoneChecker.zoneVisualInstance != null) {
                        // Instanciar el objeto solo si no hay uno ya
                        Destroy(zoneChecker.zoneVisualInstance);
                    }
                    return;
                }
            }
        }
    }
}
