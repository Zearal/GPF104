using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform camPos;
    public Transform playerPos;
    
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        camPos.position = new Vector3(playerPos.position.x, playerPos.position.y, -12f);
    }
}
