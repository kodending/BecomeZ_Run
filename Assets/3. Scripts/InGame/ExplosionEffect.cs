using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public GameObject ExplosionEff;

    public IEnumerator StartEffect(Vector3 i_vStartPos)
    {
        transform.position = new Vector3(i_vStartPos.x, 0, i_vStartPos.z);

        ExplosionEff.SetActive(true);

        yield return new WaitForSeconds(3f);

        ExplosionEff.SetActive(false);
        gameObject.SetActive(false);
    }

    public IEnumerator StartSound()
    {
        ExplosionEff.GetComponentInChildren<AudioSource>().Play();

        yield return new WaitForSeconds(3f);

        ExplosionEff.GetComponentInChildren<AudioSource>().Stop();
    }
}
