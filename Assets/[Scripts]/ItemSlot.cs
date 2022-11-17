using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Item inSlot = null;

    public Image iconInSlot;
    public TMPro.TextMeshProUGUI countText;

    //C# property
    public int count = 0;
    public int Count
    {
        get { return count; }
        set 
        { 
            count = value;
            if (count < 1)
            {
                inSlot = null;
            }
            UpdateGraphic();
        }
    }

    bool CanUse()
    {
        return Count > 0;
    }

     public void UseItemInSlot()
    {
        if (CanUse())
        {
            inSlot.Use();
            Count--;
            UpdateGraphic();
        }
    }

    public void UpdateGraphic()
    {
        if (inSlot != null)
        {
            iconInSlot.gameObject.SetActive(true);
            iconInSlot.sprite = inSlot.icon;
            countText.text = count.ToString();
        }
        else
        {
            iconInSlot.gameObject.SetActive(false);
            iconInSlot.sprite = null;
            countText.text = "";
        }
    }

    void Start()
    {
        UpdateGraphic();
    }
}

