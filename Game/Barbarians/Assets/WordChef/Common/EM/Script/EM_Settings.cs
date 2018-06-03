using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace EasyMobile
{
    public class EM_Settings : ScriptableObject
    {
        #region Public members

        public static EM_Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadSettingsAsset();

                    if (_instance == null)
                    {
                        #if !UNITY_EDITOR
                        Debug.LogError("Easy Mobile settings not found! " +
                            "Please go to Tools>Easy Mobile>Settings to setup the plugin.");
                        #endif
                        _instance = CreateInstance<EM_Settings>();   // Create a dummy scriptable object for temporary use.
                    }
                }

                return _instance;
            }
        }

        // Module settings
        public static NotificationSettings Notification { get { return Instance._notificationSettings; } }

        // Module toggles
        public static bool IsNotificationModuleEnable { get { return Instance._isNotificationModuleEnable; } }

        public static EM_Settings LoadSettingsAsset()
        {
            return Resources.Load("EM_Settings") as EM_Settings;
        }

        #endregion

        #region Private members

        private static EM_Settings _instance;

        [SerializeField]
        private NotificationSettings _notificationSettings;

        [SerializeField]
        private bool _isNotificationModuleEnable = true;

        #endregion

        #region Editor stuff

        #if UNITY_EDITOR
        // Index of the active module on the toolbar.
        // This field is only used as a SerializedProperty in the editor scripts, hence the warning suppression.
        #pragma warning disable 0414
        [SerializeField]
        private int _activeModuleIndex = 0;
        #pragma warning restore 0414

        #endif

        #endregion
    }
}

