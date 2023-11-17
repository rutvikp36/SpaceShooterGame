using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainGameController : MonoBehaviour
{

    private static MainGameController instance;

    public static MainGameController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MainGameController>() as MainGameController;
            return instance;
        }
    }


    [HideInInspector]
    public GameObject Player;

    [SerializeField]
    GameObject[] AirPlaneModels;
    [SerializeField]
    GameObject Borders;

    [SerializeField]
    GameObject EnemyPrefab;

    public GameObject MainMenuCanvas;
    public GameObject InGameCanvas;


    public GameObject Mode1;
    public GameObject Mode2;

    [HideInInspector]
    public int gameMode = 0,Score = 0;

    float  CurrentScore =0;
    [HideInInspector]
    public GameObject enemy;

    private void Awake()
    {
        OnGameStart();
    }


    public void OnGameStart()
    {
        Time.timeScale = 1;
        gameMode = 0;
        Score = 0;
        CurrentScore = 0;
        MainMenuCanvas.SetActive(true);
        InGameCanvas.SetActive(false);
        Mode1.SetActive(false);
        Mode2.SetActive(false);
        object[] allPlayerObjects = (FindObjectsOfType(typeof(PlayerController),true));
        foreach(var plyr in allPlayerObjects)
        {
            Destroy(((PlayerController)plyr).gameObject);
        }
        object[] enemyObjects = (FindObjectsOfType(typeof(Enemy), true));
        foreach (var enmy in enemyObjects)
        {
            Destroy(((Enemy)enmy).gameObject);
        }
        AsteroidsSpawner.Instance.DestroyAllChildren();

    }

    void Update()
    {
        if(gameMode == 1)
        {
            Borders.transform.position = new Vector3(Borders.transform.position.x, Borders.transform.position.y, Player.transform.position.z);
            CurrentScore += Time.deltaTime;
            Score = (int)CurrentScore;
        }

    }
    
    public void startMode1(int selectedPlane)
    {
        gameMode = 1;
        MainMenuCanvas.SetActive(false);
        Player = Instantiate(AirPlaneModels[selectedPlane], Mode1.transform);
/*        enemy = Instantiate(EnemyPrefab, Mode1.transform);
        enemy.transform.localPosition = new Vector3(0, 0, 10);*/
        InGameCanvas.SetActive(true);
        Mode1.SetActive(true);
        StartCoroutine(MainUIController.Instance.LoadingScreen());
    }

    public void startMode2(int selectedPlane)
    {
        gameMode = 2;
        MainMenuCanvas.SetActive(false);
        Player = GameObject.Instantiate(AirPlaneModels[selectedPlane], Mode2.transform);
        enemy = Instantiate(EnemyPrefab, Mode2.transform);
        enemy.transform.localPosition = new Vector3(0, 0, 10);
        InGameCanvas.SetActive(true);
        Mode2.SetActive(true);
        StartCoroutine(MainUIController.Instance.LoadingScreen());
    }

    public void gameOver()
    {
        Time.timeScale = 0;
        MainUIController.Instance.gameOverUI();
    }

}
