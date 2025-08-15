using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    public void Close()
    {
        OnClose();
        gameObject.SetActive(false);
    }
    
    protected virtual void OnOpen(){}
    protected virtual void OnClose(){}
}