using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey2_Controller : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 3f;               
    [SerializeField] private Transform projectileSpawnPoint;      
    [SerializeField] private GameObject bulletPrefab;         
    [SerializeField] private float projectileCooldown = 1f;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private HealthBarController healthBarController;
    [SerializeField] private int lifeRest;
    [SerializeField] UIManager uiManager;


    [Header("Area Settings")]
    [SerializeField] private CircleCollider2D detectionZone;     
    [SerializeField] private LayerMask playerLayer;              

    private Vector2 initialPosition;                              
    private bool isPlayerInZone = false;                         
    private Transform playerTransform;                            
    private bool isShooting = false;
    

    private void Start()
    {
        detectionZone = GetComponent<CircleCollider2D>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isPlayerInZone && moveSpeed != 0)
        {
            MoveTowardsPlayer();
        }
        else
        {
            ReturnToInitialPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInZone = true;
            playerTransform = collision.transform;
            StartShooting();
        }
        else if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.isPlayerBullet) 
            {
                healthBarController.ApplyDamage(lifeRest);
                Destroy(collision.gameObject); 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInZone = false;
            StopShooting();
        }      
    }
    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
    }
    private void ReturnToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
    }

    private void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            InvokeRepeating("ShootProjectile", 0f, projectileCooldown);
        }
    }

    private void StopShooting()
    {
        if (isShooting)
        {
            isShooting = false;
            CancelInvoke("ShootProjectile");
        }
    }
    private void ShootProjectile()
    {
        if (bulletPrefab != null && projectileSpawnPoint != null)
        {
            Vector2 target = playerTransform.position;
            
            Vector2 direction = (target - (Vector2)transform.position);

            GameObject bullet = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(direction, projectileSpeed, false, true);
        }
    }
    public void OnHealthDepleted()
    {
        StopShooting(); 
        isShooting = false; 
        moveSpeed = 0f;
        if (detectionZone != null)
        {
            detectionZone.enabled = false;
        }
    }
}