using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EasyMobile.Editor
{
    // Partial editor class for Notification module.
    public partial class EM_SettingsEditor
    {
        const string NotificationModuleLabel = "NOTIFICATION";
        const string NotificationModuleIntro = "Notification module simplifies the implementation of push notification in your game. It is compatible with OneSignal service (free).";
        const string OneSignalImportInstruction = "OneSignal plugin is required. Please click the button below to download and import it.";
        const string OneSignalAvailMsg = "OneSignal plugin is imported and ready to use.";
        const string NotificationManualInitInstruction = "You can initialize the module manually from script by calling NotificationManager.Instance.Init() method.";

        void NotificationModuleGUI()
        {
            EditorGUILayout.BeginVertical(EM_GUIStyleManager.GetCustomStyle("Module Box"));

            EditorGUI.BeginChangeCheck();
            isNotificationModuleEnable.boolValue = EM_EditorGUI.ModuleToggle(isNotificationModuleEnable.boolValue, NotificationModuleLabel);
            if (EditorGUI.EndChangeCheck())
            {
                // Update the main prefab according to the toggle state.
                GameObject prefab = EM_EditorUtil.GetMainPrefab();

                if (!isNotificationModuleEnable.boolValue)
                {
                    EM_Manager.DisableNotificationModule(prefab);
                }
                else
                {
                    EM_Manager.EnableNotificationModule(prefab);
                }
            }

            // Now draw the GUI.
            if (!isNotificationModuleEnable.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(NotificationModuleIntro, MessageType.Info);
            }
            else
            {
                #if !EM_ONESIGNAL
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(OneSignalImportInstruction, MessageType.Error);
                EditorGUILayout.Space();
                if (GUILayout.Button("Download OneSignal Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                {
                    EM_ExternalPluginManager.DownloadOneSignalPlugin();
                }
                #else
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(OneSignalAvailMsg, MessageType.Info);
                EditorGUILayout.Space();
                if (GUILayout.Button("Download OneSignal Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                {
                    EM_ExternalPluginManager.DownloadOneSignalPlugin();
                }

                // OneSignal setup
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ONESIGNAL SETUP", EditorStyles.boldLabel);
                NotificationProperties.oneSignalAppId.property.stringValue = EditorGUILayout.TextField(NotificationProperties.oneSignalAppId.content, NotificationProperties.oneSignalAppId.property.stringValue);

                // Auto-init setup
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("AUTO-INIT CONFIG", EditorStyles.boldLabel);
                NotificationProperties.autoInit.property.boolValue = EditorGUILayout.Toggle(NotificationProperties.autoInit.content, NotificationProperties.autoInit.property.boolValue);

                // Auto init delay
                EditorGUI.BeginDisabledGroup(!NotificationProperties.autoInit.property.boolValue);
                NotificationProperties.autoInitDelay.property.floatValue = EditorGUILayout.FloatField(NotificationProperties.autoInitDelay.content, NotificationProperties.autoInitDelay.property.floatValue);
                EditorGUI.EndDisabledGroup();

                // Init tip
                if (!NotificationProperties.autoInit.property.boolValue)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox(NotificationManualInitInstruction, MessageType.Info);
                }
                #endif
            }

            EditorGUILayout.EndVertical();
        }
    }
}