using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float paddingX = 1f;
    [SerializeField] float paddingY = 1f;
    [SerializeField] int health = 3;
    [Header("Die")]
    [SerializeField] AudioClip playerExplosionSound;
    [SerializeField] [Range(0, 1)] float volumeExplosion = 0.7f;
    [SerializeField] float dutationOfExplosion = 1f;
    [SerializeField] GameObject explosionEffect;

    [Header("Projectile")]
    [SerializeField] GameObject LaserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] [Range(0,1)] float projectileFiringPeriod = 1f;
    [SerializeField] AudioClip shotSound;
    [SerializeField] [Range(0, 1)] float volumeShot = 0.7f;

    //Variable

    Coroutine FiringCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;


    // Start is called before the first frame update
    void Start()
    {
        playerSpaceLimit();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           FiringCoroutine = StartCoroutine(FireContinuously());

        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(FiringCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject Laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity) as GameObject;
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position, volumeShot);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        processHit(damageDealer);
    }    
    public int GetHealth()
    {
        return health;
    }
    private void processHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, dutationOfExplosion);
        AudioSource.PlayClipAtPoint(playerExplosionSound, Camera.main.transform.position, volumeExplosion);
        Level.FindObjectOfType<Level>().LoadGameOver();
    }


    private void Move()
    {
       //Command keyboard

       //var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
       //var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;


        // Mouse Command

        var deltaX = Input.GetAxis("Mouse X") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Mouse Y") * Time.deltaTime * moveSpeed; 

        var newXPos = Mathf.Clamp((transform.position.x + deltaX), xMin, xMax);
        var newYPos = Mathf.Clamp((transform.position.y + deltaY), yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }
    private void playerSpaceLimit()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingX;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingY;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingY;
    }
}
