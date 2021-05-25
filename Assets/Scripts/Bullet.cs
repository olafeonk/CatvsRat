using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    
    public bool isEnemyBullet;

    private Vector2 lastPosition;

    private Vector2 currentPosition;

    private Vector2 playerPosition;
    
    private float coolDown = 0.1f;

    private bool coolDownDamaged;

    private void Start()
    {
        StartCoroutine( DeathDelay());
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    private void Update()
    {
        if (isEnemyBullet)
        {
            currentPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, 5f * Time.deltaTime);
            if (currentPosition == lastPosition)
            {
                Destroy(gameObject);
            }

            lastPosition = currentPosition;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPosition = player.position;
    }
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && !isEnemyBullet)
        {
            collider.gameObject.GetComponent<EnemyControler>().Damage(GameController.DamagePlayer);
            Destroy(gameObject);
        }

        if (collider.CompareTag("Player") && isEnemyBullet && !coolDownDamaged)
        {
            GameController.DamagedPlayer(1);
            Destroy(gameObject);
            StartCoroutine(CoolDown());
        }
    }
    
    private IEnumerator CoolDown()
    {
        coolDownDamaged = true;
        yield return new WaitForSeconds(coolDown);
        coolDownDamaged = false;
    }
}
