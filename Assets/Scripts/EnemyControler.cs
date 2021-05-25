using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Idle,
    Follow,
    Attacking
}

public enum EnemyType
{
    Melee,
    Ranged,
    Boom
}

public class EnemyControler : MonoBehaviour
{
    private GameObject player;

    public EnemyState currentState = EnemyState.Idle;

    public EnemyType enemyType;

    public float range;

    public float speed;

    public float healthPoint;

    public float attackingRange;

    public int damage = 1;

    private bool chooseDirection;

    public bool notInRoom;

    private bool dead = false;

    public float coolDown;

    private bool coolDownAttack;

    private Vector3 randomDirection;

    public GameObject bulletPrefab;

    public bool isBoss;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Attacking:
                Attacking();
                break;
        }

        if(!notInRoom)
        {
            if(IsPlayerInRange(range))
            {
                currentState = EnemyState.Follow;
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= attackingRange)
            {
                currentState = EnemyState.Attacking;
            }
        }
        else
        {
            currentState = EnemyState.Idle;
        }

    }
    

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }
    
    private void Follow()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.transform.position,
            speed * Time.deltaTime);
    }

    void Attacking()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagedPlayer(damage);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.GetComponent<Bullet>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<Bullet>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break;
                case EnemyType.Boom:
                    GameController.DamagedPlayer(damage);
                    Destroy(gameObject);
                    StartCoroutine(CoolDown());
                    break;
            }
        }
        
    }

    public void Damage(float damage)
    {
        healthPoint -= damage;
        if (healthPoint <= 0)
        {
            Death();
        }
    }
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
    public void Death()
    {
        switch (enemyType)
        {
            case EnemyType.Melee:
                GameController.PointsChange(25);
                break;
            case EnemyType.Ranged:
                GameController.PointsChange(50);
                break;
        }

        if (isBoss)
        {
            GameController.PointsChange(100);
            Camera.main.GetComponent<UIManagerController>().Win();
        }
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }
}
