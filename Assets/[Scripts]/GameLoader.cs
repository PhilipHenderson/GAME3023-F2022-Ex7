using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    PlayerCharacterMovement script;
    [SerializeField]
    GameObject player;


    //float xPosition;
    //string xKey = "Player's X Position";

    //float yPosition;
    //string yKey = "Player's Y Position";

    // This Script Will Get The x and y Key From The Saved Location(Registry Editor)
    // And Change The Players X And Y Positions Accordingly 

    void Awake()
    {
        script = player.GetComponent<PlayerCharacterMovement>();
        script.isLoading = false;
    }


    public void Load()
    {
        script.isLoading = true;
        //xPosition = PlayerPrefs.GetFloat(xKey);
        //yPosition = PlayerPrefs.GetFloat(yKey);
    }
}
