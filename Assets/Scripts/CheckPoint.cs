using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    float zShiftForCheckP = 100f;

    void Update()
    {
        if (MainGameController.Instance.Player.transform.position.z + zShiftForCheckP < transform.position.z)
        {
            AsteroidsSpawner.Instance.decreseCheckPNum();
            GameObject.Destroy(gameObject);
        }
    }
}
