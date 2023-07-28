using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudControl : MonoBehaviour
{
    float fMoveSeed;

    void Start()
    {
        fMoveSeed = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - fMoveSeed * Time.deltaTime, transform.position.y, transform.position.z);

        if (transform.position.x <= -40.0f)
        {
            transform.position = new Vector3(40.0f, transform.position.y, transform.position.z);
        }
    }
}
