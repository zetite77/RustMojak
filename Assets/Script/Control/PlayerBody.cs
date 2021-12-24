using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField]
    GameObject playerBody;
    [SerializeField]
    Rigidbody rigidBody;
    [SerializeField]
    CapsuleCollider capsuleCollider;

    public float turnSpeed; // 마우스 회전 속도
    public float moveSpeed; // 이동 속도
    public float jumpForce; // 점프 강도
    public Vector3 rotForward; // 플레이어가 보는 방향

    private bool isGround = true;

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Ground")
            isGround = true;    //Ground에 닿으면 isGround는 true
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseRotation();
        KeyboardMove();
    }
    void MouseRotation()
    {
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;
       
        // 카메라 회전량을 몸통에 반영(Y축만 회전)
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    void KeyboardMove()
    {
        // WASD 키 또는 화살표키의 이동량을 측정
        Vector3 dir = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
        );

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {   // shift누르고 W입력시 더빨리 전진
            dir.z *= 2;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {   // control누르고 방향키 입력시 느린 이동
            dir.z /= 2; dir.x /= 2;
        }

        if (Input.GetButton("Jump"))
        {   //점프 키가 눌렸을 때
            if (isGround == true)
            {   //점프 중이지 않을 때
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                isGround = false;
            }
        }

        // 이동방향 * 속도 * 프레임단위 시간을 곱해서 플레이어의 트랜스폼을 이동
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
