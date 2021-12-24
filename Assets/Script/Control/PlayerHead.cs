using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour
{
    [SerializeField]
    Canvas m_gatheringUI;
    [SerializeField]
    Canvas m_gatheringClearUI;

    public float turnSpeed; // 마우스 회전 속도
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )

    private bool rayCollision = false;  // 플레이어가 보는 방향에 채집물이 있는지 여부
    private const float RAY_COLLISION_MAX_DISTANCE = 2.0f;    // 광선충돌 최대 거리
    private Text raycastTxt;
    private Text gatheringClearTxt;
    private GameObject clearObj;

    private void Start()
    {
        //UI들 초기에 비표시상태로 시작
        m_gatheringUI.enabled = false;
        m_gatheringClearUI.enabled = false;
        // 
        raycastTxt = GameObject.Find("GatheringText").GetComponent<Text>();
        gatheringClearTxt = GameObject.Find("GatheringClearText").GetComponent<Text>();
    }

    void Update()
    {
        RaycastControl();   // 플레이어 보는 방향에 광선충돌
        MouseRotation();    // 마우스의 움직임에 따라 카메라를 회전
        KeyboardControl();  // 키보드 입력 컨트롤
    }

    void MouseRotation()
    {
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
    void KeyboardControl()
    {

    }
    void RaycastControl()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Gathering"); 
        RaycastHit hitInfo;

        rayCollision = Physics.Raycast(this.transform.position,
           transform.forward,
           out hitInfo,
           RAY_COLLISION_MAX_DISTANCE,
           layerMask
           );  // 플레이어 머리에서, 보는방향으로, 2.0f거리에, 채집물 레이어와 충돌할경우

        if (rayCollision)
        {   // <E키를 눌러 상호작용> 텍스트UI 노출
            m_gatheringUI.enabled = true;
            switch (hitInfo.collider.tag)
            {
                case "Tree":
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
                gatheringClearTxt.text += "\n" + hitInfo.collider.tag.ToString() + "+1";
                m_gatheringClearUI.enabled = true;

                InsertItemToInventory(hitInfo.collider.tag.ToString());

                clearObj = hitInfo.transform.gameObject;
                Destroy(clearObj);
            }
        }
        else
            m_gatheringUI.enabled = false;
    }


    private const int BAG_MAX = 24;
    public Button[] SlotImage = new Button[BAG_MAX];
    private bool[] isFilled = new bool[BAG_MAX];

    public void InsertItemToInventory(string str1)
    {
        for (int i = 0; i < BAG_MAX; i++)
        {
            if (isFilled[i] == false)
            {
                ColorBlock cb = SlotImage[i].colors;
                Color newColor = Color.green;
                cb.normalColor = newColor;
                SlotImage[i].colors = cb;

                isFilled[i] = true;
                break;
            }
        }
    }
}
