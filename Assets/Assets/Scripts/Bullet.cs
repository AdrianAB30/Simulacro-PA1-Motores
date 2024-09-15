using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 5f;
    [SerializeField] public bool isPlayerBullet;
    [SerializeField] public bool isEnemyBullet;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    public void Initialize(Vector2 direction, float speed, bool isPlayer, bool isEnemy)
    {
        rb.velocity = direction * speed;
        isPlayerBullet = isPlayer;
        isEnemyBullet = isEnemy;
    }
}
