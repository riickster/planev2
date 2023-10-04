using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour{
    
    [Header("Speed")]
    public float speed = 2f;
    public float speedY = 2f;
    [Header("Limites")]
    public float limiteX = 4f;

    [Header("Disparo")] 
    public GameObject prefabDisparo;
    public float disparoSpeed = 2f;
    public float timeDisparoDestroy = 2f;
    public float timeBetweenShots = 0.5f;

    public Transform weapon1;
    public Transform weapon2;

    private bool isShooting = false;
    private Rigidbody2D rb;
    private float timeSinceLastShot = 0f;

    // Player health variables
    public int maxHealth = 3;
    public int currentHealth;

    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update(){
        MovePlayer();
        HandleFireInput();
    }

    public void MovePlayer(){
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, speedY);
        if (transform.position.x > limiteX){
            transform.position = new Vector2(limiteX, transform.position.y);
        } else if (transform.position.x < -limiteX){
            transform.position = new Vector2(-limiteX, transform.position.y);
        }
    }

    public void HandleFireInput(){
        if (Input.GetButtonDown("Fire1") && !isShooting){
            isShooting = true;
            StartCoroutine(ShootInterval());
        } else if (Input.GetButtonUp("Fire1")){
            isShooting = false;
        }
    }

    IEnumerator ShootInterval(){
        while (isShooting){
            if (Time.time - timeSinceLastShot >= timeBetweenShots){
                Shoot();
                timeSinceLastShot = Time.time;
            }
            yield return null;
        }
    }

    void Shoot(){
        GameObject disparoInstance = Instantiate(prefabDisparo);
        disparoInstance.transform.SetParent(transform.parent);

        disparoInstance.transform.position = weapon1.position;
        disparoInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
        Destroy(disparoInstance, timeDisparoDestroy);

        GameObject disparoInstance2 = Instantiate(prefabDisparo);
        disparoInstance2.transform.SetParent(transform.parent);
        disparoInstance2.transform.position = weapon2.position;

        disparoInstance2.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
        Destroy(disparoInstance2, timeDisparoDestroy);
    }

    public void TakeHit() {
        currentHealth--;

        if (currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D otherCollider){
        if (otherCollider.gameObject.tag == "disparoEnemigo"){
            TakeHit();
            Destroy(otherCollider.gameObject);
        } else if (otherCollider.gameObject.tag == "enemigos"){
            Die();
            Destroy(otherCollider.gameObject);
        }
    }

}
