using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    float yawSpeed = 100f,
        avoidDistance = 20f,
        fixedVelocity = 25f,
        distantToTarget = 15f;


    [SerializeField]
    Transform rayTransform;

    Rigidbody rigidBody;


    GameObject player;

    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        player = MainGameController.Instance.Player;
        setVelocity();
        checkforohbstacle();
    }


    private void TurnToTarget(Vector3 desiredHeading)
    {
        Quaternion rotationGoal = Quaternion.LookRotation(desiredHeading);
        float step = yawSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationGoal, step);
    }

    private void setVelocity()
    {
        Vector3 velo = player.GetComponent<Rigidbody>().velocity + (player.transform.position - transform.position);
        rigidBody.velocity = -transform.forward * (velo.magnitude- distantToTarget);
    }
    private void checkforohbstacle()
    {
        RaycastHit hit;
        Vector3 targetPosition = player.transform.position;
        Vector3 agentPosition = transform.position;
        Vector3 desiredHeading = -(targetPosition - agentPosition);

        if (Physics.Raycast(rayTransform.position,-desiredHeading * avoidDistance,out hit,avoidDistance))
        {
            if (hit.collider.gameObject.CompareTag("Asteroid")) 
            {
                targetPosition = checkForDirections();
                desiredHeading = -(targetPosition - agentPosition);
            }

        }
        TurnToTarget(desiredHeading);
    }


    private Vector3 checkForDirections()
    {

        Vector3[] directions =
        {
            -rayTransform.right,
            rayTransform.right,
            -rayTransform.up,
            rayTransform.up
        };

        if (!Physics.Raycast(rayTransform.position, rayTransform.forward, avoidDistance / 2f))
        {
            return rayTransform.forward;
        }
        foreach (Vector3 dir in directions)
        {

            Vector3 checkDir = (-rayTransform.forward + dir).normalized * avoidDistance /2f;
            if (!Physics.Raycast(rayTransform.position, dir, avoidDistance/2f))
            {
                return checkDir;
            }
        }
        return -rayTransform.forward + directions[Random.Range(0, directions.Length)];
    }

}
