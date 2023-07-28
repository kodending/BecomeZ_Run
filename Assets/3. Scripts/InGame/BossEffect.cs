using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffect : MonoBehaviour
{
    public IEnumerator StartDeadEff(Vector3 i_vStartPos)
    {
        transform.position = i_vStartPos;

        gameObject.SetActive(true);

        yield return new WaitForSeconds(7f);

        gameObject.SetActive(false);
    }
}
