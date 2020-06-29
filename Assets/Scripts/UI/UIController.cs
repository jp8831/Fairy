using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Dictionary<string, UIElement> m_uiMap;

    private void Awake()
    {
        m_uiMap = new Dictionary<string, UIElement>();
    }

    public void RegisterUI (UIElement ui)
    {
        if (m_uiMap == null)
        {
            return;
        }

        if (string.IsNullOrEmpty (ui.ElementID))
        {
            Debug.LogWarning($"UI ID is invalid!");
            return;
        }

        UIElement found;

        if (m_uiMap.TryGetValue (ui.ElementID, out found))
        {
            Debug.LogWarning($"{ui.ElementID} already exist!");
            return;
        }

        m_uiMap.Add(ui.ElementID, ui);
    }

    public void UnregisterUI(UIElement ui)
    {
        if (m_uiMap == null)
        {
            return;
        }

        if (m_uiMap.ContainsKey (ui.ElementID))
        {
            m_uiMap.Remove(ui.ElementID);
        }
    }

    public TUI FindUI<TUI> (string id) where TUI : UIElement
    {
        TUI found = null;

        if (string.IsNullOrEmpty(id) == false)
        {
            if (m_uiMap.ContainsKey(id))
            {
                UIElement element = m_uiMap[id];
                found = element as TUI;
            }
        }

        return found;
    }

    public void ActivateUI (string id)
    {
        var found = FindUI<UIElement>(id);

        if (found)
        {
            found.gameObject.SetActive(true);
        }
    }

    public void DeactivateUI(string id)
    {
        var found = FindUI<UIElement>(id);

        if (found)
        {
            found.gameObject.SetActive(false);
        }
    }
}
