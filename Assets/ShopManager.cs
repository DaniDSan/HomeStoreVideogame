using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    public GameObject prefabPanel;
    public Transform content;

    public ShopItemSO[] shopItemSOs;
    public List<GameObject> shopPanels;
    public RenderTexture[] renderTextures;
    public List<Button> purchaseButtons;

    private void Start() {
        LoadPanles();
    }

    void LoadPanles() {
        foreach(ShopItemSO shopItem in shopItemSOs) {
            shopPanels.Add(Instantiate(prefabPanel, content));
        }

        for(int i = 0; i < shopItemSOs.Length; i++) {
            int index = i;
            shopPanels[i].GetComponent<ShopTemplate>().priceTxt.text = shopItemSOs[i].price.ToString();
            shopPanels[i].GetComponent<ShopTemplate>().previewImg.texture = renderTextures[i];

            purchaseButtons.Add(shopPanels[i].GetComponent<ShopTemplate>().buyBtn);
            purchaseButtons[i].onClick.AddListener(() => BuyItem(index));
        }
    }

    void BuyItem(int btnNumber) {
        print(shopItemSOs[btnNumber].price.ToString());
    }
}
