using UnityEngine;
using System.Collections;

namespace EasyMobile
{
    [System.Serializable]
    public class NotificationSettings
    {
        public bool IsAutoInit { get { return _autoInit; } }

        public float AutoInitDelay { get { return _autoInitDelay; } }

        public string OneSignalAppId { get { return _oneSignalAppId; } }

        // Auto-init config
        [SerializeField]
        private bool _autoInit = true;
        [SerializeField]
        private float _autoInitDelay = 0f;

        // App credentials
        [SerializeField]
        private string _oneSignalAppId;
    }
}

