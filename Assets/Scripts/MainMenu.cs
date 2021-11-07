using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Menu; // Объект, содержащий главное меню
    public GameObject SettingsScreen; // Объект, содержащий меню настроек
    public GameObject AboutGameScreen; // Объект, содержащий информацию об игре

    public TMPro.TMP_Text BestResult;

    void Start()
    {
        BestResult.text = $"Лучшее время: {PlayerPrefs.GetInt("BestMinutes"):00}:{PlayerPrefs.GetInt("BestSeconds"):00}";
    }

    // -- Метод вызывается, когда нажимаем кнопку "Начать игру"
    public void Play()
    {
        UnityEngine.Cursor.visible = false; // прячем курсор 
        SceneManager.LoadScene(1); // загружаем игру (переходим на сцену с уровнем)
    }

    // -- Метод вызывается, когда нажимаем кнопку "Настройки"
    public void Settings()
    {
        Menu.SetActive(false); // скрываем главное меню
        SettingsScreen.SetActive(true); // показываем меню настроек
    }

    // -- Метод вызывается, когда нажимаем кнопку "Об игре"
    public void About()
    {
        Menu.SetActive(false); // скрываем главное меню
        AboutGameScreen.SetActive(true); // показываем информацию об игре
    }

    // -- Метод вызывается, когда нажимаем кнопку "Выход"
    public void Exit()
    {
        Application.Quit(); // выходим из игры
    }

}
