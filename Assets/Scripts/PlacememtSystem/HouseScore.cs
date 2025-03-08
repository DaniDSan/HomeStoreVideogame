using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseScoreEnum {
    bad,
    good,
    excellent
}

public class HouseScore : MonoBehaviour {
    private HouseScoreEnum houseScore;
    [SerializeField] private int minItemsForExcellent;

    public static HouseScore Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void CalculateScore() {
        houseScore = GetHouseScore();
        PrintScoreMessage();
    }

    private HouseScoreEnum GetHouseScore() {
        // Verifica si todas las zonas son válidas
        if(!AreAllZonesValid()) {
            return HouseScoreEnum.bad;
        }

        int placeableLayer = LayerMask.NameToLayer("PlaceableObject");
        int itemCount = 0;

        foreach(ItemData item in FindObjectsOfType<ItemData>()) {
            if(item.gameObject.layer == placeableLayer) {
                if(!item.CompareTag("Wall") && !item.CompareTag("Floor")) {
                    itemCount++;
                }
            }
        }

        //Cantidad minima de objetos.
        if (itemCount <= 5) return HouseScoreEnum.bad;

        return itemCount >= minItemsForExcellent ? HouseScoreEnum.excellent : HouseScoreEnum.good;
    }

    private bool AreAllZonesValid() {
        ZoneChecker[] zoneCheckers = FindObjectsOfType<ZoneChecker>();

        foreach(ZoneChecker zoneChecker in zoneCheckers) {
            if(!zoneChecker.IsZoneValid()) {
                return false; // Si una zona es inválida, toda la casa es inválida
            }
        }
        return true; // Todas las zonas son válidas
    }

    private void PrintScoreMessage() {
        switch(houseScore) {
            case HouseScoreEnum.bad:
                Debug.Log("Malo, hay objetos en zonas incorrectas.");
                break;
            case HouseScoreEnum.good:
                Debug.Log("Bueno, la casa está bien organizada.");
                break;
            case HouseScoreEnum.excellent:
                Debug.Log("¡Excelente! La casa está perfectamente organizada.");
                break;
        }
    }
}
