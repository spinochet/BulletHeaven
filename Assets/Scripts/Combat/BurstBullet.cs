using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBullet : MonoBehaviour
{
    [SerializeField] private float chargeTime;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource audio;

    private Vector3 size;
    private float timer;
    private bool isShot;

    // Awake is called when object is instanced
    void Awake()
    {
        size = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShot)
        {
            transform.position += transform.forward * speed * Time.unscaledDeltaTime;
        }
        else if (timer < chargeTime)
        {
            timer += Time.unscaledDeltaTime;
            transform.localScale = size * (timer / chargeTime);
        }
    }

    public void Shoot()
    {
        isShot = true;
        audio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Edge")
        {
            Destroy(gameObject);
        }
    }
}
