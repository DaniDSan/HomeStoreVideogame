using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoObjetoTienda", menuName = "Tienda/ObjetoTienda", order = 1)]
public class ShopItemSO : ScriptableObject {
    public GameObject prefabItem;
    public string category;
    public int price;

    [Range(0, 1000)]  // Esto te permite especificar un rango en el Inspector.
    public int minPrice = 10;

    [Range(0, 1000)]  // Rango para el precio máximo.
    public int maxPrice = 100;

    [ContextMenu("Set Price")]
    void SetPrice() {
        if(minPrice < maxPrice) {
            price = Random.Range(minPrice, maxPrice);
        }
    }
}
