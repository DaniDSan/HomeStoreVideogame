using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Category {
    public string name;                  // Nombre de la categoría
    public ShopItemSO[] shopItemSOs;     // Lista de items en esta categoría
}

public class ShopManager : MonoBehaviour {
    public GameObject prefabPanel;                // Prefab del panel de cada item
    public Transform content;                     // Contenedor para colocar los paneles
    public ScrollRect scrollRect;                 // Referencia al ScrollRect del contenedor

    public ShopConfigurationPreview shopConfigurationPreview;
    public List<RenderTexture> renderTextures;
    public List<Button> purchaseButtons;

    // Lista de categorías
    public List<Category> categories = new List<Category>();

    // Paneles actuales cargados en escena
    private List<GameObject> currentPanels = new List<GameObject>();

    private void Start() {
        // Cargar la primera categoría por defecto
        if(categories.Count > 0) {
            LoadPanelsForCategory(categories[0].name);
        }
    }

    // Cargar solo los paneles de la categoría especificada
    public void LoadPanelsForCategory(string categoryName) {
        shopConfigurationPreview.ResetIndex();
        // Destruir los paneles actuales antes de cargar la nueva categoría
        foreach(GameObject panel in currentPanels) {
            Destroy(panel);
        }
        currentPanels.Clear();  // Limpiar la lista de paneles actuales
        shopConfigurationPreview.actaulPrefabs.Clear();
        purchaseButtons.Clear();

        // Reiniciar la posición del ScrollRect al principio (izquierda o parte superior)
        ResetScrollPosition();


        // Buscar la categoría correspondiente
        Category selectedCategory = new Category();
        bool categoryFound = false;
        foreach(Category category in categories) {
            if(category.name == categoryName) {
                selectedCategory = category;
                categoryFound = true;
                break;
            }
        }

        if(!categoryFound) {
            Debug.LogWarning($"Category '{categoryName}' not found.");
            return;
        }

        int renderIndex = 0;

        // Crear nuevos paneles para los items en la categoría seleccionada
        foreach(ShopItemSO shopItem in selectedCategory.shopItemSOs) {
            GameObject newPanel = Instantiate(prefabPanel, content);  // Instanciar el panel
            currentPanels.Add(newPanel);  // Agregar el panel a la lista actual

            // Configurar el panel
            ShopTemplate template = newPanel.GetComponent<ShopTemplate>();
            template.priceTxt.text = shopItem.price.ToString() + "€";
            template.previewImg.texture = renderTextures[renderIndex];
            renderIndex = (renderIndex + 1) % renderTextures.Count;

            shopConfigurationPreview.actaulPrefabs.Add(shopItem.prefabItem);

            // Configurar el botón de compra
            Button buyButton = template.buyBtn;
            int index = purchaseButtons.Count;
            purchaseButtons.Add(buyButton);
            buyButton.onClick.AddListener(() => SelectItem(shopItem));

            shopConfigurationPreview.ShowPreview(shopItem.prefabItem, shopConfigurationPreview.actaulPrefabs.Count - 1);
        }
    }

    // Método para comprar un ítem
    void SelectItem(ShopItemSO shopItem) {
        PlacementSystem.Instance.isSelectingBathroom = false;
        PlacementSystem.Instance.isSelectingKitchen = false;
        PlacementSystem.Instance.isSelling = false;
        PlacementSystem.Instance.CancelPreview();
        PlacementSystem.Instance.placeableItem = shopItem;
    }

    // Método para reiniciar la posición del ScrollRect
    void ResetScrollPosition() {
        if(scrollRect != null) {
            // Establecer la posición de desplazamiento en la parte superior izquierda
            scrollRect.normalizedPosition = new Vector2(0, 1);
        }
    }
}