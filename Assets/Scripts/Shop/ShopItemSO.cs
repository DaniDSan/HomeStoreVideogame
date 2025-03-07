using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoObjetoTienda", menuName = "Tienda/ObjetoTienda", order = 1)]
public class ShopItemSO : ScriptableObject {
    public GameObject prefabItem;
    public int price;
    public string category;
}
