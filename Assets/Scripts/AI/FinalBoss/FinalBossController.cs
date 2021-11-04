using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : EnemyController
{
    [SerializeField] private GameObject sideLaserPrefab;
    [SerializeField] private float sideLaserRate = 1.5f;
    private float sideLaserTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Destroy(Pawn _pawn)
    {
        // PlayerNetworkManager.Instance.LoadArcadeLevel("CutScene5");
    }

    // Update is called once per frame
    void Update()
    {
        sideLaserTimer += Time.deltaTime;
        if (sideLaserTimer >= sideLaserRate)
        {
            sideLaserTimer = 0.0f;
            // Pew();
        }
    }

    // [Command(requiresAuthority = false)]
    // private void Pew()
    // {
    //     GameObject sl = GameObject.Instantiate(sideLaserPrefab);
    //     NetworkServer.Spawn(sl);
    // }
}
