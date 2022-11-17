using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    private static PlayerCharacterMovement player = null;
    public static PlayerCharacterMovement Player
    {
        get { return player; }
        private set { }
    }

    void Awake()
    {
        if (player == null)
        {
            GameObject newObject = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            player = newObject.GetComponent<PlayerCharacterMovement>();
        }
    }
}
