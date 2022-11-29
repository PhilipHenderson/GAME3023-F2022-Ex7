// POortal script will allow any travaler to touch it, then it will send them to a specified location in the specified Sene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This portal leads to the Sceme with the same naem as its Tag, to a portal, with the same tag as its name
/// </summary>

public class PortalScript : MonoBehaviour
{
    public string targetSpawn = "";
    //Target scene
    //target location within scene
    //Who can Travel?
    //[SerializeField]
   //string targetScene = "OverWorld";

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log("Portal Triggaered with: " + collision.gameObject.name);
       Traveller traveller = collision.GetComponent<Traveller>();

        if (traveller != null)
        {
            Debug.Log("Portal Warping: " + traveller.gameObject.name);
            traveller.SetSpawn(targetSpawn);
            traveller.onTrasportToNewScene.Invoke(SceneManager.GetSceneByName(tag));
            SceneManager.LoadScene(tag, LoadSceneMode.Single);
            Debug.Log("Should have loaded scene");
        }
    }
}
