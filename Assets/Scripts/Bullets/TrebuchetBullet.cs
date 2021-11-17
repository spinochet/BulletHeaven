using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetBullet : Bullet
{
    [Header ("Trebuchet")]
    [SerializeField] private GameObject shadow;
    [SerializeField] private float radius = 5.0f;
    [SerializeField] private float expandSpeed = 2.5f;
    [SerializeField] private List<GameObject> targets;

    private float timer = 0.0f;

    void Awake()
    {
        // Play launch sound
        // SoundManager.Instance.Play(name);
    }

    // Update is called once per frame
    void Update()
    {
        // Combat
        timer += Time.deltaTime;
        if (expandSpeed <= timer)
        {
            foreach (GameObject target in targets)
            {
                if (target)
                {
                    Pawn pawn = target.GetComponent<Pawn>();
                    if (pawn) {
                        pawn.TakeDamage(levels[currentLevel].damage);
                        
                    }
                        
                }
            }
            SoundManager.Instance.Play("Boulder Crash");
            Destroy(gameObject);
            
        }
        else
        {
            float size = (timer / expandSpeed) * radius * 2.0f;
            shadow.transform.localScale = new Vector3(size, size, size);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        targets.Add(col.gameObject);
    }

    public void OnTriggerExit(Collider col)
    {
        targets.Remove(col.gameObject);
    }
}
