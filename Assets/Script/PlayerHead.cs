using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour
{
    public float turnSpeed; // 마우스 회전 속도
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )

    private bool rayCollision = false;  // 플레이어가 보는 방향에 채집물이 있는지 여부
    private const float RAY_COLLISION_MAX_DISTANCE = 2.0f;    // 광선충돌 최대 거리

    void Update()
    {
        RaycastControl();   // 플레이어 보는 방향에 광선충돌
        if (!UIControl.Instance.isInventoryOn)
        {// 인벤토리 UI 꺼져 있을때만 회전
            MouseRotation();    // 마우스의 움직임에 따라 카메라를 회전
        }
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

        UIControl.Instance.RaycastUI(rayCollision, hitInfo);
    }
}
