using UnityEngine;
using UnityEditor;

public class PrefabEditor : EditorWindow {
    private string prefabsFolderPath = ""; // Ruta predeterminada para los prefabs
    private string soFolderPath = ""; // Ruta predeterminada para los ScriptableObjects

    // Crea el menú de la ventana
    [MenuItem("Tools/Asignar referencias a Prefabs de SO")]
    public static void ShowWindow() {
        GetWindow<PrefabEditor>("Asignar Scriptable Objects");
    }

    private void OnGUI() {
        // Título de la ventana
        GUILayout.Label("Asignar ScriptableObjects a Prefabs", EditorStyles.boldLabel);

        // Entrada para la ruta de los prefabs
        prefabsFolderPath = EditorGUILayout.TextField("Ruta de los Prefabs:", prefabsFolderPath);

        // Entrada para la ruta de los ScriptableObjects
        soFolderPath = EditorGUILayout.TextField("Ruta de los ScriptableObjects:", soFolderPath);

        // Botón para asignar los ScriptableObjects
        if(GUILayout.Button("Asignar Scriptable Objects")) {
            AsignScriptableObjects();
        }
    }

    private void AsignScriptableObjects() {
        // Asegúrate de que las rutas no estén vacías
        if(string.IsNullOrEmpty(prefabsFolderPath) || string.IsNullOrEmpty(soFolderPath)) {
            Debug.LogError("Por favor ingrese ambas rutas.");
            return;
        }

        // Obtener todos los prefabs en la carpeta indicada
        string[] prefabsPaths = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Prefabs/" + prefabsFolderPath });
        // Obtener todos los ScriptableObjects en la carpeta indicada
        string[] soPaths = AssetDatabase.FindAssets("t:ShopItemSO", new string[] { "Assets/ScriptableObjects/" + soFolderPath });

        // Verificar que la cantidad de prefabs coincida con la cantidad de ScriptableObjects
        if(prefabsPaths.Length != soPaths.Length) {
            Debug.LogError("La cantidad de prefabs no coincide con la cantidad de ScriptableObjects.");
            return;
        }

        // Asignar los ScriptableObjects a los prefabs
        for(int i = 0; i < prefabsPaths.Length; i++) {
            // Obtener el path del prefab y su ScriptableObject correspondiente
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabsPaths[i]);
            string soPath = AssetDatabase.GUIDToAssetPath(soPaths[i]);

            // Cargar el prefab y el ScriptableObject
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            ShopItemSO scriptableObject = AssetDatabase.LoadAssetAtPath<ShopItemSO>(soPath);

            // Asignar el ScriptableObject al prefab
            ItemData prefabScript = prefab.GetComponent<ItemData>();
            if(prefabScript != null) {
                prefabScript.shopItemSO = scriptableObject;

                // Marcar el prefab como modificado
                EditorUtility.SetDirty(prefab);
            }
        }

        // Guardar todos los cambios
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObjects asignados correctamente.");
    }
}
