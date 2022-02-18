using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour
{
    public float turnSpeed; // ���콺 ȸ�� �ӵ�
    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )

    private bool rayCollision = false;  // �÷��̾ ���� ���⿡ ä������ �ִ��� ����
    private const float RAY_COLLISION_MAX_DISTANCE = 2.0f;    // �����浹 �ִ� �Ÿ�

    void Update()
    {
        RaycastControl();   // �÷��̾� ���� ���⿡ �����浹
        if (!UIControl.Instance.isInventoryOn)
        {// �κ��丮 UI ���� �������� ȸ��
            MouseRotation();    // ���콺�� �����ӿ� ���� ī�޶� ȸ��
        }
    }

    void MouseRotation()
    {
        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
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
           );  // �÷��̾� �Ӹ�����, ���¹�������, 2.0f�Ÿ���, ä���� ���̾�� �浹�Ұ��

        UIControl.Instance.RaycastUI(rayCollision, hitInfo);
    }
}
