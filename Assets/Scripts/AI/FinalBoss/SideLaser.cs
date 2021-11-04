using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideLaser : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private Vector3 position = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 scale = Vector3.zero;

    private float timer;

    void Awake()
    {
        float f = Random.Range(0.0f, 1.0f);

        // Left
        if (f < 0.25f)
        {
            position.x = 0.0f;
            position.y = Random.Range(Camera.main.pixelHeight * 0.30f, Camera.main.pixelHeight * 0.70f);

            rotation.y = 90.0f;
        }
        // Right
        else if (f < 0.5f)
        {
            position.x = Camera.main.pixelWidth;
            position.y = Random.Range(Camera.main.pixelHeight * 0.30f, Camera.main.pixelHeight * 0.7f);

            rotation.y = 270.0f;
        }
        // Down
        else if (f < 0.75f)
        {
            position.x = Random.Range(Camera.main.pixelWidth * 0.30f, Camera.main.pixelWidth * 0.7f);
            position.y = 0.0f;

            rotation.y = 0.0f;
        }
        // Up
        else
        {
            position.x = Random.Range(Camera.main.pixelWidth * 0.30f, Camera.main.pixelWidth * 0.7f);
            position.y = Camera.main.pixelHeight;

            rotation.y = 180.0f;
        }

        position = Camera.main.ScreenToWorldPoint(position);
        position.y = 0.5f;

        rotation.y += Random.Range(-20.0f, 20.0f);

        scale.y = 1.0f;
        scale.z = 1.0f;

        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2.5f)
        {
            timer = 0.0f;

            GameObject b = Instantiate(bulletPrefab, transform.position, transform.rotation);

            Destroy(gameObject);

            // float f = Random.Range(0.0f, 1.0f);
            // // Left
            // if (f < 0.25f)
            // {
            //     position.x = 0.0f;
            //     position.y = Random.Range(Camera.main.pixelHeight * 0.30f, Camera.main.pixelHeight * 0.70f);

            //     rotation.y = 90.0f;
            // }
            // // Right
            // else if (f < 0.5f)
            // {
            //     position.x = Camera.main.pixelWidth;
            //     position.y = Random.Range(Camera.main.pixelHeight * 0.30f, Camera.main.pixelHeight * 0.7f);

            //     rotation.y = 270.0f;
            // }
            // // Down
            // else if (f < 0.75f)
            // {
            //     position.x = Random.Range(Camera.main.pixelWidth * 0.30f, Camera.main.pixelWidth * 0.7f);
            //     position.y = 0.0f;

            //     rotation.y = 0.0f;
            // }
            // // Up
            // else
            // {
            //     position.x = Random.Range(Camera.main.pixelWidth * 0.30f, Camera.main.pixelWidth * 0.7f);
            //     position.y = Camera.main.pixelHeight;

            //     rotation.y = 180.0f;
            // }

            // position = Camera.main.ScreenToWorldPoint(position);
            // position.y = 0.5f;
            // rotation.y += Random.Range(-20.0f, 20.0f);
            // scale.x = 0.0f;

            // transform.position = position;
            // transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            scale.x = (timer / 2.5f) * 1.5f;
            transform.localScale = scale;
        }
    }
}
