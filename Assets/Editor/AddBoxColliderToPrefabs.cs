using UnityEngine;
using UnityEditor;
using System.IO;

public class AddBoxColliderToPrefabs : EditorWindow {
    private string subfolderName = ""; // Valor por defecto

    [MenuItem("Tools/Agregar BoxCollider Hijo a Prefabs")]
    public static void ShowWindow() {
        // Mostrar ventana personalizada del editor
        GetWindow<AddBoxColliderToPrefabs>("Agregar BoxCollider a Prefabs");
    }

    private void OnGUI() {
        // Campo de texto para ingresar la subcarpeta
        GUILayout.Label("Ruta de subcarpeta dentro de 'Assets/Prefabs'", EditorStyles.boldLabel);
        subfolderName = EditorGUILayout.TextField("Subcarpeta:", subfolderName);

        // Botón para ejecutar la función
        if(GUILayout.Button("Agregar BoxCollider")) {
            string path = $"Assets/Prefabs/{subfolderName}";
            AddBoxColliderToPrefabsAtPath(path);
        }
    }

    public static void AddBoxColliderToPrefabsAtPath(string path) {
        if(!Directory.Exists(path)) {
            Debug.LogError($"La ruta '{path}' no existe.");
            return;
        }

        // Buscar todos los prefabs en la carpeta especificada
        string[] prefabPaths = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
        int count = 0;

        foreach(string prefabPath in prefabPaths) {
            // Cargar el prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
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

        Debug.Log($"Se agregaron objetos con BoxCollider a {count} prefabs en '{path}'.");
    }
}