using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    //This script gets the player and camera positions and pairs them up.
    public Transform camPos;
    public Transform playerPos;
    

    // Update is called once per frame
    void Update()
    {
        camPos.position = new Vector3(playerPos.position.x + 1, playerPos.position.y + 1, -20f);
    }
}
