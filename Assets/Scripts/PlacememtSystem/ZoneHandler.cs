using UnityEngine;

public class ZoneHandler : MonoBehaviour {
    [SerializeField]
    GameObject zoneVisualRef;

    void Update() {
        if(Input.GetMouseButton(0)) {
            HandleZoneSelection();
        }

        if(Input.GetMouseButtonDown(1)) // Clic derecho
        {
            SetToOther(); // Cambiar a Zone.Other solo si hay una zona seleccionada
        }
    }

    private void HandleZoneSelection() {
        if(!PlacementSystem.Instance.isSelectingKitchen && !PlacementSystem.Instance.isSelectingBathroom) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Floor")) {
            ZoneChecker zoneChecker = hit.collider.GetComponentInParent<ZoneChecker>();
            if(zoneChecker == null) return;
            if(zoneChecker.zoneVisualInstance == null) {
                // Instanciar el objeto solo si no hay uno ya
                zoneChecker.zoneVisualInstance = Instantiate(zoneVisualRef, zoneChecker.transform.position, Quaternion.identity);
            }

            if(PlacementSystem.Instance.isSelectingKitchen)
                zoneChecker.ActivateZone(Zone.Kitchen);
            else if(PlacementSystem.Instance.isSelectingBathroom)
                zoneChecker.ActivateZone(Zone.Bathroom);
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
        if(Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Floor")) {
            ZoneChecker zoneChecker = hit.collider.GetComponentInParent<ZoneChecker>();
            if(zoneChecker != null) {
                zoneChecker.ActivateZone(Zone.Other); // Activar Zone.Other
            }
        }
    }
}
