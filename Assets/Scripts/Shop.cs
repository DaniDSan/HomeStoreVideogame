using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Shop : MonoBehaviour {
    public List<ShopItemSO> objetosEnTienda; // Lista de objetos de la tienda
    public VistaPreviaTienda vistaPreviaTienda; // Referencia al script de vista previa
    public List<TextMeshProUGUI> priceList;

    private void Start() {
        foreach(TextMeshProUGUI item in priceList) {
            item.text = "0";
        }
    }

    // Método para seleccionar un objeto
    public void SeleccionarObjeto(ShopItemSO shopItem) {
        // Llamar a la vista previa y pasar el prefab del objeto seleccionado
        vistaPreviaTienda.MostrarObjeto(shopItem.prefabItem);
    }
}