using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    abstract public bool Activate();
    abstract public void Deactivate();
    abstract public float GetCost();
}
