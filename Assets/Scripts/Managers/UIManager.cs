using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            if (_instance == null)
            {
                GameObject go = new GameObject(name : "UIManager");
                _instance = go.AddComponent<UIManager>();
            }

            return _instance;
        }
    }
    #endregion

    private Dictionary<string, UIBase> _uiList = new();

    private string GetUIName<T>() => typeof(T).Name;

    public T Open<T>() where T : UIBase
    {
        string uiName = GetUIName<T>();

        if (_uiList.TryGetValue(uiName, out UIBase spawnedUI))
        {
            spawnedUI.Open();
            return spawnedUI as T;
        }

        GameObject prefab = Resources.Load<GameObject>($"UI/{uiName}");

        if (prefab == null)
            throw new($"[UIManager] Can not find ui prefab [{uiName}]");

        T ui = Instantiate(prefab).GetComponent<T>();
        _uiList.Add(uiName, ui);
        ui.Open();
        return ui;
    }

    public void Close<T>() where T : UIBase
    {
        string uiName = GetUIName<T>();

        if (_uiList.TryGetValue(uiName, out UIBase ui) == false)
            return;
        
        ui.Close();
    }

    public bool TryGet<T>(out T ui) where T : UIBase
    {
        ui = null;
        
        if (_uiList.TryGetValue(GetUIName<T>(), out UIBase spawnedUI) == false)
            return false;

        ui = spawnedUI as T;

        return true;
    }
}