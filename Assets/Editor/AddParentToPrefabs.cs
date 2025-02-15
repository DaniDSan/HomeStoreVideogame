using UnityEngine;
using UnityEditor;
using System.IO;

public class AddParentToPrefabs : EditorWindow {
    private string subfolderName = ""; // Ruta de la subcarpeta
    private string prefabPrefix = "closet"; // Prefijo del nombre de los prefabs

    [MenuItem("Tools/Agregar Objeto Padre a Prefabs en la Escena")]
    public static void ShowWindow() {
        // Mostrar ventana personalizada del editor
        GetWindow<AddParentToPrefabs>("Agregar Objeto Padre a Prefabs en la Escena");
    }

    private void OnGUI() {
        // Campo de texto para ingresar la subcarpeta
        GUILayout.Label("Ruta de subcarpeta dentro de 'Assets/Interior_14'", EditorStyles.boldLabel);
        subfolderName = EditorGUILayout.TextField("Subcarpeta:", subfolderName);

        // Campo de texto para ingresar el prefijo de los prefabs
        GUILayout.Label("Prefijo del nombre del prefab (ej. 'closet')", EditorStyles.boldLabel);
        prefabPrefix = EditorGUILayout.TextField("Prefijo del Prefab:", prefabPrefix);

        // Botón para ejecutar la función
        if(GUILayout.Button("Agregar Objeto Padre a Prefabs en la Escena")) {
            string path = $"Assets/Interior_14/{subfolderName}";
            AddParentToPrefabsInScene(path, prefabPrefix);
        }
    }

    public static void AddParentToPrefabsInScene(string path, string prefabPrefix) {
        if(!Directory.Exists(path)) {
            Debug.LogError($"La ruta '{path}' no existe.");
            return;
        }

        // Buscar todos los archivos .prefab en la carpeta especificada
        string[] prefabPaths = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
        int count = 0;

        foreach(string prefabPath in prefabPaths) {
            // Cargar el prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if(prefab != null) {
                // Filtrar solo los prefabs cuyo nombre empieza con el prefijo especificado
                if(prefab.name.StartsWith(prefabPrefix)) {
                    // Instanciar el prefab en la escena
                    GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                    if(prefabInstance != null) {
                        // Crear un nuevo objeto padre con el nombre del prefab
                        GameObject newParent = new GameObject(prefab.name);

                        // Hacer que el prefab sea hijo del nuevo objeto padre en la escena
                        prefabInstance.transform.SetParent(newParent.transform);
                        prefabInstance.transform.localPosition = Vector3.zero; // Aseguramos que la posición sea la correcta

                        // Registrar la creación del nuevo objeto padre para deshacer la acción en caso de ser necesario
                        Undo.RegisterCreatedObjectUndo(newParent, "Crear objeto padre para prefab");

                        // Registrar la creación del prefab instanciado para deshacer la acción
                        Undo.RegisterCreatedObjectUndo(prefabInstance, "Instanciar prefab con objeto padre");

                        count++;
                    }
                }
            }
        }

        Debug.Log($"Se agregaron objetos padre a {count} prefabs con el prefijo '{prefabPrefix}' en '{path}'. Los cambios están en la escena.");
    }
}