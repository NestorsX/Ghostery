using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public AudioSource PlayerSound; // источник звуков персонажа
    public AudioClip Boost; // звук, когда персонаж делает рывок

    public float MaxMoveSpeed = 8; // максимальная скорость персонажа

    private CharacterController controllerComponent; // компонент, нужный для передвижения персонажа

    private Vector3 moveSpeed; // вектор скорости персонажа (то есть в какую сторону он будет передвигаться)
    private float dashingTimeLeft; // таймер на рывок персонажа

    // --- Метод "Старт" вызывается один раз
    private void Start()
    {
        controllerComponent = GetComponent<CharacterController>(); // получаем компонент, нужный для передвижения
    }

    // --- Метод вызывается 1 раз за кадр
    private void Update()
    {
        if(GameController.isGameContinue) // Если игра не окончена
        {
            UpdateWalk(); // вызываем метод передвижения персонажа

            //проверка зажата ли кнопка рывка или нет (кнопки: shift или X)
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) Dash(false);
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) Dash(true);
        }
    }

    // --- Задержка между рывками
    private void Dash(bool holding)
    {
        if (dashingTimeLeft < (holding ? -.4f : -.2f)) // если задержка меньше указанного времени:
        {                    // если кнопка удерживается - задержка между рывком 0.2 секунды, если кнопку нажали один раз - задержка перед следующим рывком 0.4 секунды
            PlayerSound.PlayOneShot(Boost); // проигрываем звук рывка
            dashingTimeLeft = .3f; // ставим новую задержку
        }
    }

    // --- Метод передвижения персонажа
    private void UpdateWalk()
    {
        float ySpeed = moveSpeed.y; // сохраняем значение по y (вверх-вниз)
        moveSpeed.y = 0; // ставим значение по y вектору на 0
        if (dashingTimeLeft <= 0) // если задержка до рывка закончилась
        {
            Vector3 target = MaxMoveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized; // получаем вектор координат, куда передвинется персонаж
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300); // двигаем персонажа

            if (moveSpeed.magnitude > 0.1f) // если длина вектора больше 0.1
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed), Time.deltaTime * 720); // поворачиваем персонажа в нужную сторону
            }
        }
        else // если задержка до рывка еще не прошла
        {
            moveSpeed = MaxMoveSpeed * 5 * moveSpeed.normalized; // находим скорость персонажа
        }

        dashingTimeLeft -= Time.deltaTime; // уменьшаем задержку на секунду

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime; // находим скорость по вертикали
        controllerComponent.Move(moveSpeed * Time.deltaTime); // двигаем персонажа
    }
}