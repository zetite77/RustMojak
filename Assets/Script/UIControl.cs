using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{// �̱������� �ٸ� cs���Ͽ��� ���� �����ϰ� �Ѵ�.
    private static UIControl _instance;
    public static UIControl Instance
    {
        get 
        {
            if (!_instance)
            { // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                _instance = FindObjectOfType(typeof(UIControl)) as UIControl;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public Canvas m_CvsUI;
    public Canvas m_CvsInventory;
    public Canvas m_CvsMaking;
    public Canvas m_gatheringUI;
    public Canvas m_gatheredUI;

    public Button m_MoveToMaking;
    public Button m_MoveToInventory;

    public bool isInventoryOn = false;
    //private bool isMakingOn = false;
    private Text raycastTxt;
    private Text gatheredTxt;
    private GameObject clearObj;

    void Start()
    {
        //UI�� �ʱ⿡ ��ǥ�û��·� ����
        m_CvsInventory.enabled = false;
        m_gatheringUI.enabled = false;
        m_gatheredUI.enabled = false;

        // �κ��丮 <-> ���� â ��ȯ ��ư
        m_MoveToMaking.onClick.AddListener(() => {
            m_CvsInventory.enabled = false;
            m_CvsMaking.enabled = true;
        });
        m_MoveToInventory.onClick.AddListener(()=> { 
            m_CvsInventory.enabled = true;
            m_CvsMaking.enabled = false; });

        raycastTxt = m_gatheringUI.gameObject.GetComponentInChildren<Text>();
        gatheredTxt = m_gatheredUI.gameObject.GetComponentInChildren<Text>();

    }
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOn = !isInventoryOn;
            InventoryOnOff(isInventoryOn);
        }
    }
    

    void InventoryOnOff(bool _isInventoryOn)
    {
        switch (_isInventoryOn)
        {
            case true:
                m_CvsUI.enabled = true;
                m_CvsInventory.enabled = true;
                m_CvsMaking.enabled = false;
                Cursor.visible = true;
                break;
            case false:
                m_CvsUI.enabled = false;
                m_CvsInventory.enabled = false;
                m_CvsMaking.enabled = false;
                Cursor.visible = false;
                break;
        }
    }


    public void RaycastUI(bool _rayCollision, RaycastHit _hitInfo)
    {
        if (_rayCollision)
        {   // <EŰ�� ���� ��ȣ�ۿ�> �ؽ�ƮUI ����
            m_gatheringUI.enabled = true;
            switch (_hitInfo.collider.tag)
            {
                case "Wood":
                    raycastTxt.text = "����";
                    break;
                case "Rock":
                    raycastTxt.text = "����";
                    break;
                case "Pumpkin":
                    raycastTxt.text = "ȣ��";
                    break;
                case "Fabric":
                    raycastTxt.text = "����";
                    break;
                default:
                    raycastTxt.text = "error";
                    break;
            }
            raycastTxt.text += "\nEŰ�� ���� ��ȣ�ۿ�";

            if (Input.GetKeyDown(KeyCode.E))
            {   // UI ���� �� ��ȣ�ۿ� Ű �Է� �� ���� �� ����������Ʈ ����
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
