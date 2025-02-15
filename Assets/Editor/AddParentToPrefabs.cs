using UnityEngine;
using UnityEditor;
using System.IO;

public class AddParentToPrefabs : EditorWindow {
    private string subfolderName = ""; // Valor por defecto

    [MenuItem("Tools/Agregar Objeto Padre a Prefabs")]
    public static void ShowWindow() {
        // Mostrar ventana personalizada del editor
        GetWindow<AddParentToPrefabs>("Agregar Objeto Padre a Prefabs");
    }

    private void OnGUI() {
        // Campo de texto para ingresar la subcarpeta
        GUILayout.Label("Ruta de subcarpeta dentro de 'Assets/Prefabs'", EditorStyles.boldLabel);
        subfolderName = EditorGUILayout.TextField("Subcarpeta:", subfolderName);

        // Botón para ejecutar la función
        if(GUILayout.Button("Agregar Objeto Padre")) {
            string path = $"Assets/Prefabs/{subfolderName}";
            AddParentToPrefabsAtPath(path);
        }
    }

    public static void AddParentToPrefabsAtPath(string path) {
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
                // Instanciar una copia del prefab para modificar
                GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if(prefabInstance != null) {
                    // Crear un nuevo objeto padre con el mismo nombre que el prefab
                    GameObject newParent = new GameObject(prefab.name);

                    // Reorganizar la jerarquía: hacer que el prefab sea hijo del nuevo padre
                    prefabInstance.transform.SetParent(newParent.transform);
                    prefabInstance.transform.localPosition = Vector3.zero; // Opcional: Asegurar posición

                    // Aplicar los cambios al prefab (se sobrescribe el prefab original)
                    PrefabUtility.SaveAsPrefabAssetAndConnect(newParent, prefabPath, InteractionMode.UserAction);

                    count++;

                    // Destruir la instancia para evitar residuos en la escena
                    GameObject.DestroyImmediate(newParent);
                }
            }
        }

        Debug.Log($"Se agregaron objetos padre a {count} prefabs en '{path}'.");
    }
}