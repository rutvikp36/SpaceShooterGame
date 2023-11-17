using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    GameObject DestroyedAsteroid;

    [SerializeField]
    float breakForce = 1500f,
            animTime = 1f;

    void Update()
    {
        if( MainGameController.Instance.gameMode == 1 && MainGameController.Instance.Player.transform.position.z+100 < transform.position.z)
        {
            AsteroidsSpawner.Instance.decreseAseNum();
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Bullet"))
        {
            if(MainGameController.Instance.gameMode == 2)
            {
                MainGameController.Instance.Score++;
            }
            StartCoroutine(BlastAsteroid());
        }
    }

    public IEnumerator BlastAsteroid()
    {
        AsteroidsSpawner.Instance.decreseAseNum();
        gameObject.GetComponent<AudioSource>().Play();
        GameObject tempAsteroid = Instantiate(DestroyedAsteroid, transform);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        foreach(var colider in gameObject.GetComponents<CapsuleCollider>())
        {
            colider.enabled = false;
        }
        foreach (Rigidbody rb in tempAsteroid.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
        yield return new WaitForSeconds(animTime);
        Destroy(gameObject);
        
    }

}
