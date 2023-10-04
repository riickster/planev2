using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour{
    
    [Header("Speed")]
    public float speed = 2f;

    [Header("Disparo")]
    public GameObject prefabDisparo;
    public float disparoSpeed = 2f;
    public float shootingInterval = 6f;
    public float timeDisparoDestroy = 2f;
    public float shootingCooldownDuration = 3f;

    [Header("Hits to Destroy")]
    public int maxHits = 3;
    private int currentHits = 0;

    public float lifetime = 15f;

    private float shootingTimer;
    private bool isVisible = true;

    private float leftBoundary;
    private float rightBoundary;
    private bool movingRight;

    private bool canShoot = true;
    private float shootingCooldownTimer = 0f;

    public Transform weapon1;
    public Transform weapon2;

    void Start(){
        shootingTimer = Random.Range(0f, shootingInterval);
        leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        movingRight = transform.position.x >= 0f;
        StartCoroutine(DestroyAfterLifetime());
    }

    void Update(){
        if (isVisible){
            if (canShoot){
                StartFire();
            }

            MoveHorizontally();
            HandleShootingCooldown();
        }
    }

    public void StartFire(){
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0f){
            shootingTimer = shootingInterval;

            if (gameObject.activeSelf){
                GameObject disparoInstance = Instantiate(prefabDisparo);
                disparoInstance.transform.position = weapon1.position;

                disparoInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
                Destroy(disparoInstance, timeDisparoDestroy);

                GameObject disparoInstance2 = Instantiate(prefabDisparo);
                disparoInstance2.transform.position = weapon2.position;

                disparoInstance2.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
                Destroy(disparoInstance2, timeDisparoDestroy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider){
        if (otherCollider.CompareTag("disparoPlayer") && canShoot){
            TakeHit();
            Destroy(otherCollider.gameObject);
            ApplyShootingCooldown();
        } else {
            TakeHit();
            Destroy(otherCollider.gameObject);
        }
    }

    public void TakeHit(){
        currentHits++;

        if (currentHits >= maxHits){
            Die();
        }
    }

    private void Die(){
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterLifetime(){
        yield return new WaitForSeconds(lifetime);

        if (gameObject.activeSelf){
            Destroy(gameObject);
        }
    }
    
    private void MoveHorizontally(){
        float horizontalMovement = movingRight ? speed * Time.deltaTime : -speed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(horizontalMovement, 0, 0);

        if (newPosition.x <= leftBoundary && movingRight){
            movingRight = false;
        }

        if (newPosition.x >= rightBoundary && !movingRight){
            movingRight = true;
        }

        transform.position = newPosition;
    }

    private void HandleShootingCooldown(){
        if (!canShoot){
            shootingCooldownTimer -= Time.deltaTime;

            if (shootingCooldownTimer <= 0f){
                canShoot = true;
            }
        }
    }

    private void ApplyShootingCooldown(){
        canShoot = false;
        shootingCooldownTimer = shootingCooldownDuration;
    }
}
