using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    [Header ("Model")]
    public GameObject model;
    public Sprite portrait;
    public string name;

    [Header ("Movement")]
    public float speed = 10.0f;
    public float dashDist = 5.0f;

    [Header ("Stats")]
    public float maxHP = 100.0f;
    public float hpRegenRate = 0.0f;
    public float hpRegenCooldown = 0.0f;
    [Space (10)]
    public float maxStamina = 100.0f;
    public float staminaRegenRate = 5.0f;
    public float staminaRegenCooldown = 0.5f;

    [Header ("Combat")]
    public GameObject bulletPrefab;
    public GameObject burstPrefab;

    [Header ("Abilities")]
    public Ability abilityL;
    public Ability abilityR;
}
