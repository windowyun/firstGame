using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    instance = obj;
                }

                else
                {
                    var newobj = new GameObject().AddComponent<GameManager>();
                    instance = newobj;
                }
            }
            return instance;
        }
    }

    [SerializeField]
    ChangePlayer changePlayer;

    public ChangePlayer ChangeManager
    {
        get { return changePlayer; }
    }

    [SerializeField]
    GameObject[] skull;

    public GameObject Skull(int index)
    {
        return skull[index];
    }
    //public GameObject[] Skull
    //{
    //    get { return skull; }
    //}

    [SerializeField]
    csPlayer[] player;

    public csPlayer Player(int index)
    {
        return player[index];
    }
    //public csPlayer[] Player
    //{
    //    get { return player; }
    //}

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if(objs.Length != 1)
        {
            Debug.Assert(Instance);
            Debug.LogError(gameObject);
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        

    }
}
