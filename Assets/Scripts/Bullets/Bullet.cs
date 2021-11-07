﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [System.Serializable]
    public struct Level
    {
        public bool destroyOnCollision; 
        public float speed;
        public float damage;

        public float fireRate;
        public int numBullets;
        public float bulletSpacing;
    }

    [SerializeField] protected string name = null;
    [SerializeField] protected List<Level> levels = null;
    protected PlayerController owner = null;
    protected int currentLevel = 0;

    void Awake()
    {
        SoundManager.Instance.Play(name);
    }

    // Update is called once per frame
    void Update()
    {
        if (!owner)
            transform.position += transform.forward * levels[currentLevel].speed * Time.deltaTime;
        else
            transform.position += transform.forward * levels[owner.CurrentLevel].speed * Time.unscaledDeltaTime;
            
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    // Return bullet's fire rate
    public float GetFireRate(int currentLevel = 0)
    {
        return levels[currentLevel].fireRate;
    }

    // Return number of bullets
    public int GetNumBullets(int currentLevel = 0)
    {
        return levels[currentLevel].numBullets;
    }

    // return bullet spacing
    public float GetBulletSpacing(int currentLevel = 0)
    {
        return levels[currentLevel].bulletSpacing;
    }

    // Set the ownwe of the bullet
    public void SetOwner(PlayerController playerController, int bulletLevel = 0)
    {
        owner = playerController;
        currentLevel = bulletLevel;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.GetComponent<Pawn>() || col.transform.GetComponent<Bullet>())
        {
            int level = 0;
            if (owner)
                level = owner.CurrentLevel;

            float hp = 1;
            if (col.transform.GetComponent<Pawn>())
                hp = col.transform.GetComponent<Pawn>().TakeDamage(levels[level].damage);

            if (owner != null && hp <= 0.0f)
                owner.AddPoints(100);

            if (levels[level].destroyOnCollision)
                Destroy(gameObject);
        }
    }
}
