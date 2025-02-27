using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ItemData : MonoBehaviour {
    public ShopItemSO shopItemSO;

    private string lastTag; // Para detectar cambios en el Inspector

    void Update() {
        if(gameObject.tag != lastTag) // Solo actualiza si el tag cambia
        {
            lastTag = gameObject.tag;
            //ChangeTagRecursively(transform, lastTag);
        }
    }

    void ChangeTagRecursively(Transform obj, string newTag) {
        obj.gameObject.tag = newTag;
        foreach(Transform child in obj) {
            ChangeTagRecursively(child, newTag);
        }
    }
}
