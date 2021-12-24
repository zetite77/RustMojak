using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ITEM_KIND
{
    ERR = -1,
    CLOTHING,
    RANGE_WEAPON,
    MELEE_WEAPON,
    FOOD,
    BUILD,
    ROPE,
    CARD,
    OTHERS
}

public struct Item
{
    public string itemName;
    public ITEM_KIND itemKind;
}

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
