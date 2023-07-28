using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFSM : MonoBehaviour
{
    public ItemData itemData { get; set; }

    //아이템을 캐릭터에게 움직이게 해야됨

    bool isDragPlayer;

    [SerializeField]
    private float fRotateSpeed;

    float fMoveSpeed;

    float fTimer;

    [SerializeField]
    private float fDisableTime;

    void Update()
    {
        if (isDragPlayer) return;

        fTimer += Time.deltaTime;

        if(fTimer >= fDisableTime)
        {
            OnDisable();
        }
    }

    void FixedUpdate()
    {
        if (!isDragPlayer)
            transform.Rotate(Vector3.up * Time.deltaTime * fRotateSpeed);

        else
        {
            //플레이어 위치로 이동시키기
            Vector3 targetPos = GameManager.gm.playerCtrl.gameObject.transform.position;

            Vector3 velo = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velo, fMoveSpeed);

            fMoveSpeed -= 0.01f;
        }
    }

    public void InitPosition(Vector3 i_vEnemyPos)
    {
        this.gameObject.transform.position = i_vEnemyPos;
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        isDragPlayer = false;
        fTimer = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.gm.playerCtrl.isDead) return;

        if (other.tag == "DragRange")
        {
            isDragPlayer = true;
            fMoveSpeed = 0.1f;
        }

        if (other.tag == "Player")
        {
            GameManager.gm.playerCtrl.AddHPPotion(itemData.iHealAmount);

            OnDisable();
        }
    }
}
