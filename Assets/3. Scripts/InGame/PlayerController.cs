using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joy;
    //캐릭터 정보

    public float fSpeed { get; set; }   //이동속도

    public int iCurHP { get; set; }     //현재 체력

    public int iMaxHP { get; set; }     //최대 체력

    public int iDef { get; set; }       //방어력

    public int iLevel { get; set; }     //캐릭터 레벨

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

    SkinnedMeshRenderer m_skinMesh;     //스킨 매쉬
    Material m_returnMat;                  //원래 컬러
    public Material m_DmgMat;                  //데미지 머테리얼
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
        //기초 플레이어 정보 생성
        fSpeed = playerData.fMoveSpeed;
        iCurHP = playerData.iHP;
        iMaxHP = iCurHP;
        iDef = playerData.iDefense;
        iLevel = 1;

        //사운드 정지시키기
        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            sfxPlayer[idx].Stop();
        }

        //플레이어 정보 갱신(텍스트 때문에)
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
            //멈췄을때는 프리즈 활성화 시키기
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
        
        //보스 스킬 공격에 맞았을경우
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
        //무적할거면 여기 싹 비활성화하면됨

        int AttkPower = i_iDamagePoint;
        int playerDef = iDef;

        //상대방 데미지 - 방어력 계산시키기
        //방어력 - 공격 먼저 계산
        int calDamaged = AttkPower - playerDef;

        if (calDamaged > 0)
        {
            iCurHP = iCurHP - calDamaged;
        }

        RefreshPlayerInfo();
    }

    IEnumerator OnDamage(DEADTYPE i_Type)
    {
        //피격사운드 넣기
        StartSound(SOUNDTYPE.DAMAGED);

        //피격시 색깔 잠깐 바꾸기
        m_skinMesh.material = m_DmgMat;

        yield return new WaitForSecondsRealtime(0.25f);
        m_skinMesh.material = m_returnMat;

        //사망시키자
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
                    //좀 조절해야됨

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

            //사망 메세지 키기
            gameOverTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.GAMEOVER);
            gameOverTxt.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(3f);

            //사망 메세지 UI 종료
            gameOverTxt.gameObject.SetActive(false);

            GameManager.gm.gameState = GameManager.GAMESTATE.DEAD;
            Time.timeScale = 0;

            //게임 중지창 띄우기
            GameManager.gm.OnPauseBtn();
        }

        else yield return null;
            
    }

    //인게임 화면에서 떠야할 텍스트 정보들 입력
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
