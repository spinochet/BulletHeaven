using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float damage;
    [SerializeField] private float chargeTime;
    [SerializeField] private float speed;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < chargeTime)
        {
            Color c = sprite.color;
            c.a = (Mathf.Sin(timer * 3.0f) + 1.0f) * 0.25f;
            sprite.color = c;
        }
        else
        {
            sprite.gameObject.SetActive(false);
            Vector3 euler = transform.eulerAngles;
            euler.y -= Time.deltaTime * speed;
            transform.eulerAngles = euler;
        }

        if (timer > chargeTime + speed / 90.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        NewPlayerController player = col.transform.GetComponent<NewPlayerController>();

        if (player)
        {
            player.TakeDamage(damage);
        }
    }
}
