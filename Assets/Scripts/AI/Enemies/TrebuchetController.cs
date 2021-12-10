using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrebuchetController : EnemyController
{
    [SerializeField] private Animator anim;
    [SerializeField] private Slider hp;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        contactTimer = contactCooldown;

        if (!anim)
            anim = GetComponent<Animator>();
    }   

    // Update is called once per frame
    void Update()
    {
        Scroll();

        if (hp)
        {
            hp.gameObject.SetActive(pawn.GetHP() != 1.0f);
            hp.value = pawn.GetHP();

        }

        // Combat
        fireTimer += Time.deltaTime;
        if (fireTimer > 1.0f / pawn.FireRate)
        {
            fireTimer = 0.0f;
            Vector3 target = GetNearestPlayer();
            target.y = 0.5f;


            if (anim) anim.Play("Attack");
            SoundManager.Instance.Play("Trebuchet");
            pawn.Bomb(target);
        }
    }
}
