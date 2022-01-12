using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator m_AniMeleeAttack;
    public GameObject m_playerHead;

    private bool rayCollision = false;  // 플레이어가 보는 방향에 채집물이 있는지 여부
    private const float RAY_COLLISION_MAX_DISTANCE = 2.0f;    // 광선충돌 최대 거리
    private bool isAnimFinished = true;

    ItemManager.Item item;

    // 표식 컨트롤 변수
    private int Combo = 0;
    const int MAX_COMBO = 4;
    Vector3 markPosition;
    Quaternion markRotation;
    private float treeRad = 0.5f;
    private float markRotAng = 0;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isAnimFinished)
        {
            if (NumberSlotSwap.equipped)
            {
                isAnimFinished = false;
                m_AniMeleeAttack.SetBool("B_MouseLeftClick", true);
                StopCoroutine("RaycastControl");
                StartCoroutine("RaycastControl");
            }
        }

        if (NumberSlotSwap.equipped)
        {
            m_AniMeleeAttack.SetBool("B_Equipped", true);
        }
        else
        {
            m_AniMeleeAttack.SetBool("B_Equipped",false);
        }

    }

    IEnumerator RaycastControl()
    {
        yield return new WaitForSeconds(0.5f); // 근접공격 타격 타이밍

        int layerMask = 1 << LayerMask.NameToLayer("AttackFarming");
        RaycastHit hitInfo;

        rayCollision = Physics.Raycast(
           m_playerHead.transform.position,
           m_playerHead.transform.forward,
           out hitInfo,
           RAY_COLLISION_MAX_DISTANCE,
           layerMask
           );

        if (rayCollision)
        {
            if (hitInfo.transform.tag == "Tree")
            {
                item = ItemManager.Instance.ConfigItem(hitInfo.collider.tag.ToString());
                ItemManager.Instance.InsertItem(item, 1);
            }
            else if (hitInfo.transform.tag == "TreeMark")
            {
                MarkControl(hitInfo);
            }
        }


        m_AniMeleeAttack.SetBool("B_MouseLeftClick", false);
        yield return new WaitForSeconds(0.7f); // 근접공격 애니메이션 종료
        isAnimFinished = true;

    }

    void MarkControl(RaycastHit _hitInfo)
    {
        GameObject hitInfo = _hitInfo.transform.gameObject;
        GameObject tree = hitInfo.transform.parent.gameObject;
        markPosition = hitInfo.transform.localPosition;
        markRotation = hitInfo.transform.rotation;

        if (Combo < MAX_COMBO)
            Combo++;

        item = ItemManager.Instance.ConfigItem(hitInfo.tag.ToString());
        ItemManager.Instance.InsertItem(item, 1 + Combo);

        markRotAng += (float)System.Math.PI / 10;
        markPosition.x = (float)System.Math.Sin(markRotAng) * tree.transform.localScale.x * treeRad;
        markPosition.z = -((float)System.Math.Cos(markRotAng)) * tree.transform.localScale.x * treeRad;
        markRotation.SetEulerAngles(0, -markRotAng, 0);
        // 로컬포지션은 set이 안되는듯 -> 부모포지션 + 로컬포지션변동
        hitInfo.transform.SetPositionAndRotation(tree.transform.position + markPosition, markRotation);



    }
}
