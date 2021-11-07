using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioSource SoundPlayer;
    public AudioSource MusicPlayer;
    public AudioClip GameOverSound;
    public AudioClip TimePlus;
    bool isGameOverSoundPlayed;

    public GameObject TimerSlider; // Объект, содержащий компонент "слайдер"
    Slider slider; // сам компонент "слайдер"
    public GameObject TimerText; // Объект, содержащий текст со временем таймера
    TMPro.TMP_Text text; // сам текст со счетчиком оставшегося времени
    
    float timer; // таймер (числовое выражение оставшегося времени)
    public float timerStartValue; // стартовое значение таймера в секундах (например 30 или 100)

    public static bool isObjectTaken; // логическая переменная, определяющая подобрал ли игрок объект

    public GameObject enemyPrefab; // префаб спавнящегося объекта
    int enemyCount = 10; // число объектов, сколько их должно быть на сцене
    public static int currEnemyCount; // реально число объектов, находящихся на сцене


    public GameObject GameOver; // сообщение об окончании игры
    public static bool isGameContinue; // логическая переменная "продолжается ли игра"

    public GameObject GeneralTime; // Объект, содержащий текст с общим временем игры
    TMPro.TMP_Text generalTimeText; // сам текст со счетчиком общего времени игры
    float generalTimer;

    public GameObject TimeResult; // Объект, содержащий текст с результатом общего времени игры
    TMPro.TMP_Text TimeResultText; // сам текст с результатом общего времени игры


    // --- Метод "Старт" вызывается один раз
    void Start()
    {
        currEnemyCount = 0; // на сцене сейчас 0 объектов

        StartCoroutine(spawnEnemy()); // Вызываем событие со спавном объектов

        slider = TimerSlider.GetComponent<Slider>(); // получаем компонент "Слайдер"
        text = TimerText.GetComponent<TMPro.TMP_Text>(); // получаем компонент "TextMeshPro" (счетчик оставшегося времени)

        slider.maxValue = timerStartValue; // устанавливаем максимальное значение слайдера
        slider.value = timerStartValue; //отодвигаем слайдер на максимум

        text.text = timerStartValue.ToString(); // выводим количество времени
        timer = timerStartValue; // запускаем таймер

        isGameContinue = true;
        isObjectTaken = false;
        isGameOverSoundPlayed = false;

        generalTimeText = GeneralTime.GetComponent<TMPro.TMP_Text>(); // получаем компонент "TextMeshPro" (счетчик общего времени)
        generalTimer = 0f;

        TimeResultText = TimeResult.GetComponent<TMPro.TMP_Text>(); // получаем компонент "TextMeshPro" (результат общего времени)

    }

    // --- Событие спавна объектов
    private IEnumerator spawnEnemy () {
        while(currEnemyCount<=enemyCount) // если кол-во объектов на сцене меньше, чем их должно быть
        {
            float xPos = Random.Range(-50f, 50f); // рандомная координата по X
            float zPos = Random.Range(-50f, 50f); // рандомная координата по Y
        
            // Спавним объект enemyPrefab в указанной позиции
            Instantiate(enemyPrefab, new Vector3(xPos, 1f, zPos), Quaternion.identity);
            currEnemyCount++; // говорим, что объектов стало больше
        }
        yield return new WaitForSeconds(0); // задержка - 0
    }

    // --- Метод вызывается 1 раз за кадр
    void Update()
    {
        if(isGameContinue) // если игра продолжается
        {
            if(currEnemyCount<enemyCount) // если кол-во объектов на сцене меньше, чем их должно быть
                StartCoroutine(spawnEnemy()); // Вызываем событие со спавном объектов
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.Cursor.visible = true;
                ExitToMainMenu();
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
        if (timer > 0) // если время таймера еще не вышло
        {
            timer -= Time.deltaTime; // уменьшаем значение таймера на тик
            UpdateSlider(); // Вызываем метод для изменения положения слайдера и вывода текста с кол-вом оставшегося времени
            generalTimer += Time.deltaTime;
            UpdateGeneralTimer();

        }
        else // если время закончилось
        {
            isGameContinue = false;
            GameOver.SetActive(true);
            int minutes = (int)generalTimer / 60;
            int seconds = (int)generalTimer % 60;
            TimeResultText.text = $"Результат: {minutes:00}:{seconds:00}";
            if(PlayerPrefs.GetInt("BestMinutes")<=minutes)
            {
                PlayerPrefs.SetInt("BestMinutes", minutes);
                if(PlayerPrefs.GetInt("BestMinutes")==minutes)
                {
                    if(PlayerPrefs.GetInt("BestSeconds")<=seconds)
                    {
                        PlayerPrefs.SetInt("BestSeconds", seconds);
                    }
                }
            }
            if(!isGameOverSoundPlayed)
            {
                MusicPlayer.Stop();
                isGameOverSoundPlayed = true;
                SoundPlayer.PlayOneShot(GameOverSound);
            }
        }
    }

    // --- метод для изменения положения слайдера и вывода текста с кол-вом оставшегося времени
    void UpdateSlider()
    {
        slider.value = timer; // меняем значение слайдера
        text.text = timer.ToString("#"); // выводим время без дробной части (Нпример: 28.97 => 28)
    }

    void UpdateGeneralTimer()
    {
        int minutes = (int)generalTimer / 60;
        int seconds = (int)generalTimer % 60;
        generalTimeText.text = $"Время: {minutes:00}:{seconds:00}"; // выводим время без дробной части (Нпример: 28.97 => 28)
    }

    // Событие срабатывает, если кто-то входит в коллайдер (коллайдер должен быть триггером)
    private void OnTriggerEnter(Collider collider) 
    {
        if(TakableObjects.currObj!=null) // если объект существует
        {
            SoundPlayer.PlayOneShot(TimePlus);
            Destroy(TakableObjects.currObj); // уничтожаем объект
            GameController.currEnemyCount--; // говорим, что объектов на сцене стало меньше
            isObjectTaken = false; // логическая переменная, определяющая подобрал ли игрок объект
            timer += 10f; // увеличиваем значение таймера
            if(timer>timerStartValue) // если текущее значение больше начального
                timer = timerStartValue; // меням время в таймере на начальное
            UpdateSlider(); // вызываем метод для изменения положения слайдера и вывода текста с кол-вом оставшегося времени
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        GameOver.SetActive(false);
        isGameContinue = true;
        SceneManager.LoadScene(1);
    }
}
