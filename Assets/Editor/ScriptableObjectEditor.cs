using UnityEngine;
using UnityEditor;

public class ScriptableObjectEditor : EditorWindow {
    private string prefabsFolderPath = "Assets/Prefabs"; // Ruta base predeterminada para los prefabs
    private string soFolderPath = "Assets/ScriptableObjects"; // Ruta base predeterminada para los ScriptableObjects

    private string prefabsFolderEnd = ""; // Parte final de la ruta de los prefabs (para completar)
    private string soFolderEnd = ""; // Parte final de la ruta de los ScriptableObjects (para completar)

    // Crea el menú de la ventana
    [MenuItem("Tools/Asignar a SO su prefab")]
    public static void ShowWindow() {
        GetWindow<ScriptableObjectEditor>("Asignar Prefabs a Scriptable Objects");
    }

    private void OnGUI() {
        // Título de la ventana
        GUILayout.Label("Asignar Prefabs a ScriptableObjects", EditorStyles.boldLabel);

        // Entrada para la parte final de la ruta de los prefabs
        prefabsFolderEnd = EditorGUILayout.TextField("Ruta final de los Prefabs:", prefabsFolderEnd);

        // Entrada para la parte final de la ruta de los ScriptableObjects
        soFolderEnd = EditorGUILayout.TextField("Ruta final de los ScriptableObjects:", soFolderEnd);

        // Botón para asignar los Prefabs
        if(GUILayout.Button("Asignar Prefabs a Scriptable Objects")) {
            AssignPrefabs();
        }
    }

    private void AssignPrefabs() {
        // Construir las rutas completas
        string fullPrefabPath = prefabsFolderPath + "/" + prefabsFolderEnd;
        string fullSOPath = soFolderPath + "/" + soFolderEnd;

        // Asegúrate de que las rutas no estén vacías
        if(string.IsNullOrEmpty(prefabsFolderEnd) || string.IsNullOrEmpty(soFolderEnd)) {
            Debug.LogError("Por favor ingrese ambas rutas finales.");
            return;
        }

        // Obtener todos los prefabs en la carpeta indicada
        string[] prefabsPaths = AssetDatabase.FindAssets("t:Prefab", new string[] { fullPrefabPath });
        // Obtener todos los ScriptableObjects en la carpeta indicada
        string[] soPaths = AssetDatabase.FindAssets("t:ShopItemSO", new string[] { fullSOPath });

        // Verificar que la cantidad de prefabs coincida con la cantidad de ScriptableObjects
        if(prefabsPaths.Length != soPaths.Length) {
            Debug.LogError("La cantidad de prefabs no coincide con la cantidad de ScriptableObjects.");
            return;
        }

        // Asignar los Prefabs a los ScriptableObjects
        for(int i = 0; i < prefabsPaths.Length; i++) {
            // Obtener el path del prefab y su ScriptableObject correspondiente
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabsPaths[i]);
            string soPath = AssetDatabase.GUIDToAssetPath(soPaths[i]);

            // Asegúrate de que el prefab y el ScriptableObject existan
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            ShopItemSO scriptableObject = AssetDatabase.LoadAssetAtPath<ShopItemSO>(soPath);

            if(prefab == null) {
                Debug.LogError($"Prefab no encontrado: {prefabPath}");
                continue;
            }

            if(scriptableObject == null) {
                Debug.LogError($"ScriptableObject no encontrado: {soPath}");
                continue;
            }

            // Asignar el Prefab al ScriptableObject
            scriptableObject.prefabItem = prefab;

            // Marcar el ScriptableObject como modificado
            EditorUtility.SetDirty(scriptableObject);
        }

        // Guardar todos los cambios
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Prefabs asignados a ScriptableObjects correctamente.");
    }
}