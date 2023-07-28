using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public MonsterPoolManager monsterPool;

    public MonsterSpawner monsterSpawner;

    public BulletPoolManager bulletPool;

    public TurretSpawner turretSpawner;

    public TurretPoolManager turretPool;

    public EnemyFactory enemyFac;

    public TurretFactory TurretFac;

    public PlayerController playerCtrl;

    public CardManager cardManager;

    public ItemPoolManager itemPool;

    public BossEffect bossEff;

    public ExplosionEffectPoolManager ExplosionEffPool;

    [SerializeField]
    public List<GMData> gmDataList;

    [SerializeField]
    public GMData curLevelData;

    [SerializeField]
    private AudioSource PointSndPlayer;

    [SerializeField]
    private AudioSource LvUpSndPlayer;

    [SerializeField]
    private AudioSource BtnSndPlayer;

    [SerializeField]
    private GameObject GamePauseUI;

    [SerializeField]
    private Text GameEndTxt;

    [SerializeField]
    GameObject playBtn;

    [SerializeField]
    GameObject Xbtn;

    [SerializeField]
    GameObject BannerAdmob;

    public enum GAMESTATE
    {
        READY,      //�غ�
        PLAY,       //������
        PAUSE,      //��ø���
        END,        //����
        CLEAR,      //Ŭ����
        DEAD,       //����
        _MAX_
    }

    public float fGameTime;
    int iMinute;
    float fSec;

    [SerializeField]
    Text textTimer;

    [SerializeField]
    Image curEXPImg;

    [SerializeField]
    Text pointTxt;

    public GAMESTATE gameState;

    //���� ����
    public int iGameLevel;

    public int iEndLevel;

    float fMonsterSpawnTime;

    private int playerCurEXP;
    private int playerNextLevelEXP;
    private int gamePoint;


    //test
    bool bTest;
    enum CARDTYPE
    {
        PLAYERSPEED,        //�÷��̾� �̵��ӵ�
        PLAYERHP,           //�÷��̾� �ִ�ü��
        PLAYERDEFENSE,      //�÷��̾� ����
        TURRETATTK,         //��ž ���ݷ�
        TURRETSPEED,        //��ž ���ݼӵ�
        TURRETRANGE,        //��ž ����
        TURRETNUM           //��ž ���� �߰�
    }

    public int iTurretAttk;
    public float fTurretSpeed;
    public float fTurretRange;

    void Awake()
    {
        gm = this;
        gameState = GAMESTATE.READY;
        iGameLevel = 1;

        //�� �ʱ�ȭ
        iTurretAttk     = 0;
        fTurretSpeed    = 0;
        fTurretRange    = 0;

        for (int idx = 1; idx < iEndLevel + 1; idx++)
        {
            GMData gmData = Resources.Load<GMData>("ScriptableData/GameManager/" + idx.ToString());

            gmDataList.Add(gmData);
        }

        //1���� ������ ����

        curLevelData = gmDataList[iGameLevel - 1];

        gamePoint = 0;

        pointTxt.text = string.Format("{0:D1}", gamePoint);

        //Time.timeScale = 0; //deltatime �� 0���� ���� �Ͻ�����ó�� ���̰� �����./
        //Time.timeScale = 5f;
        StartCoroutine(GameReady());
    }

    void Start()
    {
        PointSndPlayer.Stop();
        BtnSndPlayer.Stop();

        playerCurEXP = 0;
        playerNextLevelEXP = playerCtrl.playerData.iStartEXP;

        curEXPImg.fillAmount = (float)playerCurEXP / (float)playerNextLevelEXP;
    }

    void Update()
    {
        //�ð��������ϱ�
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    bTest = !bTest;
        //}

        //if (bTest) Time.timeScale = 1f;
        //else Time.timeScale = 10f;

        if (gameState != GAMESTATE.PAUSE)
        {
            fGameTime += Time.deltaTime;

            Timer();
            SpawnTimer();
            CheckLevelUp();
        }

        else if (gameState == GAMESTATE.PAUSE)
        {
            Time.timeScale = 0;
        }
    }

    private IEnumerator GameReady()
    {
        textTimer.transform.parent.gameObject.SetActive(true);
        
        yield return new WaitForSecondsRealtime(2.0f); //timeScale ��Ž

        gameState = GAMESTATE.PLAY;

        //���� ������ ���Ϳ� �ͷ��� �����Ѵ�
        LevelUpSpawn();
    }

    private void Timer()
    {
        fSec += Time.deltaTime;

        textTimer.text = string.Format("{0:D2}:{1:D2}", iMinute, (int)fSec);
        
        if ((int)fSec > 59)
        {
            fSec = 0;
            iMinute++;
            iGameLevel++;


            //�������������� ���߰� �����ؾߵ�
            if (iGameLevel > iEndLevel)
            {
                gameState = GAMESTATE.END;
                Time.timeScale = 0.3f;
                StartCoroutine(GameEndMessage());
                return;
            }
            
            curLevelData = gmDataList[iGameLevel - 1];

            LevelUpSpawn();
        }
    }

    IEnumerator GameEndMessage()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 0;
        //�ؽ�Ʈ ����
        GameEndTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.GAMECLEAR);
        GameEndTxt.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        OnPauseBtn();
    }

    private void SpawnTimer()
    {
        if (curLevelData.bBossTime) return;

        fMonsterSpawnTime += Time.deltaTime;

        if(fMonsterSpawnTime >= curLevelData.fCreateTime)
        {
            fMonsterSpawnTime = 0;
            SpawnMonster(1);
        }
    }

    //������ �� �Ѱ���
    private void LevelUpSpawn()
    {
        LevelUpSpawnMonster();
        StartCoroutine(LevelUPSpawnTurret());
    }

    //�������ϸ� �ѹ��� ��ȯ�ϴ� ����
    private void LevelUpSpawnMonster()
    {
        enemyFac.SpawnMonsters(curLevelData.iCreateCount, iGameLevel);
    }

    //�������ϸ� �ѹ��� ��ȯ�ϴ� �ͷ� //�ð� �����ð� �ο�
    IEnumerator LevelUPSpawnTurret()
    {
        float delayTime = Random.Range(3, 8);

        yield return new WaitForSeconds(delayTime);

        TurretFac.SpawnTurrets(1, 1);
    }

    //���� ����
    public void SpawnMonster(int i_iSpawnCnt)
    {
        enemyFac.SpawnMonsters(i_iSpawnCnt, iGameLevel);
    }

    //ĳ�� ����
    IEnumerator SpawnTurret(int i_iSpawnCnt)
    {
        float delayTime = Random.Range(3, 8);

        yield return new WaitForSeconds(delayTime);

        TurretFac.SpawnTurrets(i_iSpawnCnt, 1);
    }

    //ī�� ���� �Է�
    public void InputCardInfo(CardData cd)
    {
        //ī�� Ÿ�Կ� ���� �������� �־���ߵ�
        switch(cd.iCardType)
        {
            case (int)CARDTYPE.PLAYERSPEED:

                playerCtrl.fSpeed += cd.fPlusPoint;

                break;

            case (int)CARDTYPE.PLAYERHP:

                //�ִ�ü�� ����
                playerCtrl.iMaxHP += Mathf.FloorToInt(cd.fPlusPoint);

                //UI �ݿ����ֱ�
                playerCtrl.RefreshPlayerInfo();

                break;

            case (int)CARDTYPE.PLAYERDEFENSE:

                //���� ����
                playerCtrl.iDef += Mathf.FloorToInt(cd.fPlusPoint);

                break;


           //�ͷ��� ������ƮǮ�� �ν��Ͻ��� �����ϱ� ������ �� ��ũ��Ʈ���� ���ӿ�����Ʈ ������ ���������ؾߵ�
            case (int)CARDTYPE.TURRETATTK:

                //��ž ���ݷ� ����
                iTurretAttk += Mathf.FloorToInt(cd.fPlusPoint);

                break;

            case (int)CARDTYPE.TURRETSPEED:

                fTurretSpeed += cd.fPlusPoint;

                break;

            case (int)CARDTYPE.TURRETRANGE:

                fTurretRange += cd.fPlusPoint;

                break;

            case (int)CARDTYPE.TURRETNUM:

                //��ž �����ϴ� �Լ� ȣ��

                int MaxSize = Mathf.FloorToInt(cd.fPlusPoint);

                for (int idx = 0; idx < MaxSize; idx++)
                {
                    SpawnTurret(1);
                }

                break;
        }

        // ī�弱�� ��� ��ü ���ְ�
        cardManager.CardBGSetActive(false);

        // ������ �ٽý��۽�Ų��
        gameState = GAMESTATE.PLAY;

        // �����ð���� ������
        Time.timeScale = 1;
        //Time.timeScale = 5f;
    }

    public void ObtainEXP(int i_iEXP)
    {
        if (playerCtrl.isDead) return;

        //point ���� �ֱ�
        PointSndPlayer.Play();

        playerCurEXP += i_iEXP;
        ObtainGamePoint(i_iEXP);
    }

    void CheckLevelUp()
    {
        if (playerCtrl.isDead) return;

        if (playerCurEXP >= playerNextLevelEXP)
        {
            //������ ���� �÷���
            LvUpSndPlayer.gameObject.SetActive(true);
            LvUpSndPlayer.Play();

            playerCtrl.iLevel++;

            GameManager.gm.playerCtrl.RefreshPlayerInfo();

            playerCurEXP -= playerNextLevelEXP;

            float nextLevelEXP = playerNextLevelEXP * playerCtrl.playerData.fNextEXP;

            playerNextLevelEXP = Mathf.FloorToInt(nextLevelEXP);

            GameManager.gm.cardManager.OpenCard();
            gameState = GAMESTATE.PAUSE;
        }

        curEXPImg.fillAmount = (float)playerCurEXP / (float)playerNextLevelEXP;
    }

    void ObtainGamePoint(int i_iPoint)
    {
        gamePoint += i_iPoint;
        pointTxt.text = string.Format("{0:D1}", gamePoint);
    }

    public void OnPlayBtn()
    {
        GamePauseUI.SetActive(false);

        if(!cardManager.GetCardBGActive())
        {
            Time.timeScale = 1;
            gameState = GAMESTATE.PLAY;
        }

        OnExitBannerAd();
    }

    public void OnReStartBtn()
    {
        OnExitBannerAd();
        OnStartVedioAd(VedioAdmobo.BtnType.RESTART);
    }

    public void OnExitBtn()
    {
        OnExitBannerAd();
        OnStartVedioAd(VedioAdmobo.BtnType.EXIT);
    }

    public void BtnSndPlay()
    {
        BtnSndPlayer.Play();
    }

    public void OnPauseBtn()
    {
        GamePauseUI.SetActive(true);

        bool isDead = gm.playerCtrl.isDead;

        playBtn.SetActive(!isDead);
        Xbtn.SetActive(!isDead);

        gameState = GAMESTATE.PAUSE;

        OnStartBannerAd();
    }

    void OnExitBannerAd()
    {
        BannerAdmob.GetComponent<BannerAdmobo>().EndBanner();
    }

    void OnStartBannerAd()
    {
        BannerAdmob.GetComponent<BannerAdmobo>().StartBanner();
    }

    void OnStartVedioAd(VedioAdmobo.BtnType btnType)
    {
        BannerAdmob.GetComponent<VedioAdmobo>().RequestInterstitial(btnType);
    }

    public void InitRestartInfo()
    {
        Time.timeScale = 1;
        gameState = GAMESTATE.READY;
        StartCoroutine(GameReady());
        SceneManager.LoadScene("InGame");
    }

    public void InitExitInfo()
    {
        Time.timeScale = 1;
        gameState = GAMESTATE.END;
        SceneManager.LoadScene("Main");
    }
}
