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
        READY,      //준비
        PLAY,       //게임중
        PAUSE,      //잠시멈춤
        END,        //종료
        CLEAR,      //클리어
        DEAD,       //죽음
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

    //게임 레벨
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
        PLAYERSPEED,        //플레이어 이동속도
        PLAYERHP,           //플레이어 최대체력
        PLAYERDEFENSE,      //플레이어 방어력
        TURRETATTK,         //포탑 공격력
        TURRETSPEED,        //포탑 공격속도
        TURRETRANGE,        //포탑 범위
        TURRETNUM           //포탑 갯수 추가
    }

    public int iTurretAttk;
    public float fTurretSpeed;
    public float fTurretRange;

    void Awake()
    {
        gm = this;
        gameState = GAMESTATE.READY;
        iGameLevel = 1;

        //값 초기화
        iTurretAttk     = 0;
        fTurretSpeed    = 0;
        fTurretRange    = 0;

        for (int idx = 1; idx < iEndLevel + 1; idx++)
        {
            GMData gmData = Resources.Load<GMData>("ScriptableData/GameManager/" + idx.ToString());

            gmDataList.Add(gmData);
        }

        //1레벨 데이터 적용

        curLevelData = gmDataList[iGameLevel - 1];

        gamePoint = 0;

        pointTxt.text = string.Format("{0:D1}", gamePoint);

        //Time.timeScale = 0; //deltatime 을 0으로 만들어서 일시정지처럼 보이게 만든다./
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
        //시간빠르게하기
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
        
        yield return new WaitForSecondsRealtime(2.0f); //timeScale 안탐

        gameState = GAMESTATE.PLAY;

        //최초 레벨의 몬스터와 터렛을 생성한다
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


            //다음레벨에서는 멈추고 리턴해야됨
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
        //텍스트 수정
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

    //스폰할 곳 한곳에
    private void LevelUpSpawn()
    {
        LevelUpSpawnMonster();
        StartCoroutine(LevelUPSpawnTurret());
    }

    //레벨업하면 한번에 소환하는 몬스터
    private void LevelUpSpawnMonster()
    {
        enemyFac.SpawnMonsters(curLevelData.iCreateCount, iGameLevel);
    }

    //레벨업하면 한번에 소환하는 터렛 //시간 램덤시간 부여
    IEnumerator LevelUPSpawnTurret()
    {
        float delayTime = Random.Range(3, 8);

        yield return new WaitForSeconds(delayTime);

        TurretFac.SpawnTurrets(1, 1);
    }

    //몬스터 스폰
    public void SpawnMonster(int i_iSpawnCnt)
    {
        enemyFac.SpawnMonsters(i_iSpawnCnt, iGameLevel);
    }

    //캐논 스폰
    IEnumerator SpawnTurret(int i_iSpawnCnt)
    {
        float delayTime = Random.Range(3, 8);

        yield return new WaitForSeconds(delayTime);

        TurretFac.SpawnTurrets(i_iSpawnCnt, 1);
    }

    //카드 정보 입력
    public void InputCardInfo(CardData cd)
    {
        //카드 타입에 따라 정보들을 넣어줘야됨
        switch(cd.iCardType)
        {
            case (int)CARDTYPE.PLAYERSPEED:

                playerCtrl.fSpeed += cd.fPlusPoint;

                break;

            case (int)CARDTYPE.PLAYERHP:

                //최대체력 증가
                playerCtrl.iMaxHP += Mathf.FloorToInt(cd.fPlusPoint);

                //UI 반영해주기
                playerCtrl.RefreshPlayerInfo();

                break;

            case (int)CARDTYPE.PLAYERDEFENSE:

                //방어력 증가
                playerCtrl.iDef += Mathf.FloorToInt(cd.fPlusPoint);

                break;


           //터렛은 오브젝트풀로 인스턴스를 생성하기 때문에 각 스크립트에서 게임오브젝트 정보를 가져오게해야됨
            case (int)CARDTYPE.TURRETATTK:

                //포탑 공격력 증가
                iTurretAttk += Mathf.FloorToInt(cd.fPlusPoint);

                break;

            case (int)CARDTYPE.TURRETSPEED:

                fTurretSpeed += cd.fPlusPoint;

                break;

            case (int)CARDTYPE.TURRETRANGE:

                fTurretRange += cd.fPlusPoint;

                break;

            case (int)CARDTYPE.TURRETNUM:

                //포탑 생성하는 함수 호출

                int MaxSize = Mathf.FloorToInt(cd.fPlusPoint);

                for (int idx = 0; idx < MaxSize; idx++)
                {
                    SpawnTurret(1);
                }

                break;
        }

        // 카드선택 배경 전체 꺼주고
        cardManager.CardBGSetActive(false);

        // 게임을 다시시작시킨다
        gameState = GAMESTATE.PLAY;

        // 원래시간대로 돌리기
        Time.timeScale = 1;
        //Time.timeScale = 5f;
    }

    public void ObtainEXP(int i_iEXP)
    {
        if (playerCtrl.isDead) return;

        //point 사운드 넣기
        PointSndPlayer.Play();

        playerCurEXP += i_iEXP;
        ObtainGamePoint(i_iEXP);
    }

    void CheckLevelUp()
    {
        if (playerCtrl.isDead) return;

        if (playerCurEXP >= playerNextLevelEXP)
        {
            //레벨업 사운드 플레이
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
