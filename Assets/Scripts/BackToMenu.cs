using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;

    public void ReturnToMenu()
    {
        Menu.SetActive(true);
        Settings.SetActive(false);
        PauseMenu.isSettingsVisible = false;
    }
}
