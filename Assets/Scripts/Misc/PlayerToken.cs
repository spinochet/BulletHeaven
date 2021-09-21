using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    private Transform player;
    public PlayerCharacterSelect characterSelect;

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            transform.position = player.position + (Vector3.up * 9.0f);
        }

        if (characterSelect != null)
            sprite.enabled = !characterSelect.IsReady();
    }

    // Initialize token
    public void Init(Transform _player, PlayerCharacterSelect _characterSelect)
    {
        player = _player;
        characterSelect = _characterSelect;
    }
}
