using UnityEngine;
using System.Collections.Generic;

public enum Zone {
    Kitchen,
    Bathroom,
    Other
}

public class ZoneChecker : MonoBehaviour {
    public Zone zone;
    public GameObject zoneVisualInstance;

    public static ZoneChecker Instance;

    private void Awake() {
        if(Instance != null) {
            Instance = this;
        }
    }

    public void ActivateZone(Zone newZone) {
        zone = newZone;
        StartRaycast();
    }

    public void StartRaycast() {
        Vector3 rayOrigin = transform.position + Vector3.down;
        Debug.DrawRay(rayOrigin, Vector3.up * 5f, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.up, 5f);

        foreach(RaycastHit hit in hits) {
            ItemData parentData = hit.collider.GetComponentInParent<ItemData>();
            if(parentData != null && IsValidCategory(parentData.shopItemSO.category)) {
                print("A");
            }
        }
    }

    private bool IsValidCategory(string category) {
        return zone switch {
            Zone.Kitchen => category != "bed" && category != "bathroom",
            Zone.Bathroom => category != "bed" && category != "kitchen",
            Zone.Other => category != "bathroom" && category != "kitchen",
            _ => true
        };
    }
}
