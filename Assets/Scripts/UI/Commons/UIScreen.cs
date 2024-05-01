using UnityEngine;
using UnityEngine.Events;

public abstract class UIScreen : MonoBehaviour
{
    public UnityEvent
        postShowCallBack, 
        preShowCallBack, 
        postHideCallBack, 
        preHideCallBack;

    public virtual void OnPostShow() => postShowCallBack.Invoke();
    public virtual void OnPreShow() => preShowCallBack.Invoke();
    public virtual void OnPostHide() => postHideCallBack.Invoke();
    public virtual void OnPreHide() => preHideCallBack.Invoke();

    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);
}
