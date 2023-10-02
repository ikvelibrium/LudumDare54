using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMBullet : MonoBehaviour
{
   [SerializeField] private Transform target;        // ���� (�����)
   [SerializeField] private float speed = 5f;        // �������� ����
   [SerializeField] private float rotateSpeed = 200f; // �������� �������� ����
   [SerializeField] private float deviationAmount = 30f; // ������������ ���������� �� ����������� � ����

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // ��������� ��������� ���������� �� ���������� ����������� ����
        float deviation = Random.Range(-deviationAmount, deviationAmount);
        transform.Rotate(0f, 0f, deviation);
    }

    void Update()
    {
        if (target != null)
        {
            // ���������� ����������� � ����
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            // ��������� ���� ����� ������� ������������ ���� � ������������ � ����
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // ������������ ���� � ����������� ����
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
        else
        {
            // ���� ���� �����������, ���������� ����
            Destroy(gameObject);
        }
    }

    // ����� ��� ��������� ���� (������)
    
}
