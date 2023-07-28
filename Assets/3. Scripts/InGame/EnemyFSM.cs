using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    private GameObject m_playerTarget;

    NavMeshAgent nav;
    Rigidbody rigid;
    Animator anim;

    bool isChase;
    bool isAttaked;

    public bool GetIsAttacked() { return isAttaked; }
    public void SetIsAttacked(bool bAttack) { isAttaked = bAttack; }

    public int iMaxHealth;
    public int iCurHealth;

    GameObject m_goGrandChild;          //���� ������Ʈ
    SkinnedMeshRenderer m_skinMesh;     //��Ų �Ž�
    Material m_returnMat;                  //���� �÷�
    public Material m_DmgMat;                  //������ ���׸���

    [SerializeField]
    private MonsterData monsterData;

    private int m_iHP;
    private int m_iMaxHP;

    [SerializeField]
    private GameObject[] skillObjects;

    [SerializeField]
    private GameObject SpinTarget;

    bool isBoss;
    bool bBossPhase2;
    float fSkillCoolTimer;      //��ų ��Ÿ�̸�
    float fSkillTimer;          //��ų ��� �� ������� Ÿ�̸�
    float fStartPhase2Rate;     //������2 �Ѿ�� ����

    [SerializeField]
    private AudioSource Phase2SndPlayer;

    [SerializeField]
    private AudioSource BossDeadSndPlayer;

    void Awake()
    {
        isAttaked = false;
        isChase = false;
        m_playerTarget = GameManager.gm.playerCtrl.gameObject;
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);

        GameObject goChild = this.transform.GetChild(0).gameObject;

        for (int idx = 1; idx < goChild.transform.childCount; idx++)
        {
            if (goChild.transform.GetChild(idx).gameObject.activeSelf)
            {
                m_goGrandChild = goChild.transform.GetChild(idx).gameObject;
                if(m_goGrandChild.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    m_skinMesh = m_goGrandChild.GetComponent<SkinnedMeshRenderer>();
                    m_returnMat = m_skinMesh.material;
                }
            }
        }
    }

    void Start()
    {
        monsterData = GetComponent<EnemyInfo>().monsterData;
        nav.speed = monsterData.fMonsterMoveSpeed;
        m_iHP = monsterData.iMonsterHP;
        m_iMaxHP = m_iHP;
        isBoss = monsterData.bBoss;
        bBossPhase2 = false;
        //�Ѿ�� ������ �׶� �׶� �ٸ�
        fStartPhase2Rate = Random.Range(0.25f, 0.35f);
    }

    public void InitInfo()
    {
        isChase = false;
        monsterData = GetComponent<EnemyInfo>().monsterData;
        nav.speed = monsterData.fMonsterMoveSpeed;
        m_iHP = monsterData.iMonsterHP;
        m_iMaxHP = m_iHP;
        isBoss = monsterData.bBoss;
        bBossPhase2 = false;
        fSkillCoolTimer = 0;
        fSkillTimer = 0;

        rigid.rotation = Quaternion.identity;

        rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationZ;

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        if (isBoss)
            anim.SetTrigger("isWalking");
        else
            anim.SetBool("isRunning", true);

        nav.enabled = true;
        isChase = true;
    }

    void Update()
    {
        if(isChase && nav.enabled)
            nav.SetDestination(m_playerTarget.transform.position);

        if(isBoss)
        {
            SkillCoolTimer();
            CheckActiveSkillObject();
            CheckPhase2();
        }
    }

    public void StartAttack()
    {
        StartCoroutine(ActiveAttack());
    }

    IEnumerator ActiveAttack()
    {
        float fAttkSpeed = monsterData.fMonsterAttkSpeed;

        yield return new WaitForSeconds(fAttkSpeed);

        isAttaked = false;
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.angularVelocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackRange" && nav.enabled)
        {
            int AttkPower = other.GetComponent<AttackRange>().turretData.iTurretAttkPower + GameManager.gm.iTurretAttk;

            m_iHP -= AttkPower;

            Vector3 reactVec = transform.position - other.gameObject.transform.position;

            StartCoroutine(OnDamage(reactVec));
        }
    }

    IEnumerator OnDamage(Vector3 i_reactVec)
    {
        m_skinMesh.material = m_DmgMat;

        yield return new WaitForSeconds(0.25f);
        m_skinMesh.material = m_returnMat;

        //�����Ű��
        if(m_iHP <= 0)
        {
            if (isBoss)
                BossDeadMotion();
            else
                NormalDeadMotion(i_reactVec);

            yield return new WaitForSeconds(2.0f);
            OnDisable();
        }
    }

    void NormalDeadMotion(Vector3 i_reactVec)
    {
        //�� ������ Ȯ���� ���� ����Ʈ����
        DropHealItem();

        //ĳ���� ����ġ�߰�
        GameManager.gm.ObtainEXP(monsterData.iExpPoint);

        isChase = false;
        nav.enabled = false;
        anim.SetBool("isDead1", true);

        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        i_reactVec = i_reactVec.normalized;
        i_reactVec += Vector3.up * 3;

        rigid.freezeRotation = false;
        rigid.AddForce(i_reactVec * 2, ForceMode.Impulse);

        i_reactVec = new Vector3(1.5f, 0, 0);

        rigid.AddTorque(i_reactVec * 30, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        //���� �ʱ�ȭ ���� ���ߵ�
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        gameObject.SetActive(false);
    }

    public MonsterData GetMonsterData()
    {
        return monsterData;
    }

    void DropHealItem()
    {
        //���Ȯ���� �ް�
        int dropRate = monsterData.iHealDropRate;

        int randomNum = Random.Range(1, 101);

        //���Ȯ������ ����Ȯ���� ���ƾߵ�
        if (randomNum <= dropRate)
        {
            GameObject intantItem = GameManager.gm.itemPool.Get();
            intantItem.transform.position = new Vector3 (transform.position.x, 1f, transform.position.z);
        }
    }

    //�������� �Լ���//
    void BossDeadMotion()
    {
        //������ �� ��� ������
        //�� ������ Ȯ���� ���� ����Ʈ����
        //DropHealItem();

        //���� �״� ���� �߰�
        bool isMute = OptionSetting.os.GetIsMute();

        if (!isMute)
        {
            BossDeadSndPlayer.gameObject.SetActive(true);
            BossDeadSndPlayer.Play();
        }

        //ĳ���� ����ġ�߰�
        GameManager.gm.ObtainEXP(monsterData.iExpPoint);

        isChase = false;
        nav.enabled = false;

        //��ų Ȱ��ȭ �Ǿ������� ��Ȱ��ȭ
        if(SpinTarget.activeSelf)
        {
            int maxSize = skillObjects.Length;

            for (int idx = 0; idx < maxSize; idx++)
            {
                skillObjects[idx].SetActive(false);
            }

            SpinTarget.SetActive(false);
        }

        //�״� ��� ���ϱ�
        //�״� ����Ҷ� ��ü ����¡�ϱ�
        rigid.constraints = RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionZ;
        rigid.freezeRotation = true;

        //anim Ȱ��ȭ
        anim.SetTrigger("isDead");

        //�״� ��� ���Ҷ� ����Ʈ Ȱ��ȭ�ϱ�
        StartCoroutine(GameManager.gm.bossEff.StartDeadEff(transform.position));
    }

    //�Ź� �˻��ϰ��ϱ�
    void SkillCoolTimer()
    {
        if (!nav.enabled) return;

        if(SpinTarget.activeSelf)
        {
            fSkillTimer += Time.deltaTime;

            if(fSkillTimer >= 20.0f)
            {
                fSkillTimer = 0;

                int MaxSize = skillObjects.Length;

                for(int idx = 0; idx < MaxSize; idx++)
                {
                    if(skillObjects[idx].activeSelf)
                    {
                        skillObjects[idx].SetActive(false);
                    }
                }

                SpinTarget.SetActive(false);
            }
        }

        else 
        {
            fSkillCoolTimer += Time.deltaTime;

            if(fSkillCoolTimer >= 30.0f)
            {
                fSkillCoolTimer = 0;

                //��ų Ȱ��ȭ
                ActiveSkill();
            }
        }

    }

    //�Ź��˻��ϰ� �ϱ�
    void CheckActiveSkillObject()
    {
        if (!SpinTarget.activeSelf &&
            !nav.enabled) 
                return;

        //���⸦ �����ؾߵȴ�
        bool bCheckActive = false;

        int MaxSize = skillObjects.Length;

        for(int idx = 0; idx < MaxSize; idx++)
        {
            if (skillObjects[idx].activeSelf)
            {
                bCheckActive = true;
                break;
            }
        }

        if(!bCheckActive)
        {
            SpinTarget.SetActive(false);
            fSkillTimer = 0;
        }
    }

    void ActiveSkill()
    {
        isChase = false;
        nav.enabled = false;

        anim.SetTrigger("isSkillMotion");

        StartCoroutine(SkillObjectActive());
    }

    IEnumerator SkillObjectActive()
    {
        int maxSize = skillObjects.Length;

        for (int idx = 0; idx < maxSize; idx++)
        {
            yield return new WaitForSeconds(0.5f);

            if(!SpinTarget.activeSelf) 
                SpinTarget.SetActive(true);

            skillObjects[idx].SetActive(true);
        }

        yield return new WaitForSeconds(2f);


        if (bBossPhase2)
            anim.SetTrigger("isRunning");
        else
            anim.SetTrigger("isWalking");

        isChase = true;
        nav.enabled = true;

        yield return null;
    }

    //�Ź��˻��ϱ�
    void CheckPhase2()
    {
        float checkRate = (float)m_iHP / (float)m_iMaxHP;

        if(checkRate <= fStartPhase2Rate &&
           bBossPhase2 == false)
        { 
            StartCoroutine(StartPahse2());
        }
    }

    IEnumerator StartPahse2()
    {
        //���� ����

        bool isMute = OptionSetting.os.GetIsMute();

        if(!isMute)
        {
            Phase2SndPlayer.gameObject.SetActive(true);
            Phase2SndPlayer.Play();
        }

        bBossPhase2 = true;
        //������ 2�� ������

        isChase = false;
        nav.enabled = false;

        anim.SetTrigger("isShouting");

        yield return new WaitForSeconds(3f);

        anim.SetTrigger("isRunning");

        isChase = true;
        nav.enabled = true;
        nav.speed = nav.speed * 2f;
    }
}
