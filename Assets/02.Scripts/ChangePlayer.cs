using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayer : MonoBehaviour
{
    //[Header("Skull")]
    //[SerializeField] GameObject Skull1;
    //[SerializeField] GameObject Skull2;
    //[SerializeField] csPlayer csPlayer1;
    //[SerializeField] csPlayer csPlayer2;
    [Header("Camera")]
    [SerializeField] GameObject camera1;
    [SerializeField] GameObject camera2;

   
    bool currentstats = false;

    GameObject currentSkull;
    public GameObject CurrentSkull
    {
        get { return currentSkull; }
    }

    csPlayer currentPlayer;
    public csPlayer CurrentPlayer
    {
        get { return currentPlayer; }
    }

    float changeCoolTime = 5.0f;
    float changeTime = 0f;

    void Awake()
    {
        changeTime -= changeCoolTime;

        if(currentstats)
        {
            GameManager.Instance.Skull(1).SetActive(false);
            currentPlayer = GameManager.Instance.Player(0);

            //Skull2.SetActive(false);
            //currentPlayer = csPlayer1;
            camera2.SetActive(false);
            camera1.SetActive(true);
            //Skull1.SetActive(true);
            //currentstats = true;

            GameManager.Instance.Skull(0).SetActive(true);
            currentSkull = GameManager.Instance.Skull(0);
            currentstats = true;
        }

        else
        {
            GameManager.Instance.Skull(0).SetActive(false);
            currentPlayer = GameManager.Instance.Player(1);
            camera1.SetActive(false);
            camera2.SetActive(true);
            GameManager.Instance.Skull(1).SetActive(true);
            currentSkull = GameManager.Instance.Skull(1);
            currentstats = false;
        }    
    }

    void Update()
    {
        
        PlayerChange();
    }

    void PlayerChange()
    {
        if (!currentstats && Input.GetKeyDown(KeyCode.Alpha1) && currentPlayer.IsJump && Time.time - changeTime >= changeCoolTime)
        {
            changeTime = Time.time;
            GameManager.Instance.Skull(1).SetActive(false);
            currentPlayer = GameManager.Instance.Player(0);
            GameManager.Instance.Skull(0).transform.position = GameManager.Instance.Skull(1).transform.position + new Vector3(0f,0.38f, 0f);
            float dir = GameManager.Instance.Skull(0).transform.localScale.x > 0 ? 1f : -1f;
            GameManager.Instance.Skull(0).transform.localScale = new Vector3(4f * dir, 4f, 1f);
            camera2.SetActive(false);
            camera1.transform.position = camera2.transform.position;
            camera1.SetActive(true);
            currentstats = true;
            GameManager.Instance.Skull(0).SetActive(true);
            currentSkull = GameManager.Instance.Skull(0);
        }

        else if (currentstats && Input.GetKeyDown(KeyCode.Alpha2) && currentPlayer.IsJump && Time.time - changeTime >= changeCoolTime)
        {
            changeTime = Time.time;
            GameManager.Instance.Skull(0).SetActive(false);
            currentPlayer = GameManager.Instance.Player(1); ;
            GameManager.Instance.Skull(1).transform.position = GameManager.Instance.Skull(0).transform.position + new Vector3(0f, -0.38f, 0f);
            float dir = GameManager.Instance.Player(0).transform.localScale.x > 0 ? 1f : -1f;
            GameManager.Instance.Skull(1).transform.localScale = new Vector3(3f * dir , 3f, 1f);
            currentstats = false;
            camera1.SetActive(false);
            camera2.transform.position = camera1.transform.position;
            camera2.SetActive(true);
            GameManager.Instance.Skull(1).SetActive(true);
            currentSkull = GameManager.Instance.Skull(1);
        }
    }

}
