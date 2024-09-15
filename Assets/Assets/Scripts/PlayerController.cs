using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 direction;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private HealthBarController healthBarController;
    [SerializeField] private int lifeRestPlayer;
    [SerializeField] UIManager uiManager;


    private void OnEnable()
    {
        healthBarController.OnHealthDepletedEvent += HandleHealthDepleted;
    }

    private void OnDisable()
    {
        healthBarController.OnHealthDepletedEvent -= HandleHealthDepleted;
    }

    private void Update() {

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        Vector2 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckFlip(mouseInput.x);
    
        Debug.DrawRay(transform.position, mouseInput.normalized * rayDistance, Color.red);

        if(Input.GetMouseButtonDown(0)){
            Shoot(mouseInput);
            Debug.Log("Right Click");
        }else if(Input.GetMouseButtonDown(1)){
            Shoot(mouseInput);
            Debug.Log("Left Click");
        }
    }
    private void FixedUpdate()
    {
        myRBD2.velocity = direction * velocityModifier;
        myRBD2.position = new Vector2(Mathf.Clamp(myRBD2.position.x, -14f, 14f), Mathf.Clamp(myRBD2.position.y, -12f, 12f));
    }
    public void OnMovementX(InputAction.CallbackContext context)
    {
        direction.x = context.ReadValue<float>();
    }
    public void OnMovementY(InputAction.CallbackContext context)
    {
        direction.y = context.ReadValue<float>();
    }

    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.isEnemyBullet)
            {
                healthBarController.ApplyDamage(lifeRestPlayer);
                Destroy(collision.gameObject);
            }
        }
    }
    private void HandleHealthDepleted()
    {
        ChangeScene("Game Over");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void Shoot(Vector2 target)
    {
        Vector2 shootingDirection = (target - (Vector2)transform.position);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(shootingDirection, bulletSpeed, true, false);
    }
}
