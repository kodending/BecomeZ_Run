using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{

    public Transform target;
    public float fOrbitSpeed;
    [SerializeField]
    private float fRotateSpeed;
    Vector3 offset;

    void Start()
    {
        //초기화하는곳
        offset = transform.position - target.position;
        fRotateSpeed = Random.Range(200.0f, 360.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //공전계산
        transform.position = target.position + offset;
        transform.RotateAround(target.position,
                               Vector3.up,
                               fOrbitSpeed * Time.deltaTime);
        offset = transform.position - target.position;

        //자전계산
        transform.Rotate(Vector3.up * Time.deltaTime * fRotateSpeed);
    }
}
