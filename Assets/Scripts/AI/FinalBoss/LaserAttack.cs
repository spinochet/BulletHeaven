using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;

    private Rigidbody2D rb;
    private Vector3 dir;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position += dir * speed * Time.deltaTime;

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
