using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletAttackRange : MonoBehaviour
{
    public GameObject MaxRange;
    public GameObject CurRange;
    public GameObject AttkRange;

    GameObject intantMaxRange;
    GameObject intantCurRange;
    GameObject intantAttkRange;
    float fMaxDistance = 0;         //ó�� �߻����� �� �÷��̾�� ���������� �Ÿ� ����
    float fPerMaxRange = 0;         // ������� ��� 1���δ� �ִ� ũ������

    GameObject cannon;

    bool m_bActiveAttk;

    //curRange�� ������� �ʱ�ȭ�Ѵ�.
    GameObject initCurRange;

    Vector3 m_vStartPos;

    bool m_ActiveRange;

    bool m_bCreated; //�̹� ������ Ŭ�������� Ȯ��

    public void StartBullet(Vector3 i_vTargetPos, Quaternion i_qTargetRot, Vector3 i_vStartPos, TurretData i_tTurretData)
    {
        //�θ��� ��ũ��Ʈ �����;ߵ�
        cannon = transform.parent.gameObject;

        intantMaxRange = Instantiate(MaxRange, i_vTargetPos, i_qTargetRot, GameManager.gm.bulletPool.transform);
        intantCurRange = Instantiate(CurRange, i_vTargetPos, i_qTargetRot, GameManager.gm.bulletPool.transform);
        intantAttkRange = Instantiate(AttkRange, i_vTargetPos, i_qTargetRot, GameManager.gm.bulletPool.transform);

        //���� ���� ��Ȱ��ȭ
        intantAttkRange.SetActive(false);
        intantAttkRange.GetComponent<AttackRange>().turretData = i_tTurretData;

        initCurRange = CurRange;

        //���ݹ��� �߰��� ���� �ڵ� ����
        //���� ũ�⿡�� �󸶳� �����Ǵ����� ������ ��
        Vector3 vMaxScale = MaxRange.transform.localScale;

        intantMaxRange.transform.localScale = new Vector3(vMaxScale.x + GameManager.gm.fTurretRange, vMaxScale.y, vMaxScale.z + GameManager.gm.fTurretRange);
        intantAttkRange.transform.localScale = intantMaxRange.transform.localScale;

        Vector3 planarTarget = new Vector3(i_vTargetPos.x, 0, i_vTargetPos.z);
        Vector3 planarCannonPos = new Vector3(i_vStartPos.x, 0, i_vStartPos.z);
        fMaxDistance = Vector3.Distance(planarTarget, planarCannonPos);
        fPerMaxRange = intantMaxRange.transform.localScale.x / 100f; 

        m_vStartPos = i_vStartPos;

        m_ActiveRange = true;
        m_bCreated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ActiveRange == false) return;

        if(intantCurRange.transform.localScale.x < MaxRange.transform.localScale.x + GameManager.gm.fTurretRange &&
           intantCurRange.transform.localScale.z < MaxRange.transform.localScale.z + GameManager.gm.fTurretRange)
        {
            Scaling(new Vector3(fPerMaxRange * MovePerDistance(m_vStartPos), CurRange.transform.localScale.y, fPerMaxRange * MovePerDistance(m_vStartPos)));
        }

        else
        {
            if (m_bActiveAttk == false && intantMaxRange.activeSelf == true)
            {
                StartCoroutine(ActiveAttack());
            }
        }
    }

    private void Scaling(Vector3 newScale)
    {
        intantCurRange.transform.localScale = newScale;
    }

    float MovePerDistance(Vector3 i_vStartPos)
    {
        Vector3 planarCannonPos = new Vector3(i_vStartPos.x, 0, i_vStartPos.z);
        Vector3 planarMissilePos = new Vector3(transform.position.x, 0, transform.position.z);

        float fMoveDistance = Vector3.Distance(planarCannonPos, planarMissilePos);

        return ( fMoveDistance / fMaxDistance ) * 100f;
    }

    IEnumerator ActiveAttack()
    {
        m_bActiveAttk = true;
        intantAttkRange.SetActive(true);
        //����Ʈ����
        GameObject ef = GameManager.gm.ExplosionEffPool.Get(intantMaxRange.transform.position);
        //print("���� ��ȣ ���Ե���");

        bool isMute = OptionSetting.os.GetIsMute();

        if (!isMute)
            StartCoroutine(ef.GetComponent<ExplosionEffect>().StartSound());

        yield return new WaitForSeconds(0.05f);
        intantMaxRange.SetActive(false);
        intantCurRange.SetActive(false);
        intantAttkRange.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        //���⼭ �ʱ�ȭ �ڵ� �־��ָ�ɵ��Դ�
        OnDisable();
    }

    public void InitRange(Vector3 i_vTargetPos, Quaternion i_qTargetRot,  Vector3 i_vStartPos, TurretData i_tTurretData)
    {
        intantMaxRange.SetActive(true);
        intantCurRange.SetActive(true);
        m_vStartPos = i_vStartPos;

        //��ġ �����س���
        this.gameObject.transform.position = i_vStartPos;

        Vector3 planarTarget = new Vector3(i_vTargetPos.x, 0, i_vTargetPos.z);
        Vector3 planarCannonPos = new Vector3(i_vStartPos.x, 0, i_vStartPos.z);
        intantMaxRange.transform.position = planarTarget;
        intantCurRange.transform.position = planarTarget;
        intantAttkRange.transform.position = planarTarget;
        intantMaxRange.transform.rotation = i_qTargetRot;
        intantCurRange.transform.rotation = i_qTargetRot;
        intantAttkRange.transform.rotation = i_qTargetRot;
        fMaxDistance = Vector3.Distance(planarTarget, planarCannonPos);

        //���⼭�� ���ݹ��� ����� ���ߵ�
        Vector3 vMaxScale = MaxRange.transform.localScale;

        intantMaxRange.transform.localScale = new Vector3(vMaxScale.x + GameManager.gm.fTurretRange, vMaxScale.y, vMaxScale.z + GameManager.gm.fTurretRange);
        intantAttkRange.transform.localScale = intantMaxRange.transform.localScale;

        m_ActiveRange = true;
        intantAttkRange.GetComponent<AttackRange>().turretData = i_tTurretData;
    }

    public bool GetIsCreated()
    {
        return m_bCreated;
    }

    private void OnDisable()
    {
        //��ġ �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //���� �ʱ�ȭ
        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        intantCurRange.transform.localScale = initCurRange.transform.localScale;
        m_bActiveAttk = false;
        m_ActiveRange = false;

        this.gameObject.SetActive(false);
        intantAttkRange.SetActive(false);
    }
}
