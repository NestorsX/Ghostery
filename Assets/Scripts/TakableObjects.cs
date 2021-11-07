using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakableObjects : MonoBehaviour
{
    public AudioSource SoundPlayer;
    public AudioClip Take;


    GameObject playerHand;
    public static GameObject currObj;

    public bool isTaken;

    void Start()
    {
        playerHand = GameObject.Find("/Player/Hand");
        SoundPlayer = GameObject.Find("/Main Camera/SoundPlayer").GetComponent<AudioSource>();
        StartCoroutine(selfDestroying());
    }

    private IEnumerator selfDestroying () {
        yield return new WaitForSeconds(10f);
        if(gameObject!=null && !isTaken)
        {
            Destroy(gameObject); // уничтожаем объект
            GameController.currEnemyCount--; // говорим, что объектов на сцене стало меньше
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            MissObject();
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if(collider.gameObject.tag == "Player")
                TakeObject();
        }
    }

    void TakeObject()
    {
        if(!GameController.isObjectTaken)
        {
            SoundPlayer.PlayOneShot(Take);
            isTaken = true;
            currObj = gameObject;
            GameController.isObjectTaken = true;
            gameObject.transform.parent = playerHand.transform;
            gameObject.transform.position = playerHand.transform.position;
        }
    }

    void MissObject()
    {
        isTaken = false;
        gameObject.transform.parent = null;
        currObj = null;
        Invoke("Taken", 0.5f);
    }

    void Taken()
    {
        GameController.isObjectTaken = false;
    }
}
