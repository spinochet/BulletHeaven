using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInvisible : MonoBehaviour
{
    [SerializeField] private GameObject destroy;

    public void OnBecameInvisible()
    {
        Destroy(destroy);
    }
}
