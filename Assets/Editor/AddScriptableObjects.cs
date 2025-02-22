using UnityEngine;
using UnityEditor;
using System.Linq;

public class PrefabScriptableAssigner : EditorWindow {
    private string prefabFolder = "Assets/Prefabs";
    private string scriptableObjectFolder = "Assets/ScriptableObjects";
    private GameObject[] prefabs;
    private ShopItemSO[] scriptableObjects;

    [MenuItem("Tools/Prefab-SO Assigner")]
    public static void ShowWindow() {
        GetWindow<PrefabScriptableAssigner>("Prefab-SO Assigner");
    }

    void OnGUI() {
        GUILayout.Label("Configuración", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Selecciona las carpetas donde están los Prefabs y los ScriptableObjects.", MessageType.Info);

        prefabFolder = EditorGUILayout.TextField("Carpeta de Prefabs:", prefabFolder);
        scriptableObjectFolder = EditorGUILayout.TextField("Carpeta de ScriptableObjects:", scriptableObjectFolder);

        if(GUILayout.Button("Buscar y Asignar")) {
            BuscarYAsignar();
        }

        if(prefabs != null && scriptableObjects != null) {
            GUILayout.Space(10);
            GUILayout.Label($"Encontrados: {prefabs.Length} Prefabs y {scriptableObjects.Length} ScriptableObjects", EditorStyles.helpBox);
        }
    }

    void BuscarYAsignar() {
        prefabs = BuscarAssets<GameObject>(prefabFolder);
        scriptableObjects = BuscarAssets<ShopItemSO>(scriptableObjectFolder);

        if(prefabs.Length != scriptableObjects.Length) {
            Debug.LogError("Las listas no tienen la misma cantidad de elementos.");
            return;
        }

        for(int i = 0; i < prefabs.Length; i++) {
            string path = AssetDatabase.GetAssetPath(prefabs[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            PrefabUtility.InstantiatePrefab(prefab);

            var script = prefab.GetComponent<ItemData>(); // Busca el script específico
            if(script != null) {
                Undo.RecordObject(script, "Asignar ScriptableObject");
                script.shopItemSO = scriptableObjects[i]; // Asigna el ScriptableObject
                EditorUtility.SetDirty(script);
            } else {
                Debug.LogWarning($"El prefab {prefabs[i].name} no tiene un script que contenga 'myData'.");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Asignación completa.");
    }

    T[] BuscarAssets<T>(string folder) where T : UnityEngine.Object {
        if(!AssetDatabase.IsValidFolder(folder)) {
            Debug.LogError($"La carpeta {folder} no existe.");
            return new T[0];
        }

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folder });
        return guids.Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<T>)
                    .OrderBy(a => a.name)
                    .ToArray();
    }
}