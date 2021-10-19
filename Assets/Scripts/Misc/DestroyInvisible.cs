using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInvisible : MonoBehaviour
{
    [SerializeField] private bool destroyParent;

    public void OnBecameInvisible()
    {
        if (destroyParent)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
