using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    //This script gets the player and camera positions and pairs them up.
    public Transform camPos;
    public Transform playerPos;

    [Header("Offset")]
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = -20f;

    // Update is called once per frame
    void Update()
    {
        camPos.position = new Vector3(playerPos.position.x + offsetX, playerPos.position.y + offsetY, offsetZ);
    }
}
