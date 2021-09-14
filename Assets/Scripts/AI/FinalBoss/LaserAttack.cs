using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    private BoxCollider2D collider;
    [SerializeField] private float speed;
    [SerializeField] private float duration;

    private Color color;
    private Vector3 dir;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        color = sprite.color;
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

    void Awake()
    {
        color = sprite.color;
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (speed > timer)
        {
            color.a = timer / speed;
            sprite.color = color;
        }
        else
        {
            sprite.color = Color.white;
            collider.enabled = true;
        }

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 _dir)
    {
        dir = _dir;
    }

    // when the GameObjects collider arrange for this GameObject to travel to the left of the screen
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController player = col.transform.GetComponent<PlayerController>();

        if (player)
        {
            player.TakeDamage(20);
        }
    }
}
