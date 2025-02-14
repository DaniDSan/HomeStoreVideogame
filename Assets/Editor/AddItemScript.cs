using UnityEngine;
using UnityEditor;
using System.IO;

public class AddItemScript : EditorWindow {
    private string subfolderName = "";

    [MenuItem("Tools/Add ItemChecker to Prefabs")]
    public static void ShowWindow() {
        // Mostrar ventana personalizada del editor
        GetWindow<AddItemScript>("Agregar ItemChecker a Prefabs");
    }

    private void OnGUI() {
        // Campo de texto para ingresar la subcarpeta
        GUILayout.Label("Ruta de subcarpeta dentro de 'Assets/Prefabs'", EditorStyles.boldLabel);
        subfolderName = EditorGUILayout.TextField("Subcarpeta:", subfolderName);

        // Botón para ejecutar la función
        if(GUILayout.Button("Agregar ItemChecker")) {
            string path = $"Assets/Prefabs/{subfolderName}";
            AddItemChecker(path);
        }
    }

    public static void AddItemChecker(string path) {
        if(!Directory.Exists(path)) {
            Debug.LogError($"La ruta '{path}' no existe.");
            return;
        }

        // Buscar todos los archivos .prefab en la carpeta especificada
        string[] prefabPaths = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);

        int addedCount = 0; // Contador de componentes añadidos

        foreach(string prefabPath in prefabPaths) {
            // Cargar el prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if(prefab != null) {
                // Instanciar el prefab temporalmente para modificarlo
                GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if(prefabInstance != null) {
                    // Buscar el hijo llamado "ColliderObject"
                    Transform colliderObject = prefabInstance.transform.Find("ColliderObject");

                    if(colliderObject != null) {
                        // Verificar si ya tiene el componente "ItemChecker"
                        if(colliderObject.gameObject.GetComponent<ItemChecker>() == null) {
                            // Agregar el componente
                            colliderObject.gameObject.AddComponent<ItemChecker>();
                            addedCount++;

                            // Aplicar los cambios al prefab
                            PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.UserAction);
                            Debug.Log($"ItemChecker agregado al prefab '{prefab.name}' en '{prefabPath}'");
                        }
                    }

                    // Destruir la instancia para evitar residuos
                    GameObject.DestroyImmediate(prefabInstance);
                }
            }
        }

        Debug.Log($"Operación completada. Se agregó 'ItemChecker' a {addedCount} prefabs.");
    }
}