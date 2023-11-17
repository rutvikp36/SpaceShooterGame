using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float thrustForce = 3500f,
          pitchForce = 60f,
          yawForce = 60f,
          rollForce = 25f,
        multiplier = 1f;



    [SerializeField]
    GameObject bulletPrefab,
               destroyEffect,
               thrusterParticle;

    [SerializeField]
    Transform bulletPoint;
    
    [SerializeField]    
    float bulletSpeed;

    [SerializeField]
    MeshRenderer selfMesh;

    int NumberOfBullets = 10;
    bool isGameOver = false;
    float thrustAmount, pitchAmount, rollAmount, yawAmount = 0f;
        Rigidbody rigidBody;




    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            InputController();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireBullet();
        }

        if (MainGameController.Instance.gameMode == 1)
        {
            MainUIController.Instance.InGameNoOfBulletsText.text = "Bullets : " + NumberOfBullets;
        }
        if (MainGameController.Instance.gameMode == 2)
        {
            MainUIController.Instance.InGameNoOfBulletsText.text = "Bullets : Unlimited";
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            GameObject.Destroy(other.gameObject);
            AsteroidsSpawner.Instance.checkPointNumber--;
            NumberOfBullets += 10;
        }
        if (other.CompareTag("Border"))
        {
            ReCenter();
        }
        if (other.CompareTag("Asteroid") || other.CompareTag("EnemyBullet"))
        {
            StartCoroutine(DestroyPlayer());
        }
    }


    // --------------------------------------Helper Function ---------------------------------------------------


    private IEnumerator DestroyPlayer()
    {
        isGameOver = true;
        thrusterParticle.SetActive(false);
        gameObject.GetComponent<AudioSource>().Play();
        GameObject blastEffect = Instantiate(destroyEffect,transform.position,transform.rotation);
        blastEffect.transform.localScale = Vector3.one * 10;
        rigidBody.velocity = Vector3.zero;
        selfMesh.enabled = false;
        yield return new WaitForSeconds(2f);
        MainGameController.Instance.gameOver();
    }
    private void ReCenter()
    {
        gameObject.transform.localPosition = new Vector3(0,0, gameObject.transform.localPosition.z);
        gameObject.transform.localRotation = Quaternion.identity;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        Input.ResetInputAxes();
        MainGameController.Instance.enemy.transform.localPosition = new Vector3(0, 0, gameObject.transform.localPosition.z + 10);
        StartCoroutine(MainUIController.Instance.LoadingScreen());
    }

    private void fireBullet()
    {
        if( NumberOfBullets > 0)
        {
            if (MainGameController.Instance.gameMode == 1)
            {
                NumberOfBullets--;
            }

            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletPoint);
            bullet.GetComponent<Rigidbody>().velocity = -gameObject.transform.forward * bulletSpeed;
        }

    }


    // ------------------------------------------Main Input Controller ---------------------------------------------------------------------------


    private void InputController()
    {
        yawAmount = 0f;
        pitchAmount = 0f;
        rollAmount = 0f;
        thrustAmount = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rollAmount = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rollAmount = -1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            thrustAmount = -1f;
        }

        yawAmount = Input.GetAxis("Horizontal");
        pitchAmount = Input.GetAxis("Vertical");


        if(MainGameController.Instance.gameMode == 1)
        {
            multiplier = 1f;
           rigidBody.velocity = -transform.forward * rollForce;
            if(Vector3.Angle(transform.forward,Vector3.forward) > 100f)
            {
                ReCenter();
            }
        } 
        if(MainGameController.Instance.gameMode == 2)
        {
            //multiplier = 5f;
        }




        if (!Mathf.Approximately(0f, pitchAmount))
        {
            rigidBody.AddTorque(transform.right * (pitchForce * pitchAmount * Time.fixedDeltaTime * multiplier));
        }

        if (!Mathf.Approximately(0f, rollAmount))
        {
            rigidBody.AddTorque(transform.forward * (rollForce * rollAmount * Time.fixedDeltaTime * multiplier));
        }

        if (!Mathf.Approximately(0f, yawAmount))
        {
            rigidBody.AddTorque(transform.up * (yawAmount * yawForce * Time.fixedDeltaTime * multiplier));
        }

        if (!Mathf.Approximately(0f, thrustAmount))
        {
            rigidBody.AddForce(transform.forward * (thrustForce * thrustAmount * Time.fixedDeltaTime * multiplier));
        }
    }

}
