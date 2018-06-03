using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0067
namespace EasyMobile
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }

        /// <summary>
        /// Fired when notification is opened. The event handler should have the signature of
        /// 
        /// void OnNotificationOpened(string message, string actionID, Dictionary<string, object> additionalData, bool isAppInFocus)
        /// 
        /// In which:
        /// - message: the body message of the notification.
        /// - actionID: the id on the button the user pressed, actionID will equal "__DEFAULT__" when the notification itself was tapped when buttons were present.
        /// - additionalData: additional data of the notification.
        /// - isAppInFocus: was app in focus when the notification was opened. Normally you should take actions (e.g take user to your store) only when this value is FALSE
        /// for not interrupting user experience.
        /// </summary>
        public static event System.Action<string, string, Dictionary<string, object>, bool> NotificationOpened = null;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            if (EM_Settings.Notification.IsAutoInit)
            {
                StartCoroutine(CRAutoInit(EM_Settings.Notification.AutoInitDelay));
            }
        }

        IEnumerator CRAutoInit(float delay)
        {
            yield return new WaitForSeconds(delay);            
            Init();
        }

        /// <summary>
        /// Init the push notification service after an optional delay time.
        /// </summary>
        public static void Init()
        {
            #if EM_ONESIGNAL
            // The only required method you need to call to setup OneSignal to recieve push notifications.
            // Call before using any other methods on OneSignal.
            // Should only be called once when your game is loaded.
            OneSignal.StartInit(EM_Settings.Notification.OneSignalAppId)
                .HandleNotificationOpened(HandleNotificationOpened)
                .InFocusDisplaying(OneSignal.OSInFocusDisplayOption.None)
                .EndInit();
            #else
            if (Debug.isDebugBuild)
                Debug.LogError("SDK missing. Please import OneSignal plugin for Unity.");
            #endif
        }

        #if EM_ONESIGNAL
        // Called when a notification is opened.
        // The name of the method can be anything as long as the signature matches.
        // Method must be static or this object should be marked as DontDestroyOnLoad
        private static void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            OSNotificationPayload payload = result.notification.payload;
            Dictionary<string, object> additionalData = payload.additionalData;
            string message = payload.body;
            string actionID = result.action.actionID;
            bool isAppInFocus = result.notification.isAppInFocus;

            // Fire event
            if (NotificationOpened != null)
                NotificationOpened(message, actionID, additionalData, isAppInFocus);
        }
        #endif
    }
}