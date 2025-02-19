using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    public GameObject prefabPanel;
    public Transform content;

    public ShopItemSO[] shopItemSOs;
    public List<GameObject> shopPanels;
    public ShopConfigurationPreview shopConfigurationPreview;
    public List<RenderTexture> renderTextures;
    public List<Button> purchaseButtons;

    private void Start() {
        LoadPanles();
    }

    void LoadPanles() {
        foreach(ShopItemSO shopItem in shopItemSOs) {
            shopPanels.Add(Instantiate(prefabPanel, content));
        }

        int renderIndex = 0;
        for(int i = 0; i < shopItemSOs.Length; i++) {
            int index = i;
            ShopTemplate template = shopPanels[i].GetComponent<ShopTemplate>();
            template.priceTxt.text = shopItemSOs[i].price.ToString();
            template.previewImg.texture = renderTextures[renderIndex];
            renderIndex = (renderIndex + 1) % renderTextures.Count;

            shopConfigurationPreview.actaulPrefabs.Add(shopItemSOs[i].prefabItem);

            purchaseButtons.Add(template.buyBtn);
            purchaseButtons[i].onClick.AddListener(() => BuyItem(index));
        }
    }

    void BuyItem(int btnNumber) {
        print(shopItemSOs[btnNumber].price.ToString());
    }
}
