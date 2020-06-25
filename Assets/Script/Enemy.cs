using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health;
    [SerializeField] int scoreValue = 150 ;

    [Header("shoot")]
    [SerializeField] GameObject enemyLaserPrefab;
     float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 1f;


    [Header("die")]
    [SerializeField] GameObject explosionEffect;
    [SerializeField] AudioClip enemyExplosionSound;
    [SerializeField] [Range(0, 1)] float volumeExplosion = 0.7f;
    [SerializeField] float dutationOfExplosion = 1f;


    Coroutine FiringCoroutine;
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void Fire()
    {
            GameObject Laser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        processHit(damageDealer);
    }

    private void processHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            EnemyDie();
            //add score
            //Die effect
        }
    }
    private void EnemyDie()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion,dutationOfExplosion);
        AudioSource.PlayClipAtPoint(enemyExplosionSound, Camera.main.transform.position,volumeExplosion);
    }
}
