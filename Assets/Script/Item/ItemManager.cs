using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{// 싱글톤으로 다른 cs파일에서 접근 가능하게 한다.
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (!_instance)
            {// 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                _instance = FindObjectOfType(typeof(ItemManager)) as ItemManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public enum ITEM_KIND
    {
        ERR = -1, NULL,
        MATERIAL,
        LIVING_TOOL,
        OTHERS
    }

    public struct Item
    {
        public string itemName;
        public ITEM_KIND itemKind;
    }

    const int BAG_MAX = 24;
    public Button[] m_BagSlot = new Button[BAG_MAX]; // 인벤토리 중앙슬롯(24칸)
    string[] itemName = new string[BAG_MAX]; // 슬롯의 아이템 이름을 저장
    int[] itemQuantity = new int[BAG_MAX]; // 슬롯의 아이템 갯수를 저장
     
    public void InsertItem(Item _item, int _quantity)
    {
        int idx = 0; // 인벤토리 순회 돌 인덱스
        bool sameKindFlg = false; // 같은 종류 아이템 있을 시 true

        for (idx = 0; idx < itemName.Length; idx++)
        {
            if (itemName[idx] == _item.itemName)
            {// 같은 이름 아이템 있는지 확인
                sameKindFlg = true;
                break;
            }
        }

        if (sameKindFlg)
        { // 같은 이름 아이템 있을 때
            Text slotText = m_BagSlot[idx].transform.GetComponentInChildren<Text>();
            itemQuantity[idx] += _quantity;
            slotText.text = "x" + itemQuantity[idx];
            UI_OnOff.Instance.PopupUI(_item, _quantity);
        }
        else // 같은 이름 아이템 없을 때
        {
            for (idx = 0; idx < itemName.Length; idx++)
            {
                if (itemName[idx] == null)
                {// 인벤토리 빈 공간 존재할 때
                    SetSlotImage(idx, _item);
                    itemName[idx] = _item.itemName;
                    itemQuantity[idx] = _quantity;
                    Text slotText = m_BagSlot[idx].transform.GetComponentInChildren<Text>();
                    slotText.text = "x" + itemQuantity[idx];
                    UI_OnOff.Instance.PopupUI(_item, _quantity);
                    break;
                }
            }
        }
    } // end of InsertItem()

    public Item ConfigItem(string itemTag)
    {
        Item item;

        switch (itemTag)
        { // 아이템 확인(구조체)
            case "Tree":
            case "TreeMark":
            case "Wood":
                item.itemKind = ITEM_KIND.MATERIAL;
                item.itemName = "나무";
                return item;
            case "Rock":
                item.itemKind = ITEM_KIND.MATERIAL;
                item.itemName = "돌";
                return item;
            case "Pumpkin":
                item.itemKind = ITEM_KIND.MATERIAL;
                item.itemName = "호박";
                return item;
            default:
                item.itemKind = ITEM_KIND.ERR;
                item.itemName = "error";
                return item;
        }
    }

    void SetSlotImage(int _idx, Item _item)
    { // 인벤토리 내 아이템이미지 설정
        Color newColor;
        switch (_item.itemName)
        {
            case "나무": newColor = Color.green; break;
            case "돌": newColor = Color.gray; break;
            case "호박": newColor = new Color(1f, 0.59f, 0f); break;
            default: newColor = Color.black; break;
        }
        ColorBlock cb = m_BagSlot[_idx].colors;
        cb.normalColor = newColor;
        m_BagSlot[_idx].colors = cb;

    }
}
