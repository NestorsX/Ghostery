using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource SoundPlayer; // источник звука для меню паузы
    public AudioClip PauseOn; // звук появления меню паузы
    public AudioClip PauseOff; // звук исчезновения меню паузы

    public GameObject pauseMenu; // Объект, содержащий меню паузы
    public GameObject UI; // объект, содержащий интерфейс
    public GameObject settings; // объект, содержащий меню настроек

    public static bool GameIsPaused = false; // логическая переменная: "остановлена ли игра"
    
    public static bool isSettingsVisible = false; // логическая переменная: "открыто ли меню настроек"

    // --- Метод вызывается 1 раз за кадр
    void Update()
    {
        if(!isSettingsVisible) // если меню настроек не открыто
        {
            if(GameController.isGameContinue) // и если игра еще продолжается
            {
                if (Input.GetKeyDown(KeyCode.Escape)) // если нажали кнопку Esc
                {
                    if(GameIsPaused) // если игра остановлена на паузу
                    {
                        ResumeGame(); // вызываем метод, который снимает игру с паузы
                    }
                    else // если игра не остановлена на паузу
                    {
                        PauseGame(); // вызываем метод, останавливающий игру на паузу
                    }
                }
            }
        }
    }

    // --- Метод снимает игру с паузы
    public void ResumeGame()
    {
        UnityEngine.Cursor.visible = false;
        pauseMenu.SetActive(false); // прячем окно паузы
        UI.SetActive(true); // показываем интерфейс
        Time.timeScale = 1f; // восстанавливаем время
        GameIsPaused = false; // говорим, что игра не на паузе
        SoundPlayer.PlayOneShot(PauseOff); // проигрываем звук снятия игры с паузы
    }

    // --- Метод ставит игру на паузу
    public void PauseGame()
    {
        UnityEngine.Cursor.visible = true;
        pauseMenu.SetActive(true); // показываем меню паузы
        UI.SetActive(false); // прячем интерфейс
        Time.timeScale = 0f; // останавливаем время
        GameIsPaused = true; // говорим, что игра на паузе
        SoundPlayer.PlayOneShot(PauseOn); // проигрываем звук постановки игры на паузу
    }

    // --- Метод открывает меню настроек
    public void Settings()
    {
        isSettingsVisible = true; // говорим, что мы зашли в меню настроек
        settings.SetActive(true); // показываем меню настроек
        pauseMenu.SetActive(false); // прячем меню паузы
    }

    // --- Метод выходит из игры в главное меню
    public void ExitToMainMenu()
    {
        GameIsPaused = false; // говорим, что игра снята с паузы
        Time.timeScale = 1f; // восстанавливаем время
        SceneManager.LoadScene(0); // выходим в главное меню
    }
}
