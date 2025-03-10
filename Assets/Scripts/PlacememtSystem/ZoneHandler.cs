using UnityEngine;
using UnityEngine.UI;

public class ZoneHandler : MonoBehaviour {
    [SerializeField] GameObject zoneVisualRef;
    [SerializeField] Material kitchenMat;
    [SerializeField] Material bathroomMat;
    public Transform visualRefsTransform;

    [SerializeField] LayerMask layerMask;

    [SerializeField] Toggle SetRoomToggle;

    public static ZoneHandler Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void GetRef() {
        visualRefsTransform = PlacementSystem.Instance.house.transform.Find("VisualRefs");
    }

    public void DeleteRef() {
        visualRefsTransform = null;
    }

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
        if(visualRefsTransform == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
        foreach(RaycastHit hit in hits) {
            if(((1 << hit.collider.gameObject.layer) & layerMask) != 0) {
                if(hit.collider.CompareTag("FloorBase")) {
                    ZoneChecker zoneChecker = hit.collider.GetComponentInParent<ZoneChecker>();
                    if(zoneChecker == null) return;

                    if(PlacementSystem.Instance.isSelectingKitchen) {
                        zoneVisualRef.GetComponent<Renderer>().material = kitchenMat;
                        zoneChecker.ActivateZone(Zone.Kitchen);
                    } else if(PlacementSystem.Instance.isSelectingBathroom) {
                        zoneVisualRef.GetComponent<Renderer>().material = bathroomMat;
                        zoneChecker.ActivateZone(Zone.Bathroom);
                    }

                    if(zoneChecker.zoneVisualInstance == null) {
                        zoneChecker.zoneVisualInstance = Instantiate(zoneVisualRef, zoneChecker.transform.position + new Vector3(0f, zoneVisualRef.transform.position.y, 0f), Quaternion.identity, visualRefsTransform);
                    }
                    return;
                }
            }
        }
    }

    public void ToggleKitchenSelection() {
        GetRef();
        PlacementSystem.Instance.isSelectingKitchen = !PlacementSystem.Instance.isSelectingKitchen;
        PlacementSystem.Instance.isSelectingBathroom = false;
        visualRefsTransform.gameObject.SetActive(PlacementSystem.Instance.isSelectingKitchen);
    }

    public void ToggleBathroomSelection() {
        GetRef();
        PlacementSystem.Instance.isSelectingBathroom = !PlacementSystem.Instance.isSelectingBathroom;
        PlacementSystem.Instance.isSelectingKitchen = false;
        visualRefsTransform.gameObject.SetActive(PlacementSystem.Instance.isSelectingBathroom);
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

    public void SetOffRoomSelection() {
        if(!SetRoomToggle.isOn) {
            PlacementSystem.Instance.isSelectingKitchen = false;
            PlacementSystem.Instance.isSelectingBathroom = false;
            if(visualRefsTransform != null) {
                visualRefsTransform.gameObject.SetActive(false);
            }
        }
    }
}
