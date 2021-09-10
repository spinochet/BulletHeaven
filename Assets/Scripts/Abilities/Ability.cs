using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    abstract public void Activate();
    abstract public void Deactivate();
}
