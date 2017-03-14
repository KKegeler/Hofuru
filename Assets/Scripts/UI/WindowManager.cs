using UnityEngine;

public class WindowManager : MonoBehaviour {

    public Window[] windows;
    public int currentWindowID;
    public int defaultWindowID;

    // Use this for initialization
    void Start () {
        Window.manager = this;
        Open(defaultWindowID);
    }

    //Return das derzeitig offene Window
    public Window GetWindow(int value)
    {
        return windows[value];
    }

    //sicherstellung,dass nur 1 Fenster zur Zeit an ist
    private void ToggleVisibility(int value)
    {
        var total = windows.Length;

        //geht über jedes fenster und öffnet oder schließt es
        for (var i = 0; i < total; i++)
        {
            var window = windows[i];
            if (i == value)
                window.Open();
            else if (window.gameObject.activeSelf)
                window.Close();
        }
    }

    //Fenster öffnen
    public Window Open(int value)
    {
        currentWindowID = value;
        ToggleVisibility(currentWindowID);

        return GetWindow(currentWindowID);
    }
}
