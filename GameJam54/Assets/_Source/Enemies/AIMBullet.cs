using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMBullet : MonoBehaviour
{
   [SerializeField] private Transform target;        // ÷ель (игрок)
   [SerializeField] private float speed = 5f;        // —корость пули
   [SerializeField] private float rotateSpeed = 200f; // —корость поворота пули
   [SerializeField] private float deviationAmount = 30f; // ћаксимальное отклонение от направлени€ к цели

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // ƒобавл€ем случайное отклонение от начального направлени€ пули
        float deviation = Random.Range(-deviationAmount, deviationAmount);
        transform.Rotate(0f, 0f, deviation);
    }

    void Update()
    {
        if (target != null)
        {
            // ќпредел€ем направление к цели
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            // ¬ычисл€ем угол между текущим направлением пули и направлением к цели
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // ѕоворачиваем пулю в направлении цели
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
        else
        {
            // ≈сли цель отсутствует, уничтожаем пулю
            Destroy(gameObject);
        }
    }

    // ћетод дл€ установки цели (игрока)
    
}
