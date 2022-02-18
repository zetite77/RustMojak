using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSlotSwap : MonoBehaviour
{
    public GameObject handsObj; // 무기 슬롯 활성화 시 나타나는 3D 오브젝트 (플레이어 오른손과 연결됨)
    enum EQUIP_STATE { HANDS_FREE = -1, SLOT_1, SLOT_2, SLOT_3, SLOT_4, SLOT_5, SLOT_6, POCKET_MAX = 6 };

    Button[] pocketSlot = new Button[ (int)EQUIP_STATE.POCKET_MAX ]; // 인벤토리 하단 슬롯(24칸)
    EQUIP_STATE preNum = EQUIP_STATE.HANDS_FREE; // 무기 스왑 시 이전 활성화 슬롯

    public static bool equipped; // 스왑애니메이션과 연동 (MeleeAttack.cs)

    private void Start()
    {
        pocketSlot = this.GetComponentsInChildren<Button>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SlotSwap(EQUIP_STATE.SLOT_1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SlotSwap(EQUIP_STATE.SLOT_2);
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            SlotSwap(EQUIP_STATE.SLOT_3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SlotSwap(EQUIP_STATE.SLOT_4);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SlotSwap(EQUIP_STATE.SLOT_5);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SlotSwap(EQUIP_STATE.SLOT_6);
    }

    void SlotSwap(EQUIP_STATE num)
    {
        if (preNum == EQUIP_STATE.HANDS_FREE)
        { // 비무장 상태일 때
            handsObj.SetActive(true);

            pocketSlot[(int)num].image.color = Color.blue;
            preNum = num;
            equipped = true;
        }
        else if (preNum == num)
        { // 무장 상태 시, 비무장 상태로
            handsObj.SetActive(false);

            pocketSlot[(int)num].image.color = Color.white;
            preNum = EQUIP_STATE.HANDS_FREE;
            equipped = false;
        }
        else
        { // 이전 무장은 비활성화, 스왑한 무기는 활성화
            handsObj.SetActive(false);

            pocketSlot[(int)preNum].image.color = Color.white;
            pocketSlot[(int)num].image.color = Color.blue;
            preNum = num;
            equipped = true;
        }
    }
}
