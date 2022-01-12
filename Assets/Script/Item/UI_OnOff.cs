using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OnOff : MonoBehaviour
{// 싱글톤으로 다른 cs파일에서 접근 가능하게 한다.
    private static UI_OnOff _instance;
    public static UI_OnOff Instance
    {
        get 
        {
            if (!_instance)
            { // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                _instance = FindObjectOfType(typeof(UI_OnOff)) as UI_OnOff;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public Canvas m_Inventory;
    public Canvas m_gatheringUI;
    public Canvas m_gatheredUI;

    private bool isInventoryOn = false;
    private Text raycastTxt;
    private Text gatheredTxt;
    private GameObject clearObj;

    void Start()
    {
        //UI들 초기에 비표시상태로 시작
        m_gatheringUI.enabled = false;
        m_gatheredUI.enabled = false;

        raycastTxt = m_gatheringUI.gameObject.GetComponentInChildren<Text>();
        gatheredTxt = m_gatheredUI.gameObject.GetComponentInChildren<Text>();
    }
    void Update()
    {
        InventoryOnOff();
    }

    void InventoryOnOff()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            isInventoryOn = !isInventoryOn;

        if (isInventoryOn)
        {
            m_Inventory.enabled = true;
            Cursor.visible = true;
        }
        else
        {
            m_Inventory.enabled = false;
            Cursor.visible = false;
        }
    }
    public void RaycastUI(bool _rayCollision, RaycastHit _hitInfo)
    {
        if (_rayCollision)
        {   // <E키를 눌러 상호작용> 텍스트UI 노출
            m_gatheringUI.enabled = true;
            switch (_hitInfo.collider.tag)
            {
                case "Wood":
                    raycastTxt.text = "목재";
                    break;
                case "Rock":
                    raycastTxt.text = "석재";
                    break;
                case "Pumpkin":
                    raycastTxt.text = "호박";
                    break;
                default:
                    break;
            }
            raycastTxt.text += "\nE키를 눌러 상호작용";

            if (Input.GetKeyDown(KeyCode.E))
            {   // UI 노출 중 상호작용 키 입력 시 습득 및 기존오브젝트 제거
                ItemManager.Item item;
                item = ItemManager.Instance.ConfigItem(_hitInfo.collider.tag.ToString());
                ItemManager.Instance.InsertItem(item, 1);

                clearObj = _hitInfo.transform.gameObject;
                Destroy(clearObj);
            }
        }
        else
            m_gatheringUI.enabled = false;
    }

    public void PopupUI(ItemManager.Item _item, int _quantity)
    {
        gatheredTxt.text += "\n" + _item.itemName + "+" + _quantity;
        m_gatheredUI.enabled = true;
    }
}
