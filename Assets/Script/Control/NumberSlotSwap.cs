using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSlotSwap : MonoBehaviour
{
    public GameObject handsObj;
    enum EQUIP_STATE { HANDS_FREE = -1, SLOT_1, SLOT_2, SLOT_3, SLOT_4, SLOT_5, SLOT_6, POCKET_MAX = 6 };

    public Button[] pocketSlot = new Button[ (int)EQUIP_STATE.POCKET_MAX ];
    private RawImage rawImage;
    private EQUIP_STATE preNum = EQUIP_STATE.HANDS_FREE;

    public static bool equipped; // 스왑애니메이션과 연동 (MeleeAttack.cs)

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

            rawImage = pocketSlot[(int)num].GetComponentInChildren<RawImage>();
            rawImage.color = Color.green;
            preNum = num;
            equipped = true;
        }
        else if (preNum == num)
        { // 무장 상태 시, 비무장 상태로
            handsObj.SetActive(false);

            rawImage = pocketSlot[(int)num].GetComponentInChildren<RawImage>();
            rawImage.color = Color.white;
            preNum = EQUIP_STATE.HANDS_FREE;
            equipped = false;
        }
        else
        { // 이전 무장은 비활성화, 스왑한 무장은 활성화
            handsObj.SetActive(false);

            rawImage = pocketSlot[(int)preNum].GetComponentInChildren<RawImage>();
            rawImage.color = Color.white;
            rawImage = pocketSlot[(int)num].GetComponentInChildren<RawImage>();
            rawImage.color = Color.green;
            preNum = num;
            equipped = true;
        }
    }
}
