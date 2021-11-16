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
    private int side;
    private bool flag;

    void Awake()
    {
        float f = Random.Range(0.0f, 1.0f);
        side = f < 0.5f ? -1 : 1;

        transform.position = new Vector3(11 * side, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < chargeTime && !flag)
        {
            Color c = sprite.color;
            c.a = (Mathf.Sin(timer * 3.0f) + 1.0f) * 0.25f;
            sprite.color = c;
        }
        else if (!flag)
        {
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        if (!flag)
        {
            flag = true;
            float angle = 0.0f;
            timer = 0.0f;

            while (angle < 200.0f)
            {
                angle += speed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0.0f, -angle * side, 0.0f);
                yield return null;
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Pawn player = col.transform.GetComponent<Pawn>();

        if (player)
        {
            player.TakeDamage(damage);
        }
    }
}
