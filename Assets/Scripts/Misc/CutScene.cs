using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Mirror;

public class CutScene : NetworkBehaviour
{

    private float timer = 0.0f;
    public string scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f) {
            GameObject.Find("PlayerNetworkManager").GetComponent<PlayerNetworkManager>().LoadArcadeLevel(scene);
        }
    }
}
