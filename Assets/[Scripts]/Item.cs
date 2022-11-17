using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    public Sprite icon; // Icon for this item

    public string description = "This is an Item";

    public void Use()
    {
        Debug.Log("Used item: " + name);
    }
}
