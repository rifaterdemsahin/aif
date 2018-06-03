using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SgLib.Editor;

namespace EasyMobile.Editor
{
    [CustomEditor(typeof(EM_Settings))]
    public partial class EM_SettingsEditor : UnityEditor.Editor
    {
        #region Modules

        public enum Module
        {
            Notification = 0
        }

        static Module activeModule = Module.Notification;

        #endregion

        #region Target properties

        // Module toggles
        SerializedProperty isNotificationModuleEnable;

        // Active module (currently selected on the toolbar)
        SerializedProperty activeModuleIndex;

        public class EMProperty
        {
            public SerializedProperty property;
            public GUIContent content;

            public EMProperty(SerializedProperty p, GUIContent c)
            {
                property = p;
                content = c;
            }
        }

        // Push Notification module properties
        private static class NotificationProperties
        {
            public static SerializedProperty mainProperty;
            public static EMProperty oneSignalAppId = new EMProperty(null, new GUIContent("OneSignal App Id", "The app Id obtained from OneSignal dashboard"));
            public static EMProperty autoInit = new EMProperty(null, new GUIContent("Auto Init", "Should the service automatically initialize on start"));
            public static EMProperty autoInitDelay = new EMProperty(null, new GUIContent("Auto Init Delay", "Delay time (seconds) after Start() that the service is automatically initialized"));
        }

        #endregion


        #region GUI

        void OnEnable()
        {
            if (serializedObject == null)
                Debug.Log("SerializedObject is NULL");
            
            // Module-control properties.
            isNotificationModuleEnable = serializedObject.FindProperty("_isNotificationModuleEnable");

            activeModuleIndex = serializedObject.FindProperty("_activeModuleIndex");

            if (System.Enum.IsDefined(typeof(Module), activeModuleIndex.intValue))
            {
                activeModule = (Module)activeModuleIndex.intValue;
            }

            // Notification module properties.
            NotificationProperties.mainProperty = serializedObject.FindProperty("_notificationSettings");
            NotificationProperties.oneSignalAppId.property = NotificationProperties.mainProperty.FindPropertyRelative("_oneSignalAppId");
            NotificationProperties.autoInit.property = NotificationProperties.mainProperty.FindPropertyRelative("_autoInit");
            NotificationProperties.autoInitDelay.property = NotificationProperties.mainProperty.FindPropertyRelative("_autoInitDelay");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();

            // Draw the module-select toolbar.
            EditorGUILayout.BeginHorizontal();
            EM_EditorGUI.ToolbarButton(new GUIContent(null, EM_GUIStyleManager.NotificationIcon, "Notification"), Module.Notification, ref activeModule, EditorGUIUtility.isProSkin ? EM_GUIStyleManager.ModuleToolbarButtonRight : EM_GUIStyleManager.ModuleToolbarButton);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Store the toolbar index value to the serialized settings file.
            activeModuleIndex.intValue = (int)activeModule;

            switch (activeModule)
            {
                case Module.Notification:
                    NotificationModuleGUI();
                    break;
            }

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
