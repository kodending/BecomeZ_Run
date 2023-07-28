using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CannonFSM : MonoBehaviour
{
    public Transform bulletPos;

    //목표 위치
    private GameObject m_playerTarget;

    //목표위치까지 날려버리기 //날리는 각도입니다 이친구는
    public float m_InitAngle = 45;

    Vector3 lookDir;

    float m_fTimer;
    //public float m_fAttackTime;

    [SerializeField]
    private TurretData turretData;

    [SerializeField]
    private AudioSource fireSndPlayer;

    void Start()
    {
        turretData = GetComponent<TurretInfo>().turretData;
        m_playerTarget = GameManager.gm.playerCtrl.gameObject;
        fireSndPlayer.Pause();
        fireSndPlayer.volume = OptionSetting.os.GetIsMute() ? 0 : 0.3f;
    }

    public Vector3 GetStartPos()
    {
        return bulletPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.playerCtrl.isDead) return;

        lookDir = (m_playerTarget.transform.position - transform.position).normalized;

        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);

        transform.rotation = Quaternion.Lerp(from, to, Time.fixedDeltaTime);

        m_fTimer += Time.deltaTime;

        if (m_fTimer >= turretData.fTurretAttkTime - GameManager.gm.fTurretSpeed)
        {
            Use(m_playerTarget.transform.position, m_playerTarget.transform.rotation, bulletPos.position);

            m_fTimer = 0;
        }
    }

    public void Use(Vector3 i_vPosition, Quaternion i_qRot, Vector3 i_vStartPos)
    {
        StartCoroutine(Shot(i_vPosition, i_qRot, i_vStartPos));
    }

    IEnumerator Shot(Vector3 i_vPosition, Quaternion i_qRot, Vector3 i_vStartPos)
    {
        StartCoroutine(StartFireSound());

        //1. 총알발사
        GameObject intantBullet = GameManager.gm.bulletPool.Get(0, this.gameObject, i_vPosition, i_vStartPos);
        float randX = UnityEngine.Random.Range(-3.0f, 3.0f);
        float randZ = UnityEngine.Random.Range(-3.0f, 3.0f);
        intantBullet.transform.position = bulletPos.position;
        intantBullet.transform.rotation = bulletPos.rotation;
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();

        Vector3 TargetPos = new Vector3(i_vPosition.x + randX, i_vPosition.y, i_vPosition.z + randZ);
        Vector3 bulletVec = GetVelocity(bulletPos.position, TargetPos, m_InitAngle);
        bulletRigid.AddForce(bulletVec, ForceMode.Impulse);             //일정 힘 날라가는거
        bulletRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);      //일정 회전 도는거

        if(intantBullet.GetComponent<BulletAttackRange>().GetIsCreated() == false)
        {
            intantBullet.GetComponent<BulletAttackRange>().StartBullet(TargetPos, i_qRot, i_vStartPos, turretData);
        }
        else
        {
            intantBullet.GetComponent<BulletAttackRange>().InitRange(TargetPos, i_qRot, i_vStartPos, turretData);
        }

        yield return null;
    }

    IEnumerator StartFireSound()
    {
        //사운드넣기
        fireSndPlayer.Play();

        yield return new WaitForSecondsRealtime(1.2f);

        fireSndPlayer.Stop();
    }

    public Vector3 GetVelocity(Vector3 bullet, Vector3 target, float initAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(bullet.x, 0, bullet.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = bullet.y - target.y;

        float initVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initVelocity * Mathf.Sin(angle), initVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > bullet.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
}
