using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [SerializeField] private Transform sphere;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float duration = 3.0f;

    private Vector3 dir;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (speed > timer)
        {
            sphere.localScale = new Vector3((timer / speed) * 8.0f, (timer / speed) * 8.0f, (timer / speed) * 8.0f);
        }
        else
        {
            renderer.enabled = true;
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
}
