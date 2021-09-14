using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.unscaledDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Edge")
        {
            Destroy(gameObject);
        }
    }
}
