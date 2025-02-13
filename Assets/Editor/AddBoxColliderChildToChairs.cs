using UnityEngine;
using UnityEditor;
using System.IO;

public class AddBoxColliderChildToChairs : EditorWindow {
    [MenuItem("Herramientas/Agregar BoxCollider Hijo a Prefabs de Chairs")]
    public static void AddBoxColliderToPrefabs() {
        // Buscar todos los prefabs en la carpeta "Assets/Prefabs/Chairs"
        string[] prefabPaths = Directory.GetFiles("Assets/Prefabs/Chairs", "*.prefab", SearchOption.AllDirectories);
        int count = 0;

        foreach(string path in prefabPaths) {
            // Cargar el prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(prefab != null) {
                // Comenzar a modificar la instancia del prefab
                GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if(prefabInstance != null) {
                    // Verificar si ya tiene un hijo llamado "ColliderObject"
                    Transform existingChild = prefabInstance.transform.Find("ColliderObject");
                    if(existingChild == null) {
                        // Crear un objeto vacío hijo
                        GameObject colliderChild = new GameObject("ColliderObject");
                        colliderChild.transform.SetParent(prefabInstance.transform);
                        colliderChild.transform.localPosition = Vector3.zero;

                        // Agregar el BoxCollider
                        BoxCollider boxCollider = colliderChild.AddComponent<BoxCollider>();
                        boxCollider.isTrigger = false; // Ajusta según tus necesidades

                        // Aplicar los cambios al prefab
                        PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.UserAction);
                        count++;
                    }

                    // Destruir la instancia para evitar residuos en la escena
                    GameObject.DestroyImmediate(prefabInstance);
                }
            }
        }

        Debug.Log($"Se agregaron objetos con BoxCollider a {count} prefabs en 'Assets/Prefabs/Chairs'.");
    }
}