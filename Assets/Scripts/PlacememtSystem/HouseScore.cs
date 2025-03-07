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

    void CalculateScore() {
        houseScore = GetHouseScore();
        PrintScoreMessage();
    }

    private HouseScoreEnum GetHouseScore() {
        // Verifica si todas las zonas son v�lidas
        if(!AreAllZonesValid()) {
            return HouseScoreEnum.bad;
        }

        int placeableLayer = LayerMask.NameToLayer("PlaceableObject");
        int itemCount = 0;

        foreach(ItemData item in FindObjectsOfType<ItemData>()) {
            if(item.gameObject.layer == placeableLayer) {
                itemCount++;
            }
        }
        return itemCount >= minItemsForExcellent ? HouseScoreEnum.excellent : HouseScoreEnum.good;
    }

    private bool AreAllZonesValid() {
        ZoneChecker[] zoneCheckers = FindObjectsOfType<ZoneChecker>();

        foreach(ZoneChecker zoneChecker in zoneCheckers) {
            if(!zoneChecker.IsZoneValid()) {
                return false; // Si una zona es inv�lida, toda la casa es inv�lida
            }
        }
        return true; // Todas las zonas son v�lidas
    }

    private void PrintScoreMessage() {
        switch(houseScore) {
            case HouseScoreEnum.bad:
                Debug.Log("Malo, hay objetos en zonas incorrectas.");
                break;
            case HouseScoreEnum.good:
                Debug.Log("Bueno, la casa est� bien organizada.");
                break;
            case HouseScoreEnum.excellent:
                Debug.Log("�Excelente! La casa est� perfectamente organizada.");
                break;
        }
    }
}
