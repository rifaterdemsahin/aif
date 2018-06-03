using UnityEngine;
using System.Collections;

namespace EasyMobile
{
    public class EM_PrefabManager : MonoBehaviour
    {
        public static EM_PrefabManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject); @dongtp
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}

