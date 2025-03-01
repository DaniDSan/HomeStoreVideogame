using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemData : MonoBehaviour {
    public ShopItemSO shopItemSO;

    [ContextMenu("Change Children Tags")]
    private void ChangeTags() {
        if(gameObject == null) return;

        string parentTag = gameObject.tag;
        Transform[] children = GetComponentsInChildren<Transform>(true);

        Undo.RecordObject(this, "Change Children Tags");

        foreach(Transform child in children) {
            if(child != transform) // Evitar cambiar el padre
            {
                Undo.RecordObject(child.gameObject, "Change Child Tag");
                child.gameObject.tag = parentTag;
            }
        }

        Debug.Log("Tags updated for all children.");
    }
}
