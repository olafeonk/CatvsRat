using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string name;

    public string description;

    public Sprite itemImage;
}

public class CollectionConroller : MonoBehaviour
{
    public Item item;

    public float healthChange;

    public float moveSpeedChange;

    public float attackSpeedChange;

    public float bulletSizeChange;

    public float damageChange;
    
    private float coolDown = 0.1f;

    private bool coolDownCollection;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !coolDownCollection)
        {
            GameController.HealthPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.DamageChange	(damageChange);
            StartCoroutine(CoolDown());
            Destroy(gameObject);
        }
    }
    
    private IEnumerator CoolDown()
    {
        coolDownCollection = true;
        yield return new WaitForSeconds(coolDown);
        coolDownCollection = false;
    }
}
