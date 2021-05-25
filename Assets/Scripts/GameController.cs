using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health = 6;

    private static int maxHealth = 6;

    private static float moveSpeed = 5f;

    private static float fireRate = 1f;

    private static float damagePlayer = 10f;

    private static float bulletSize = 1f;
    

    public Text pointsText;

    private static float points;

    public float Points
    {
        get => points;
        set => points = value;
    }

    public static float DamagePlayer
    {
        get => damagePlayer;
        set => damagePlayer = value;
    }

    public static float Health
    {
        get => health;
        set => health = value;
    }

    public static int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public static float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public static float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    public static float BulletSize
    {
        get => bulletSize;
        set => bulletSize = value;
    }
    
    private void Start()
    {
        health = 6;
        maxHealth = 6;
        moveSpeed = 5f;
        fireRate = 1f;
        damagePlayer = 10f;
        bulletSize = 1f;
        points = 500;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    void Update()
    {
        points -= Time.deltaTime;
        pointsText.text = "Очки: " + Mathf.Round(points);
    }

    public static void DamagedPlayer(int damage)
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().IsPlayerWasDamaged)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().Damaged();
            health -= damage;
            if (Health <= 0)
            {
                KillPlayer();
            }
            
        }
    }

    public static void PointsChange(int point)
    {
        points += point;
    }
    
    public static void HealthPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }
    
    public static void DamageChange(float damage)
    {
        damagePlayer += damage;
    }
    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
    }
    
    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }
    
    private static void KillPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        Camera.main.GetComponent<UIManagerController>().Lose();
    }
}
