using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour {
    public GameObject placeHolderItem;
    public ShopItemSO placeableItem;
    public GameObject visualRef;
    public GameObject house;
    private GameObject tempItem;
    private GameObject draggedItem;
    private Camera cam;

    bool isShowing = false;
    bool isDragging = false;
    public bool isSelling = false;
    public bool isSelectingKitchen;
    public bool isSelectingBathroom;

    private Material originMaterial;
    public Material red;
    public Material green;
    public Material blue;

    public Color colorRed;
    public Color colorGreen;

    [SerializeField]
    ParticleSystem placeParticles;

    public static PlacementSystem Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    private void OnEnable() {
        house = GameObject.Find("House(Clone)");
    }

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if(!isSelling && !isSelectingKitchen && !isSelectingBathroom) {
            HandlePlacementFlow();
            HandleCancelPlacement();
            HandleDragItem();
        } else {
            CancelPreview();
        }

        if(placeableItem == null) {
            OutlineHandler.Instance.OutlineCheck();
        }
    }

    private void HandlePlacementFlow() {
        if(placeableItem == null) return;
        PlaceHolderControl();
    }

    private void HandleCancelPlacement() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            CancelPreview();
            if(isDragging) {
                CancelDragItem();
            }
        }
    }

    private void HandleDragItem() {
        if(Input.GetMouseButtonDown(0)) {
            DargItem();
        }
    }

    private void PlaceHolderControl() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            ShowPlaceholder(hit);
            RotateItem();
            TryPlaceItem();
        } else {
            placeHolderItem.SetActive(false);
        }
    }

    private void ShowPlaceholder(RaycastHit hit) {
        placeHolderItem.transform.position = new Vector3(Mathf.Round(hit.point.x), visualRef.transform.position.y, Mathf.Round(hit.point.z));
        placeHolderItem.SetActive(true);
        ItemPrevisualizer();
    }

    private void RotateItem() {
        if(Input.GetMouseButtonDown(1)) {
            AudioManager.instance.PlaySFX(AudioManager.instance.placementSoundsEffects.rotateSFX);
            placeHolderItem.transform.Rotate(0, 90, 0);
        }
    }

    private void TryPlaceItem() {
        if(CanPlace() && Input.GetMouseButtonDown(0)) {
            PlaceItem();
            ParticlesHandler.Instance.PlayEffect(tempItem);
        }
    }

    private void PlaceItem() {
        AudioManager.instance.PlaySFX(AudioManager.instance.placementSoundsEffects.placeSFX);
        GameObject newItem = Instantiate(placeableItem.prefabItem,
            tempItem.transform.position,
            tempItem.transform.rotation, house.transform);

        if(!isDragging) {
            GameManager.instance.RemoveMoney(newItem.GetComponent<ItemData>().shopItemSO.price);
        } else {
            CancelPreview();
            Destroy(draggedItem);
            StartCoroutine(ResetDragDealy());
        }
    }

    private void ItemPrevisualizer() {
        if(isShowing) return;

        Destroy(tempItem);
        isShowing = true;

        InstantiateTempItem();
        SetupColliderVisualization();
    }

    private void InstantiateTempItem() {
        tempItem = Instantiate(placeableItem.prefabItem,
            new Vector3(placeHolderItem.transform.position.x, placeableItem.prefabItem.transform.position.y, placeHolderItem.transform.position.z),
            placeHolderItem.transform.rotation,
            placeHolderItem.transform);

        if(tempItem.CompareTag("Wall")) {
            visualRef.SetActive(false);
        } else {
            visualRef.SetActive(true);
        }
    }

    private void SetupColliderVisualization() {
        BoxCollider[] boxColliders = tempItem.GetComponentsInChildren<BoxCollider>();
        if(boxColliders[0].size != Vector3.one) {
            AdjustPositionBySize(boxColliders[0]);
        } else {
            tempItem.transform.localPosition = new Vector3(0f, tempItem.transform.position.y, 0f);
            visualRef.transform.localPosition = Vector3.zero;
        }

        visualRef.transform.localScale = new Vector3(boxColliders[0].size.x, boxColliders[0].size.z, 1f);
        DisableAllColliders(boxColliders);
    }

    private void DisableAllColliders(BoxCollider[] boxColliders) {
        foreach(BoxCollider boxCollider in boxColliders) {
            boxCollider.enabled = false;
        }
    }

    private bool CanPlace() {
        bool canPlace = IsPlacementValid();
        UpdateMaterialColors(canPlace);
        return canPlace;
    }

    private bool IsPlacementValid() {
        ItemChecker[] itemCheckers = GetComponentsInChildren<ItemChecker>();
        foreach(var checker in itemCheckers) {
            if(!checker.canPlace) return false;
        }
        return true;
    }

    private void UpdateMaterialColors(bool canPlace) {
        Material targetMaterial = canPlace ? green : red;
        Color targetColor = canPlace ? colorGreen : colorRed;

        if(visualRef.activeSelf) {
            placeHolderItem.GetComponentInChildren<MeshRenderer>().material = targetMaterial;
        }

        foreach(MeshRenderer tempItems in tempItem.GetComponentsInChildren<MeshRenderer>())
            tempItems.material.color = targetColor;
    }

    private void AdjustPositionBySize(BoxCollider boxCollider) {
        Vector3 tempVect = Vector3.zero;

        if(Mathf.RoundToInt(boxCollider.size.x) % 2 == 0) {
            tempVect.x = boxCollider.size.x / 2 - 0.5f;
        }
        if(Mathf.RoundToInt(boxCollider.size.z) % 2 == 0) {
            tempVect.z = boxCollider.size.z / 2 - 0.5f;
        }

        tempItem.transform.localPosition = new Vector3(tempVect.x, tempItem.transform.position.y, tempVect.z);
        visualRef.transform.localPosition = tempVect;
    }

    public void CancelPreview() {
        placeableItem = null;
        isShowing = false;
        placeHolderItem.SetActive(false);
    }

    private void DargItem() {
        if(placeableItem != null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            GameObject hitObject = hit.collider.gameObject;
            TryPickupItem(hitObject);
        }
    }

    private void TryPickupItem(GameObject hitObject) {
        OutlineHandler.Instance.ResetOutline();
        if(!isDragging) {
            if(LayerMask.LayerToName(hitObject.layer) == "PlaceableObject") {
                hitObject.GetComponent<BoxCollider>().enabled = false;

                GameObject parentObject = hitObject.GetComponentInParent<ItemData>().gameObject;
                placeableItem = parentObject.GetComponent<ItemData>().shopItemSO;
                placeHolderItem.transform.rotation = hitObject.transform.rotation;

                draggedItem = parentObject;
                isDragging = true;
                placeHolderItem.SetActive(true);

                originMaterial = draggedItem.GetComponentInChildren<MeshRenderer>().material;

                foreach(MeshRenderer draggedItems in draggedItem.GetComponentsInChildren<MeshRenderer>())
                    draggedItems.material = blue;
            }
        }
    }

    void CancelDragItem() {
        foreach(MeshRenderer draggedItems in draggedItem.GetComponentsInChildren<MeshRenderer>())
            draggedItems.material = originMaterial;
        draggedItem.GetComponentInChildren<BoxCollider>().enabled = true;
        draggedItem = null;
        isDragging = false;
    }

    IEnumerator ResetDragDealy() {
        yield return new WaitForSeconds(0.1f);
        isDragging = false;
        placeableItem = null;
    }
}