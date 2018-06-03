using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SgLib.Editor;

namespace EasyMobile.Editor
{
    [InitializeOnLoad]
    public class EM_Manager : AssetPostprocessor
    {
        // This static constructor will automatically run thanks to the InitializeOnLoad attribute.
        static EM_Manager()
        {
            EditorApplication.update += Initialize;
        }

        // This is called by Unity after importing of any number of assets is complete.
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // Here we know which files have been updated.
        }

        private static void Initialize()
        {
            EditorApplication.update -= Initialize;

            // Import the native package if it has been imported before.
            if (!IsNativeCodeImported())
            {
                ImportNativeCode();
            }

            // Define a global symbol indicating the existence of EasyMobile
            GlobalDefineManager.SDS_AddDefine(EM_ScriptingSymbols.EasyMobile, EditorUserBuildSettings.selectedBuildTargetGroup);

            // Create the EM_Settings scriptable object if it doesn't exist.
            CreateSettingsAsset();

            // Create the EasyMobile prefab if it doesn't exist.
            CreateMainPrefab();

            // Regularly check for module prerequisites to avoid issues caused
            // by inadvertent changes, e.g remove components from prefab or delete scripting symbol.
            CheckModules();
        }

        [MenuItem("Window/Easy Mobile/Settings", false)]
        public static void MenuOpenSettings()
        {
            EM_Settings instance = EM_Settings.LoadSettingsAsset();

            if (instance == null)
            {
                instance = CreateSettingsAsset();
            }

            Selection.activeObject = instance;
        }

        [MenuItem("Window/Easy Mobile/Create EasyMobile.prefab", false)]
        public static void MenuCreateMainPrefab()
        {
            CreateMainPrefab(true);
            CheckModules();
        }

        [MenuItem("Window/Easy Mobile/Documentation", false)]
        public static void OpenDocumentation()
        {
            Application.OpenURL(EM_Constants.DocumentationURL);
        }

        private static EM_Settings CreateSettingsAsset()
        {
            // Stop if the asset is already created.
            EM_Settings instance = EM_Settings.LoadSettingsAsset();
            if (instance != null)
            {
                return instance;
            }

            // Create Resources folder if it doesn't exist.
            FileIO.EnsureFolderExists(EM_Constants.ResourcesFolder);

            // Now create the asset inside the Resources folder.
            instance = EM_Settings.Instance; // this will create a new instance of the EMSettings scriptable.
            AssetDatabase.CreateAsset(instance, EM_Constants.SettingsAssetPath);    
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("EM_Settings.asset is created at " + EM_Constants.SettingsAssetPath);

            return instance;
        }

        private static GameObject CreateMainPrefab(bool showAlert = false)
        {
            // Stop if the prefab is already created.
            string prefabPath = EM_Constants.MainPrefabPath;
            GameObject existingPrefab = EM_EditorUtil.GetMainPrefab();

            if (existingPrefab != null)
            {
                if (showAlert)
                {
                    EM_EditorUtil.Alert("Prefab Exists", "EasyMobile.prefab already exists at " + prefabPath);
                }

                return existingPrefab;
            }

            // Make sure the containing folder exists.
            FileIO.EnsureFolderExists(EM_Constants.MainPrefabFolder);

            // Create a temporary gameObject and then create the prefab from it.
            GameObject tmpObj = new GameObject(EM_Constants.MainPrefabName);

            // Add PrefabManager component.
            tmpObj.AddComponent<EM_PrefabManager>();

            // Generate the prefab from the temporary game object.
            GameObject prefabObj = PrefabUtility.CreatePrefab(prefabPath, tmpObj);
            GameObject.DestroyImmediate(tmpObj);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (showAlert)
            {
                EM_EditorUtil.Alert("Prefab Created", "EasyMobile.prefab is created at " + prefabPath);
            }
            else
            {
                Debug.Log("EasyMobile.prefab is created at " + prefabPath);
            }

            return prefabObj;
        }

        private static bool IsNativeCodeImported()
        {
            return EM_ProjectSettings.Instance.GetBool(EM_Constants.PSK_ImportedNativeCode, false);
        }

        private static void ImportNativeCode()
        {
            AssetDatabase.ImportPackage(EM_Constants.NativePackagePath, false);
            EM_ProjectSettings.Instance.Set(EM_Constants.PSK_ImportedNativeCode, true);
        }

        // Makes that everything is set up properly so that all modules function as expected.
        internal static void CheckModules()
        {
            GameObject mainPrefab = EM_EditorUtil.GetMainPrefab();

            // Notification module
            if (EM_Settings.IsNotificationModuleEnable)
            {
                EnableNotificationModule(mainPrefab);
            }
            else
            {
                DisableNotificationModule(mainPrefab);
            }
        }

        internal static void EnableNotificationModule(GameObject mainPrefab)
        {
            EM_EditorUtil.AddModuleToPrefab<NotificationManager>(mainPrefab);

            // Check if OneSignal is available.
            bool isOneSignalAvail = EM_ExternalPluginManager.IsOneSignalAvail();
            if (isOneSignalAvail)
            {
                GlobalDefineManager.SDS_AddDefine(EM_ScriptingSymbols.OneSignal, EditorUserBuildSettings.selectedBuildTargetGroup);
            }
        }

        internal static void DisableNotificationModule(GameObject mainPrefab)
        {
            EM_EditorUtil.RemoveModuleFromPrefab<NotificationManager>(mainPrefab);

            // Remove associated scripting symbol on all platforms it was defined.
            GlobalDefineManager.SDS_RemoveDefineOnAllPlatforms(EM_ScriptingSymbols.OneSignal);
        }
    }
}
