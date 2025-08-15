using UnityEngine;
using UnityEngine.UI;

public class UIDim : UIBase
{
    public static void Set(string text)
    {
        var instance = UIManager.Instance.Open<UIDim>();
        instance.label.text = text;
    }

    public static void Release()
    {
        UIManager.Instance.Close<UIDim>();
    }
    
    [SerializeField] private Text label;

    protected override void OnClose()
    {
        label.text = null;
    }
}