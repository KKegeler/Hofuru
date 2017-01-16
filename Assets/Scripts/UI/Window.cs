using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour {

    public GameObject firstSelected;

    public static WindowManager manager;

    protected virtual void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    //Methode um auf das EventSystem zugreifen zu können
    protected EventSystem eventSystem
    {
        get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); }
    }

    public virtual void OnFocus()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    /// <summary>
    /// Grundsätzliche Fenster Öffnen Methode,kann überschrieben werden
    /// </summary>
    /// <param name="value"></param>
    public virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()
    {
        Display(false);
    }
}
