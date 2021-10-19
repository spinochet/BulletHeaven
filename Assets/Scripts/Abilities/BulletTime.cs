using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class BulletTime : Ability
{
    [SerializeField] private float timeScale = 0.3f;

    [Command(requiresAuthority = false)]
    override public void Activate()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = timeScale / 60.0f;

        RpcActivateOnClients();
    }

    [ClientRpc]
    public void RpcActivateOnClients()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = timeScale / 60.0f;
    }

    [Command(requiresAuthority = false)]
    override public void Deactivate()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;

        RpcDeactivateOnClients();
    }

    [ClientRpc]
    public void RpcDeactivateOnClients()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }
}
