using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{// �̱������� �ٸ� cs���Ͽ��� ���� �����ϰ� �Ѵ�.
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (!_instance)
            {// �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        FOOD,
        BULLETS,
        OTHERS
    }

    public GameObject m_BagSection;
    public GameObject m_PocketSection;
    public GameObject m_EquipSection;
    public GameObject m_ImgRes;
    public GameObject m_PrefabMakingImage;

    public Canvas m_CvsMaking;
    public GameObject[] makingPanel;
    List<Text> panelRecipeList;
    public InputField makingInputField;
    public Button m_BtnMinus;
    public Button m_BtnPlus;
    public Button m_BtnMax;
    public Button m_BtnMaking;

    public enum CVS_MAKING
    {
        PANEL_LEFT,
        PANEL_MIDTOP,
        PANEL_MIDMID,
        PANEL_LEFTBOT,
        PANEL_RIGHTTOP,
        PANEL_RIGHTMID,
        PANEL_RIGHTBOT,
        PANEL_MAX = 7
    }
    public enum IMG_LIST { AX_STONE, FABRIC, PICK_STONE, PUMPKIN, ROCK, WOOD }

    const int BAG_MAX = 24;
    const int POCKET_MAX = 6;
    const int EQUIP_MAX = 6;
    Button[] BagSlot = new Button[BAG_MAX]; // �κ��丮 �߾ӽ���(24ĭ)
    Button[] PocketSlot = new Button[POCKET_MAX]; // �ϴܽ���(6ĭ)
    Button[] EquipSlot = new Button[EQUIP_MAX]; // �κ��丮 ��������(6ĭ)
    string[] bagItemName = new string[BAG_MAX]; // ������ ������ �̸��� ����
    string[] pocketItemName = new string[POCKET_MAX]; 
    int[] bagItemQuantity = new int[BAG_MAX]; // ������ ������ ������ ����
    int[] pocketItemQuantity = new int[POCKET_MAX];

    public static RawImage[] ImgRes; // ������ �� �̹���

    public class Item
    {
        public string itemName;
        public RawImage itemImage;
        public ITEM_KIND itemKind;
        public List<Item> recipe;
        public List<int> recipeNeedNum;
        public int buildTime;
        public int damage;
        public float attackSpeed;
        public float attackRange;
        public int woodGain;
        public int mineralGain;
        public int meatGain;

        public int hpUp;
        public int hungerUp;
        public int thirstUp;


        public Item(string _itemName,
                            RawImage _itemImage,
                            ITEM_KIND _itemKind,
                            List<Item> _recipe,
                            List<int> _recipeNeedNum,
                            int _buildTime,
                            int _damage,
                            float _attackSpeed,
                            float _attackRange,
                            int _woodGain,
                            int _mineralGain,
                            int _meatGain
                        )
        { // ������ ������
            this.itemName = _itemName;
            this.itemImage = _itemImage;
            this.itemKind = _itemKind;
            this.recipe = _recipe;
            this.recipeNeedNum = _recipeNeedNum;
            this.buildTime = _buildTime;
            this.damage = _damage;
            this.attackSpeed = _attackSpeed;
            this.attackRange = _attackRange;
            this.woodGain = _woodGain;
            this.mineralGain = _mineralGain;
            this.meatGain = _meatGain;
            this.hpUp = 0;
            this.hungerUp = 0;
            this.thirstUp = 0;
        }

        public Item(string _itemName,
                            RawImage _itemImage,
                            ITEM_KIND _itemKind,
                            List<Item> _recipe,
                            List<int> _recipeNeedNum,
                            int _buildTime,
                            int _damage,
                            float _attackSpeed,
                            float _attackRange
                        )
        { // ����� ������
            this.itemName = _itemName;
            this.itemImage = _itemImage;
            this.itemKind = _itemKind;
            this.recipe = _recipe;
            this.recipeNeedNum = _recipeNeedNum;
            this.buildTime = _buildTime;
            this.damage = _damage;
            this.attackSpeed = _attackSpeed;
            this.attackRange = _attackRange;
            this.woodGain = 0;
            this.mineralGain = 0;
            this.meatGain = 0;
            this.hpUp = 0;
            this.hungerUp = 0;
            this.thirstUp = 0;
        }

        public Item(string _itemName,
                            RawImage _itemImage,
                            ITEM_KIND _itemKind,
                            int _hpUp,
                            int _thirstUp,
                            int _hungerUp
                            )
        { // ���� ������
            this.itemName = _itemName;
            this.itemImage = _itemImage;
            this.itemKind = _itemKind;
            this.recipe = null;
            this.recipeNeedNum = null;
            this.buildTime = 0;
            this.damage = 0;
            this.attackSpeed = 0;
            this.attackRange = 0;
            this.woodGain = 0;
            this.mineralGain = 0;
            this.meatGain = 0;
            this.hpUp = _hpUp;
            this.hungerUp = _hungerUp;
            this.thirstUp = _thirstUp;
        }

        public Item(string _itemName, RawImage _itemImage, ITEM_KIND _itemKind)
        { // ��� ������
            this.itemName = _itemName;
            this.itemImage = _itemImage;
            this.itemKind = _itemKind;
            this.recipe = null;
            this.recipeNeedNum = null;
            this.buildTime = 0;
            this.damage = 0;
            this.attackSpeed = 0;
            this.attackRange = 0;
            this.woodGain = 0;
            this.mineralGain = 0;
            this.meatGain = 0;
            this.hpUp = 0;
            this.hungerUp = 0;
            this.thirstUp = 0;
        }
    }

    public static Item static_NullObj;
    public static Item static_Wood;
    public static Item static_Rock;
    public static Item static_Pumpkin;
    public static Item static_Fabric;
    public static Item static_Ax_Stone;
    public static Item static_Pick_Stone;


    private void Awake()
    {
        ImgRes = m_ImgRes.GetComponentsInChildren<RawImage>();
        // �̹��� ���ҽ� �ް� ���� ������ ����
        static_NullObj = new Item("error", null, ITEM_KIND.OTHERS);
        static_Wood = new Item("����", ImgRes[5], ITEM_KIND.MATERIAL);
        static_Rock = new Item("����", ImgRes[4], ITEM_KIND.MATERIAL);
        static_Pumpkin = new Item("ȣ��", ImgRes[3], ITEM_KIND.FOOD, 5, 10, 20);
        static_Fabric = new Item("����", ImgRes[1], ITEM_KIND.MATERIAL);
        static_Ax_Stone = new Item(
            "������", ImgRes[0],
            ITEM_KIND.LIVING_TOOL,
            new List<Item>() { static_Wood, static_Rock },
            new List<int>() { 2, 1 }, 4,
             10, 90f, 1f,
             5, 20, 5
            );
        static_Pick_Stone = new Item(
            "�����", ImgRes[2],
            ITEM_KIND.LIVING_TOOL,
            new List<Item>() { static_Wood, static_Rock },
            new List<int>() { 2, 2 }, 5,
             10, 60f, 1f,
             5, 20, 5
            );

    }

    private void Start()
    {
        BagSlot = m_BagSection.GetComponentsInChildren<Button>();
        PocketSlot = m_PocketSection.GetComponentsInChildren<Button>();
        EquipSlot = m_EquipSection.GetComponentsInChildren<Button>();

        PocketSlot[0].GetComponentInChildren<RawImage>().texture = 
            static_Ax_Stone.itemImage.texture;
        PocketSlot[0].GetComponentInChildren<RawImage>().enabled = true;

        m_BtnMaking.onClick.AddListener(() => MakeItem(static_Pick_Stone));

        MakingItemInfo();
    }

    public void MakingItemInfo()
    {
        RawImage tempImg =
            makingPanel[(int)CVS_MAKING.PANEL_RIGHTTOP].GetComponentInChildren<RawImage>();
        tempImg.texture = static_Pick_Stone.itemImage.texture;

        RawImage tempImg2 =
            makingPanel[(int)CVS_MAKING.PANEL_RIGHTMID].GetComponentInChildren<RawImage>();
        tempImg2.texture = static_Pick_Stone.itemImage.texture;

        Text tempTxt =
            makingPanel[(int)CVS_MAKING.PANEL_RIGHTTOP].GetComponentInChildren<Text>();
        tempTxt.text = static_Pick_Stone.itemName;

        panelRecipeList = new List<Text>(
            makingPanel[(int)CVS_MAKING.PANEL_RIGHTBOT].GetComponentsInChildren<Text>()
            );

        // ��� �� �ʿ䰹��
        panelRecipeList[4].text = static_Pick_Stone.recipeNeedNum[0].ToString() + " ";
        panelRecipeList[8].text = static_Pick_Stone.recipeNeedNum[1].ToString() + " ";
        // ��� �� �̸�
        panelRecipeList[5].text = " " + static_Pick_Stone.recipe[0].itemName;
        panelRecipeList[9].text = " " + static_Pick_Stone.recipe[1].itemName;
        // ���� �ʿ䰹�� = ��� �� �ʿ䰹�� x ���� ����
        panelRecipeList[6].text = " " + (static_Pick_Stone.recipeNeedNum[0] * 
            int.Parse(makingInputField.GetComponentInChildren<Text>().text)).ToString();
        panelRecipeList[10].text = " " + (static_Pick_Stone.recipeNeedNum[1] *
            int.Parse(makingInputField.GetComponentInChildren<Text>().text)).ToString();
        // ���� ����
        bool woodEnoughFlg = false;
        bool rockEnoughFlg = false;
        for (int i = 0; i < bagItemName.Length; i++)
        {
            if (bagItemName[i] == static_Pick_Stone.recipe[0].itemName)
            {// ���� �̸� ������ �ִ��� Ȯ��
                panelRecipeList[7].text = " " + bagItemQuantity[i].ToString();

                if (bagItemQuantity[i] >= int.Parse(panelRecipeList[6].text))
                { // ���� ������ �����ǰ����� ������Ŵ
                    woodEnoughFlg = true;
                    panelRecipeList[7].color = Color.white;
                }
                else
                {
                    woodEnoughFlg = false;
                    panelRecipeList[7].color = Color.yellow;
                }

                if (bagItemQuantity[i] == 0) // ������ ����� ������ 0 �Ǹ� �κ��丮���� ����
                    SlotClear(i);
                break;
            }
            else
            {
                woodEnoughFlg = false;
                panelRecipeList[7].text = " " + 0.ToString();
                panelRecipeList[7].color = Color.yellow;
            }
        }
        for (int i = 0; i < bagItemName.Length; i++)
        {
            if (bagItemName[i] == static_Pick_Stone.recipe[1].itemName)
            {// ���� �̸� ������ �ִ��� Ȯ��
                panelRecipeList[11].text = " " + bagItemQuantity[i].ToString();

                if (bagItemQuantity[i] >= int.Parse(panelRecipeList[10].text))
                { // ���� ������ �����ǰ����� ������Ŵ
                    rockEnoughFlg = true;
                    panelRecipeList[11].color = Color.white;
                }
                else
                {
                    rockEnoughFlg = false;
                    panelRecipeList[11].color = Color.yellow;
                }

                if (bagItemQuantity[i] == 0) // ������ ����� ������ 0 �Ǹ� �κ��丮���� ����
                    SlotClear(i);
                break;
            }
            else
            {
                rockEnoughFlg = false;
                panelRecipeList[11].text = " " + 0.ToString();
                panelRecipeList[11].color = Color.yellow;
            }
        }

        if (rockEnoughFlg && woodEnoughFlg)
            m_BtnMaking.enabled = true;
        else
            m_BtnMaking.enabled = false;

    }

    public void InsertItem(Item _item, int _quantity)
    {
        int idx = 0; // �κ��丮 ��ȸ �� �ε���
        bool sameKindFlg = false; // ���� ���� ������ ���� �� true

        for (idx = 0; idx < bagItemName.Length; idx++)
        {
            if (bagItemName[idx] == _item.itemName)
            {// ���� �̸� ������ �ִ��� Ȯ��
                sameKindFlg = true;
                break;
            }
        }

        if (sameKindFlg)
        { // ���� �̸� ������ ���� ��
            Text slotText = BagSlot[idx].transform.GetComponentInChildren<Text>();
            bagItemQuantity[idx] += _quantity;
            slotText.text = "x" + bagItemQuantity[idx];
            UIControl.Instance.PopupUI(_item, _quantity);
        }
        else // ���� �̸� ������ ���� ��
        {
            for (idx = 0; idx < bagItemName.Length; idx++)
            {
                if (bagItemName[idx] == null)
                {// �κ��丮 �� ���� ������ ��
                    SetSlotImage(idx, _item);
                    bagItemName[idx] = _item.itemName;
                    bagItemQuantity[idx] = _quantity;
                    Text slotText = BagSlot[idx].transform.GetComponentInChildren<Text>();
                    slotText.text = "x" + bagItemQuantity[idx];
                    UIControl.Instance.PopupUI(_item, _quantity);
                    break;
                }
            }
        }

        MakingItemInfo(); // ���� ����(���� ����) ������Ʈ
    }

    public Item ConfigItem(string itemTag)
    {
        switch (itemTag)
        { // ������ Ȯ��(����ü)
            case "Tree":
            case "TreeMark":
            case "Wood":
                return static_Wood;
            case "Rock":
                return static_Rock;
            case "Pumpkin":
                return static_Pumpkin;
            case "Fabric":
                return static_Fabric;
            case "Pick_Stone":
                return static_Pick_Stone;
            default:
                return static_NullObj;
        }
    }

    void SetSlotImage(int _idx, Item _item)
    { // �κ��丮 �� �������̹��� ����
        RawImage ItemImage = BagSlot[_idx].GetComponentInChildren<RawImage>();
        ItemImage.enabled = true;

        switch (_item.itemName)
        {
            case "����": ItemImage.texture = ImgRes[(int)IMG_LIST.WOOD].texture; break;
            case "����": ItemImage.texture = ImgRes[(int)IMG_LIST.ROCK].texture; break;
            case "ȣ��": ItemImage.texture = ImgRes[(int)IMG_LIST.PUMPKIN].texture; break;
            case "����": ItemImage.texture = ImgRes[(int)IMG_LIST.FABRIC].texture; break;
            case "������": ItemImage.texture = ImgRes[(int)IMG_LIST.AX_STONE].texture; break;
            case "�����": ItemImage.texture = ImgRes[(int)IMG_LIST.PICK_STONE].texture; break;
            default:    break;
        }

    }

    void SlotClear(int _slotIdx)
    { // �κ��丮 �� ������ ���� ���� ����
        bagItemName[_slotIdx] = null;
        bagItemQuantity[_slotIdx] = 0;

        RawImage ItemImage = BagSlot[_slotIdx].GetComponentInChildren<RawImage>();
        ItemImage.texture = null;
        ItemImage.enabled = false;

        Text slotText = BagSlot[_slotIdx].transform.GetComponentInChildren<Text>();
        slotText.text = null;
    }

    public void MakeItem(Item item)
    {
        for(int i = 0; i < item.recipe.Count; i++)
        { // ��� �Ҹ�
            for (int j = 0; j < BAG_MAX; j++)
            {
                if (bagItemName[j] == item.recipe[i].itemName)
                {
                    bagItemQuantity[j] -= item.recipeNeedNum[i];
                    BagSlot[j].transform.GetComponentInChildren<Text>().text = "x" + bagItemQuantity[j];
                }
            }
        }

        StartCoroutine(ItemMakingTimer(item));
        MakingItemInfo(); // ���� ����(���� ����) ������Ʈ
    }

    IEnumerator ItemMakingTimer(Item item)
    {
        GameObject tmpObj = Instantiate(m_PrefabMakingImage);
        tmpObj.transform.SetParent(makingPanel[(int)CVS_MAKING.PANEL_LEFTBOT].transform);
        tmpObj.transform.localScale = new Vector3(1,1,1); // .ũ�������ָ� ������� Ŀ��
        tmpObj.GetComponent<RawImage>().texture = item.itemImage.texture;

        int timer = item.buildTime;
        while (timer > 0)
        {
            tmpObj.GetComponentInChildren<Text>().text = timer.ToString() + "s";
            timer--;
            yield return new WaitForSeconds(1.0f);
        }

        InsertItem(item, 1);
        Destroy(tmpObj);
    }

}
