using UnityEngine;
using UnityEngine.UI;
public class NotificationAlert : CMonoBehaviour
{
    public GameObject notificationObj;
    public Animator anim;
    public enum Type { taskComplete };
    public Type alertType;
    public float showAnimationTime = 5;

    protected virtual void Start()
    {
        if (anim == null ) anim = GetComponent<Animator>();
        bool isVisible = IsNotificationAlertVisible();
        notificationObj.SetActive(isVisible);
    }

    protected void OnAlertNotification()
    {
        notificationObj.SetActive(true);
        anim.SetBool("show", true);
        Invoke("OnAlertComplete", showAnimationTime);
    }

    protected void OnAlertComplete()
    {
        if (anim.isActiveAndEnabled)
        {
            anim.SetBool("show", false);
        }
    }

    public void OnHideNotification()
    {
        notificationObj.SetActive(false);
    }

    public virtual bool IsNotificationAlertVisible()
    {
        return false;
    }
}