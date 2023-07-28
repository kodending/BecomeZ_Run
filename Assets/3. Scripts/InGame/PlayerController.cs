using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joy;
    //ĳ���� ����

    public float fSpeed { get; set; }   //�̵��ӵ�

    public int iCurHP { get; set; }     //���� ü��

    public int iMaxHP { get; set; }     //�ִ� ü��

    public int iDef { get; set; }       //����

    public int iLevel { get; set; }     //ĳ���� ����

    Rigidbody rigid;
    Animator anim;
    Vector3 moveVec;

    public int iPlayerLevel;

    public PlayerData playerData;

    [SerializeField]
    private Text LevelTxt;

    [SerializeField]
    private Text CurHPTxt;

    [SerializeField]
    private Text MaxHPTxt;

    [SerializeField]
    private Image HPBarImg;

    [SerializeField]
    private AudioSource[] sfxPlayer;

    enum SOUNDTYPE
    {
        GETPOTION,
        DAMAGED
    }

    public enum DEADTYPE
    {
        TURRET,
        ZOMBIE
    }

    DEADTYPE deadType;

    Vector3 vBulletRangePos;

    public bool isDead { get; set; }

    [SerializeField]
    Text gameOverTxt;

    SkinnedMeshRenderer m_skinMesh;     //��Ų �Ž�
    Material m_returnMat;                  //���� �÷�
    public Material m_DmgMat;                  //������ ���׸���
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        moveVec = new Vector3(0, 0, 0);
        iPlayerLevel = 1;
        playerData = Resources.Load<PlayerData>("ScriptableData/Player/player");
        isDead = false;

        GameObject goChild = transform.GetChild(0).gameObject;
        goChild = goChild.transform.GetChild(0).gameObject;

        for (int idx = 1; idx < goChild.transform.childCount; idx++)
        {
            if (goChild.transform.GetChild(idx).gameObject.activeSelf)
            {
                GameObject go = goChild.transform.GetChild(idx).gameObject;
                if(go.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    m_skinMesh = go.GetComponent<SkinnedMeshRenderer>();
                    m_returnMat = m_skinMesh.material;
                }
            }
        }
    }

    void Start()
    {
        //���� �÷��̾� ���� ����
        fSpeed = playerData.fMoveSpeed;
        iCurHP = playerData.iHP;
        iMaxHP = iCurHP;
        iDef = playerData.iDefense;
        iLevel = 1;

        //���� ������Ű��
        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            sfxPlayer[idx].Stop();
        }

        //�÷��̾� ���� ����(�ؽ�Ʈ ������)
        RefreshPlayerInfo();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // 1. Input Value
        float x = joy.Horizontal;
        float z = joy.Vertical;

        // 2. Move Position
        moveVec = new Vector3(x, 0, z) * fSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec);

        if (moveVec.sqrMagnitude == 0)
        {
            anim.SetBool("isRunning", false);
            rigid.angularVelocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
            //���������� ������ Ȱ��ȭ ��Ű��
            rigid.constraints = RigidbodyConstraints.FreezePositionX |
                                RigidbodyConstraints.FreezePositionY |
                                RigidbodyConstraints.FreezePositionZ;
            rigid.freezeRotation = true;
            return;
        }

        else
        {
            rigid.constraints = RigidbodyConstraints.None;
            rigid.freezeRotation = true;
            anim.SetBool("isRunning", true);
        }

        // 3. Move Rotation
        Quaternion dirQuat = Quaternion.LookRotation(moveVec);
        Quaternion moveQuat = Quaternion.Slerp(rigid.rotation, dirQuat, 0.3f);
        rigid.MoveRotation(moveQuat);
    }

    void LateUpdate()
    {
        if (isDead)
        {
            anim.SetFloat("Move", 0);
            return;
        }

        anim.SetFloat("Move", moveVec.sqrMagnitude);    
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyFSM ef = collision.collider.GetComponent<EnemyFSM>();

            bool bAttaked = ef.GetIsAttacked();

            if (!bAttaked)
            {
                int AttkPower = ef.GetMonsterData().iMonsterAttkPower;

                DamagedCalc(AttkPower);

                StartCoroutine(OnDamage(DEADTYPE.ZOMBIE));

                ef.StartAttack();
                ef.SetIsAttacked(true);
            }
        }
        
        //���� ��ų ���ݿ� �¾������
        if (collision.collider.CompareTag("BossRemote"))
        {
            GameObject goParent = collision.collider.gameObject.transform.parent.gameObject;
            goParent = goParent.transform.parent.gameObject;
            goParent = goParent.transform.parent.gameObject;

            EnemyFSM ef = goParent.GetComponent<EnemyFSM>();

            int AttkPower = Mathf.FloorToInt((float)ef.GetMonsterData().iMonsterAttkPower * 0.5f);

            collision.collider.gameObject.SetActive(false);

            DamagedCalc(AttkPower);

            StartCoroutine(OnDamage(DEADTYPE.ZOMBIE));

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackRange")
        {
            int AttkPower = other.GetComponent<AttackRange>().turretData.iTurretAttkPower;

            vBulletRangePos = other.gameObject.transform.position;

            DamagedCalc(AttkPower);

            StartCoroutine(OnDamage(DEADTYPE.TURRET));
        }
    }

    void DamagedCalc(int i_iDamagePoint)
    {
        //�����ҰŸ� ���� �� ��Ȱ��ȭ�ϸ��

        int AttkPower = i_iDamagePoint;
        int playerDef = iDef;

        //���� ������ - ���� ����Ű��
        //���� - ���� ���� ���
        int calDamaged = AttkPower - playerDef;

        if (calDamaged > 0)
        {
            iCurHP = iCurHP - calDamaged;
        }

        RefreshPlayerInfo();
    }

    IEnumerator OnDamage(DEADTYPE i_Type)
    {
        //�ǰݻ��� �ֱ�
        StartSound(SOUNDTYPE.DAMAGED);

        //�ǰݽ� ���� ��� �ٲٱ�
        m_skinMesh.material = m_DmgMat;

        yield return new WaitForSecondsRealtime(0.25f);
        m_skinMesh.material = m_returnMat;

        //�����Ű��
        if (iCurHP <= 0)
        {
            iCurHP = 0;

            RefreshPlayerInfo();

            isDead = true;
            //anim.SetBool("isDead1", true);
            joy.gameObject.SetActive(false);

            switch (i_Type)
            {
                case DEADTYPE.TURRET:
                    //�� �����ؾߵ�

                    anim.SetTrigger("isDead");
                    //anim.SetBool("isRunning", false);

                    vBulletRangePos = vBulletRangePos.normalized;
                    vBulletRangePos += Vector3.up * 7;

                    rigid.freezeRotation = false;
                    rigid.AddForce(vBulletRangePos * 4, ForceMode.Impulse);

                    vBulletRangePos = new Vector3(1.5f, 0, 0);

                    rigid.AddTorque(vBulletRangePos * 30, ForceMode.Impulse);
                    break;

                case DEADTYPE.ZOMBIE:

                    Time.timeScale = 0.3f;
                    anim.SetTrigger("isDead2");

                    break;
            }

            yield return new WaitForSecondsRealtime(1f);

            //��� �޼��� Ű��
            gameOverTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.GAMEOVER);
            gameOverTxt.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(3f);

            //��� �޼��� UI ����
            gameOverTxt.gameObject.SetActive(false);

            GameManager.gm.gameState = GameManager.GAMESTATE.DEAD;
            Time.timeScale = 0;

            //���� ����â ����
            GameManager.gm.OnPauseBtn();
        }

        else yield return null;
            
    }

    //�ΰ��� ȭ�鿡�� ������ �ؽ�Ʈ ������ �Է�
    public void RefreshPlayerInfo()
    {
        LevelTxt.text = string.Format("{0:D1}", iLevel);
        CurHPTxt.text = string.Format("{0:D1}", iCurHP);
        MaxHPTxt.text = string.Format("{0:D1}", iMaxHP);

        HPBarImg.fillAmount = (float)iCurHP / (float)iMaxHP;
    }

    public void AddHPPotion(int i_iHP)
    {
        StartSound(SOUNDTYPE.GETPOTION);

        iCurHP += i_iHP;

        if (iCurHP >= iMaxHP)
                iCurHP = iMaxHP;

        RefreshPlayerInfo();
    }

    void StartSound(SOUNDTYPE sndType)
    {
        switch(sndType)
        {
            case SOUNDTYPE.GETPOTION:
                sfxPlayer[(int)sndType].Play();
                break;

            case SOUNDTYPE.DAMAGED:
                sfxPlayer[(int)sndType].Play();
                break;
        }
    }
}
