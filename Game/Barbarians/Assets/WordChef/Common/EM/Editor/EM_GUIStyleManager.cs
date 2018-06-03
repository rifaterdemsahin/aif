using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace EasyMobile.Editor
{
    public static class EM_GUIStyleManager
    {
        #region Constants

        // Common button sizes.
        public static readonly float buttonHeight = EditorGUIUtility.singleLineHeight * 1.3f;
        public static readonly float smallButtonWidth = 24f;
        public static readonly float smallButtonHeight = 24f;
        public static readonly float toolboxWidth = 34;
        public static readonly float toolboxHeight = 86;

        #endregion

        #region GUISkin

        private static GUISkin _skin;

        public static GUISkin Skin
        {
            get
            {
                if (_skin != null)
                {
                    return _skin;
                }
                else
                {
                    string skinName = EditorGUIUtility.isProSkin ? "EMSkin_Dark.guiskin" : "EMSkin_Light.guiskin";
                    string skinPath = EM_Constants.SkinFolder + "/" + skinName;
                    _skin = AssetDatabase.LoadAssetAtPath(skinPath, typeof(GUISkin)) as GUISkin;

                    if (_skin == null)
                    {
                        Debug.LogError("Couldn't load the GUISkin at " + skinPath);
                    }

                    return _skin;
                }
            }
        }

        #endregion

        #region GUIStyles

        // Module toobar button
        private static GUIStyle _moduleToolbarButton;

        public static GUIStyle ModuleToolbarButton
        {
            get
            {
                if (_moduleToolbarButton == null)
                {
                    _moduleToolbarButton = new GUIStyle(EditorStyles.toolbarButton);
                    _moduleToolbarButton.fontSize = 10;
                    _moduleToolbarButton.fontStyle = FontStyle.Bold;
                    _moduleToolbarButton.fixedHeight = EditorGUIUtility.singleLineHeight * 2;
                    _moduleToolbarButton.stretchWidth = true;
                }
                return _moduleToolbarButton;
            }
        }

        // Module toobar button left
        private static GUIStyle _moduleToolbarButtonLeft;

        public static GUIStyle ModuleToolbarButtonLeft
        {
            get
            {
                if (_moduleToolbarButtonLeft == null)
                {
                    _moduleToolbarButtonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
                    _moduleToolbarButtonLeft.fontSize = 10;
                    _moduleToolbarButtonLeft.fontStyle = FontStyle.Bold;
                    _moduleToolbarButtonLeft.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;
                    _moduleToolbarButtonLeft.stretchWidth = true;
                }
                return _moduleToolbarButtonLeft;
            }
        }

        // Module toolbar button middle
        private static GUIStyle _moduleToolbarButtonMid;

        public static GUIStyle ModuleToolbarButtonMid
        {
            get
            {
                if (_moduleToolbarButtonMid == null)
                {
                    _moduleToolbarButtonMid = new GUIStyle(EditorStyles.miniButtonMid);
                    _moduleToolbarButtonMid.fontSize = 10;
                    _moduleToolbarButtonMid.fontStyle = FontStyle.Bold;
                    _moduleToolbarButtonMid.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;
                    _moduleToolbarButtonMid.stretchWidth = true;
                }
                return _moduleToolbarButtonMid;
            }
        }

        // Module toolbar button right
        private static GUIStyle _moduleToolbarButtonRight;

        public static GUIStyle ModuleToolbarButtonRight
        {
            get
            {
                if (_moduleToolbarButtonRight == null)
                {
                    _moduleToolbarButtonRight = new GUIStyle(EditorStyles.miniButtonRight);
                    _moduleToolbarButtonRight.fontSize = 10;
                    _moduleToolbarButtonRight.fontStyle = FontStyle.Bold;
                    _moduleToolbarButtonRight.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;
                    _moduleToolbarButtonRight.stretchWidth = true;
                }
                return _moduleToolbarButtonRight;
            }
        }

        // Module foldout
        private static GUIStyle _moduleFoldout;

        public static GUIStyle ModuleFoldout
        {
            get
            {
                if (_moduleFoldout == null)
                {
                    _moduleFoldout = new GUIStyle(EditorStyles.foldout);  // Extending default foldout style.
                    _moduleFoldout.fontSize = 14;
                    _moduleFoldout.fontStyle = FontStyle.Bold;
                    _moduleFoldout.alignment = TextAnchor.MiddleLeft;
                    _moduleFoldout.normal.textColor = Color.black;
                }
                return _moduleFoldout;
            }
        }

        // Textfield with wordwrap
        private static GUIStyle _wordwrapTextField;

        public static GUIStyle WordwrapTextField
        {
            get
            {
                if (_wordwrapTextField == null)
                {
                    _wordwrapTextField = new GUIStyle(EditorStyles.textField);  // Extending default textfield style.
                    _wordwrapTextField.wordWrap = true;
                }
                return _wordwrapTextField;
            }
        }

        #endregion

        #region Textures

        private static Texture2D _adIcon;

        public static Texture2D AdIcon
        {
            get
            {
                if (_adIcon == null)
                {
                    string iconName = EditorGUIUtility.isProSkin ? "ad-icon-dark.psd" : "ad-icon-light.psd";
                    _adIcon = AssetDatabase.LoadAssetAtPath(EM_Constants.SkinTextureFolder + "/" + iconName, typeof(Texture2D)) as Texture2D;
                }
                return _adIcon;
            }
        }

        private static Texture2D _iapIcon;

        public static Texture2D IAPIcon
        {
            get
            {
                if (_iapIcon == null)
                {
                    string iconName = EditorGUIUtility.isProSkin ? "iap-icon-dark.psd" : "iap-icon-light.psd";
                    _iapIcon = AssetDatabase.LoadAssetAtPath(EM_Constants.SkinTextureFolder + "/" + iconName, typeof(Texture2D)) as Texture2D;
                }
                return _iapIcon;
            }
        }

        private static Texture2D _gameServiceIcon;

        public static Texture2D GameServiceIcon
        {
            get
            {
                if (_gameServiceIcon == null)
                {
                    string iconName = EditorGUIUtility.isProSkin ? "game-service-icon-dark.psd" : "game-service-icon-light.psd";
                    _gameServiceIcon = AssetDatabase.LoadAssetAtPath(EM_Constants.SkinTextureFolder + "/" + iconName, typeof(Texture2D)) as Texture2D;
                }
                return _gameServiceIcon;
            }
        }

        private static Texture2D _notificationIcon;

        public static Texture2D NotificationIcon
        {
            get
            {
                if (_notificationIcon == null)
                {
                    string iconName = EditorGUIUtility.isProSkin ? "notification-icon-dark.psd" : "notification-icon-light.psd";
                    _notificationIcon = AssetDatabase.LoadAssetAtPath(EM_Constants.SkinTextureFolder + "/" + iconName, typeof(Texture2D)) as Texture2D;
                }
                return _notificationIcon;
            }
        }

        #endregion

        #region Methods

        private static Dictionary<string,GUIStyle> _customStyles = new Dictionary<string,GUIStyle>();

        public static GUIStyle GetCustomStyle(string styleName)
        {
            if (_customStyles.ContainsKey(styleName))
            {
                return _customStyles[styleName];
            }
            else if (Skin != null)
            {
                GUIStyle style = Skin.FindStyle(styleName);

                if (style == null)
                {
                    Debug.LogError("Couldn't find style " + styleName);
                }
                else
                {
                    _customStyles.Add(styleName, style);
                }

                return style;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}

