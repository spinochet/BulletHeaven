using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinScript : MonoBehaviour
{
    public void JoinLobby()
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerNetworkManager>().StartClient();
        Destroy(gameObject);
    }
}
