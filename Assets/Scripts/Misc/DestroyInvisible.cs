using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInvisible : MonoBehaviour
{
    [SerializeField] private GameObject destroy;

    private bool flag = false;

    public void OnBecameInvisible()
    {
        if (flag)
        {
            Destroy(destroy);
        }
    }

    void Update()
    {
        if (!flag)
        {
            flag = CheckBounds();
        }
    }

    private bool CheckBounds()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);

        bool up = position.y <= Camera.main.pixelHeight;
        bool down = position.y >= 0.0f;
        bool right = position.x <= Camera.main.pixelWidth;
        bool left = position.x >= 0.0f;

        return up && down && right && left;
    }
}
