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
    float fMaxDistance = 0;         //처음 발사했을 때 플레이어와 대포사이의 거리 측정
    float fPerMaxRange = 0;         // 백분율로 계산 1프로당 최대 크기인지

    GameObject cannon;

    bool m_bActiveAttk;

    //curRange를 원래대로 초기화한다.
    GameObject initCurRange;

    Vector3 m_vStartPos;

    bool m_ActiveRange;

    bool m_bCreated; //이미 생성된 클래스인지 확인

    public void StartBullet(Vector3 i_vTargetPos, Quaternion i_qTargetRot, Vector3 i_vStartPos, TurretData i_tTurretData)
    {
        //부모의 스크립트 가져와야됨
        cannon = transform.parent.gameObject;

        intantMaxRange = Instantiate(MaxRange, i_vTargetPos, i_qTargetRot, GameManager.gm.bulletPool.transform);
        intantCurRange = Instantiate(CurRange, i_vTargetPos, i_qTargetRot, GameManager.gm.bulletPool.transform);
        intantAttkRange = Instantiate(AttkRange, i_vTargetPos, i_qTargetRot, GameManager.gm.bulletPool.transform);

        //어택 범위 비활성화
        intantAttkRange.SetActive(false);
        intantAttkRange.GetComponent<AttackRange>().turretData = i_tTurretData;

        initCurRange = CurRange;

        //공격범위 추가에 따른 코드 수정
        //원래 크기에서 얼마나 증가되는지를 넣으면 됨
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
        //이펙트연출
        GameObject ef = GameManager.gm.ExplosionEffPool.Get(intantMaxRange.transform.position);
        //print("공격 신호 들어왔따잉");

        bool isMute = OptionSetting.os.GetIsMute();

        if (!isMute)
            StartCoroutine(ef.GetComponent<ExplosionEffect>().StartSound());

        yield return new WaitForSeconds(0.05f);
        intantMaxRange.SetActive(false);
        intantCurRange.SetActive(false);
        intantAttkRange.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        //여기서 초기화 코드 넣어주면될듯함다
        OnDisable();
    }

    public void InitRange(Vector3 i_vTargetPos, Quaternion i_qTargetRot,  Vector3 i_vStartPos, TurretData i_tTurretData)
    {
        intantMaxRange.SetActive(true);
        intantCurRange.SetActive(true);
        m_vStartPos = i_vStartPos;

        //위치 보정해놓기
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

        //여기서도 공격범위 재수정 들어가야됨
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
        //위치 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //물리 초기화
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
