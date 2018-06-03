using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EasyMobile.Demo
{
    public class DemoUtils : MonoBehaviour
    {
        public Sprite checkedSprite;
        public Sprite uncheckedSprite;

        public void GoHome()
        {
            SceneManager.LoadScene("DemoHome");
        }

        public void DisplayBool(GameObject infoObj, bool state, string msg)
        {
            Image img = infoObj.GetComponentInChildren<Image>();
            Text txt = infoObj.GetComponentInChildren<Text>();

            if (img == null || txt == null)
            {
                Debug.LogError("Could not found Image or Text component beneath object: " + infoObj.name);
            }

            if (state)
            {
                img.sprite = checkedSprite;
                img.color = Color.green;
            }
            else
            {
                img.sprite = uncheckedSprite;
                img.color = Color.red;
            }

            txt.text = msg;
        }

        public void PlayButtonSound()
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        }
    }
}
