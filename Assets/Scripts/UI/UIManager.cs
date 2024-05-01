using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIScreen[] screens;
    public UIScreen currentScreen;
    public int currentScreenIndex = 0;

    public void Start() 
    {
        ShowScreen(0);
    }

    public virtual void ShowScreen(int index) 
    {
        if (index >= 0 && index < screens.Length) 
        {
            HideScreen();
            currentScreenIndex = index;
            currentScreen = screens[currentScreenIndex];
            currentScreen.OnPreShow();
            currentScreen.Show();
            currentScreen.OnPostShow();
        }
    }

    public virtual void HideScreen() 
    {
        if (currentScreen != null) 
        {
            currentScreen.OnPreHide();
            currentScreen.Hide();
            currentScreen.OnPostHide();
        }
    }
}
