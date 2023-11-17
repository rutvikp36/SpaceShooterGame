using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject blockPrefab,
        CheckPointPrefab;

    [SerializeField]
    float xShift, yShift,zShift,zShift2;

    [SerializeField]
    float xShiftCheckPoint, yShiftCheckPoint, zShiftCheckPoint, zShift2CheckPoint;


    [SerializeField]
    float maxScale = 30f,
        mode2Shift = 100f;


    [SerializeField]
    int thresold = 10,
        checkPointThrshold = 3,
        AsteroidTheroldMode2 = 300;

    [HideInInspector]
    public int asteroidNumber = 0,
            checkPointNumber = 0;

    Vector3 SpwanPointForMode2;
    Transform PlayerLocation;
    GameObject newEnemy;


    private static AsteroidsSpawner instance;

    public static AsteroidsSpawner Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AsteroidsSpawner>() as AsteroidsSpawner;
            return instance;
        }
    }

    public void decreseAseNum()
    {
        asteroidNumber--;
    }
    public void decreseCheckPNum()
    {
        checkPointNumber--;
    }


    private void Awake()
    {
        foreach(Transform aste in transform)
        {
            Destroy(aste.gameObject);
        }
    }

    private void OnDisable()
    {
        DestroyAllChildren();
    }


    public void DestroyAllChildren()
    {
        foreach (Transform aste in transform)
        {
            Destroy(aste.gameObject);
        }
        asteroidNumber = 0;
        checkPointNumber = 0;
    }



    // Update is called once per frame
    void Update()
    {
        
        if (MainGameController.Instance.gameMode == 1)
        {
            PlayerLocation = MainGameController.Instance.Player.transform;

            while (asteroidNumber < thresold)
            {
                spwanAsteroid();

            }

            while (checkPointNumber < checkPointThrshold)
            {
                spawnCheckPoint();

            }
        }
        if(MainGameController.Instance.gameMode == 2)
        {
            PlayerLocation = MainGameController.Instance.Player.transform;
            SpwanPointForMode2 = PlayerLocation.position;
            while (asteroidNumber < AsteroidTheroldMode2)
            {
                spwanAsteroidMode2();

            }
        }

    }

    public void spawnCheckPoint()
    {
        float zPOS = Random.Range(PlayerLocation.position.z - zShiftCheckPoint, PlayerLocation.position.z - zShift2CheckPoint);
        float xPOS = Random.Range(-xShiftCheckPoint, xShiftCheckPoint);
        float yPOS = Random.Range(-yShiftCheckPoint, yShiftCheckPoint);


        GameObject checkP =  Instantiate(CheckPointPrefab, new Vector3(xPOS, yPOS, zPOS), Quaternion.AngleAxis(90,new Vector3(1,0,0)));
        checkP.transform.parent = gameObject.transform;


        checkPointNumber++;
    }

    public void spwanAsteroid()
    {
        if (blockPrefab)
        {
            float zPOS = Random.Range(PlayerLocation.position.z - zShift, PlayerLocation.position.z - zShift2);
            float xPOS = Random.Range(-xShift, xShift);
            float yPOS = Random.Range(-yShift, yShift);



            newEnemy = Instantiate(blockPrefab, new Vector3(xPOS, yPOS, zPOS), Quaternion.identity);

            newEnemy.transform.parent = transform;

            float scaleInc = Random.Range(maxScale/4, maxScale);
            newEnemy.transform.localScale += new Vector3(scaleInc, scaleInc, scaleInc);
            asteroidNumber++;
        }
    }

    public void spwanAsteroidMode2()
    {
        if (blockPrefab)
        {
            float zPOS = Random.Range(-mode2Shift,mode2Shift);
            float xPOS = Random.Range(-mode2Shift, mode2Shift);
            float yPOS = Random.Range(-mode2Shift, mode2Shift);



            newEnemy = Instantiate(blockPrefab, new Vector3(xPOS, yPOS, zPOS), Quaternion.identity);
             newEnemy.transform.parent = transform;


            float scaleInc = Random.Range(maxScale/2, maxScale);
            newEnemy.transform.localScale += new Vector3(scaleInc, scaleInc, scaleInc);

            asteroidNumber++;
        }
    }
}
