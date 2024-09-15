using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolMovementController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;  
    [SerializeField] private float normalSpeed = 5f;   
    [SerializeField] private float chaseSpeed = 10f;    
    [SerializeField] private float detectionRange = 5f; 

    [Header("Components")]
    private Rigidbody2D rb2D;        
    private SpriteRenderer spriteRenderer; 
    [SerializeField] private LayerMask playerLayer;     

    private int currentPatrolIndex = 0;                
    private bool isChasing = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        MoveToNextPatrolPoint();
    }

    private void Update()
    {
        if (IsPlayerDetected())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        if(HasReachedPatrolPoint())
        {
            MoveToNextPatrolPoint();
        }
    }

    private void Patrol()
    {
        MoveTowardsTarget(patrolPoints[currentPatrolIndex].position, normalSpeed);
    }
    private void MoveTowardsTarget(Vector2 targetPosition, float speed)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb2D.velocity = direction * speed;
        FlipSprite(direction.x);
    }

    private void ChasePlayer()
    {
        isChasing = true;
        MoveTowardsTarget(patrolPoints[currentPatrolIndex].position, chaseSpeed);
    }

    private void MoveToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void FlipSprite(float directionX)
    {
        spriteRenderer.flipX = directionX < 0;
    }
    private bool IsPlayerDetected()
    {
        Vector2 direction = rb2D.velocity.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
    private bool HasReachedPatrolPoint()
    {
        return Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f;
    }
}
