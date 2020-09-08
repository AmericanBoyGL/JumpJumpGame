using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton class: GameManager

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    //For player sattings
    public GameObject PlayerObj;
    Camera cam;

    public Player player;
    public Trajectory trajectory;
    [SerializeField] float pushForce = 4f;

    bool isDragging = false;

    //It's for trajectory line
    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;
    [SerializeField] float maxDistanceDrag;

    //For Toxic platform
    public GameObject toxicPlatform;
    public float speedTox = 0.002f;
    public float StepSpeedTox;
    public bool toxicPlatMove = false;

    //For func MapSpawner, to loop game mode
    public Renderer rend;
    public List<GameObject> MapOnGame;
    public GameObject mapPref;
    public Transform spawnPoint;
    public Transform spawnPlayer;


    //------------------------------------------
    void Start()
    {
        cam = Camera.main;
        player.DesactivateRb();

        //это потом нужно перенести в место когда вкл. уровень
        player.transform.position = spawnPlayer.position;

    }

    void FixedUpdate()
    {
        if(toxicPlatMove == true)
        {
            toxicPlatform.transform.position = new Vector3(0f, toxicPlatform.transform.position.y + (speedTox * Time.deltaTime) / 4f, 0f);
            speedTox = speedTox + StepSpeedTox;

        }
    }

    void Update()
    {
        //нужно будет добавить управление для телефона Touch
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            OnDragStart();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnDragEnd();
        }
        if (isDragging)
        {
            OnDrag();
        }

        cam.transform.position = new Vector3(0f, player.transform.position.y, -10f);
    }

    // Drag Func------------------------------------
    private void OnDragStart()
    {
        if (player.JumpCount != 0)
        {
            player.DesactivateRb();
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

            trajectory.Show();
        } 
    }

    private void OnDrag()
    {
        if(player.JumpCount != 0)
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            distance = Vector2.Distance(startPoint, endPoint);

            if (distance > maxDistanceDrag)
                distance = maxDistanceDrag;

            direction = (startPoint - endPoint).normalized;
            force = direction * distance * pushForce;

            //for debug
            Debug.DrawLine(startPoint, endPoint);

            trajectory.UpdateDots(player.pos, force);

            if (PlayerObj.transform.parent != null)
                PlayerObj.transform.parent = null;
        }
    }

    private void OnDragEnd()
    {
        if (player.JumpCount != 0)
        {
            //толкнуть шарик
            player.DragChecker = true;

            player.ActivateRb();

            player.PushPlayer(force);

            trajectory.Hide();

            player.JumpCount--;

            toxicPlatMove = true;
        } 
    }

    //------------------------------------------------------
    public void MapSpawner()
    {
        //узнаю точку где нужно поставить новую часть карты
        MapOnGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Map"));
        
        spawnPoint = MapOnGame[MapOnGame.Count - 1].transform.Find("SpawnPoint");
        Instantiate(mapPref, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z), Quaternion.identity);

        if (MapOnGame.Count > 2)
        {
            Destroy(MapOnGame[0]);
            MapOnGame.Remove(MapOnGame[0]);
        }
        MapOnGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Map"));

        GameObject[] _map = GameObject.FindGameObjectsWithTag("Map");
        List<GameObject> _MapOnGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Map"));
        
        TrapSpawner.trapSpawner.SpawnTrapsOnMap(_MapOnGame[ _MapOnGame.Count - 1 ]);
    }
}
