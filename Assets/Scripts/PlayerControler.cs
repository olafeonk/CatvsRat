using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler: MonoBehaviour
{
    public float speed = 20;
    
    public Rigidbody2D rb;
    
    public Animator animator;
    
    private Vector2 movement;

    public GameObject bulletPrefab;

    public float bulletSpeed;
    
    private float lastFire;

    public float fireDelay;
    
    public bool IsPlayerWasDamaged;

    public float coolDownDamaged;

    private void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        var shootHorizontal = Input.GetAxisRaw("ShootHorizontal");
        var shootVertical = Input.GetAxisRaw("ShootVertical");
        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }
        
        animator.SetFloat("Horizontal", movement.normalized.x);
        animator.SetFloat("Vertical", movement.normalized.y);
        animator.SetFloat("Speed", movement.normalized.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * (speed * Time.fixedDeltaTime));
    }

    private void Shoot(float x, float y)
    {
        var transform1 = transform;
        var bullet = Instantiate(bulletPrefab, transform1.position, transform1.rotation);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed, 
            0);
    }

    
    
    public void Damaged()
    {
        StartCoroutine(CoolDown());
    }
    
    private IEnumerator CoolDown()
    {
        IsPlayerWasDamaged	 = true;
        yield return new WaitForSeconds(coolDownDamaged);
        IsPlayerWasDamaged	 = false;
    }
    
}    
