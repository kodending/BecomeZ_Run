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
        //�ʱ�ȭ�ϴ°�
        offset = transform.position - target.position;
        fRotateSpeed = Random.Range(200.0f, 360.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //�������
        transform.position = target.position + offset;
        transform.RotateAround(target.position,
                               Vector3.up,
                               fOrbitSpeed * Time.deltaTime);
        offset = transform.position - target.position;

        //�������
        transform.Rotate(Vector3.up * Time.deltaTime * fRotateSpeed);
    }
}
