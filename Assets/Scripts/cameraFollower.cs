using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollower : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float xoffset, yoffsrt, zoffset;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = MainGameController.Instance.Player.transform.position;
        transform.position = new Vector3(pos.x + xoffset, pos.y + yoffsrt, pos.z + zoffset);
    }
}
