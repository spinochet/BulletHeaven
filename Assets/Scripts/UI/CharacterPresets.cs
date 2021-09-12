using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterPresets : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterPreset
    {
        [SerializeField] public Sprite portrait;
        [SerializeField] public RuntimeAnimatorController anim;
        [SerializeField] public GameObject bulletPrefab;
        [SerializeField] public float fireRate;
        [SerializeField] public GameObject ability;
    }

    [SerializeField] private List<CharacterPreset> characterPresets;
    int numPlayers = 0;
    int readyP1 = 1;
    int readyP2 = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // OnJoin event callback function
    public void OnJoin(PlayerInput input)
    {
        int playerIndex = input.playerIndex + 1;
        ++numPlayers;

        if (playerIndex == 1) readyP1 = -1;
        else if (playerIndex == 2) readyP2 = -1;

        // Setup player input
        input.transform.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("P" + playerIndex.ToString() + " Portrait"));

        // Setup player character
        input.gameObject.name = "Player " + playerIndex.ToString();
        input.transform.GetComponent<PlayerController>().SetUp(characterPresets[playerIndex - 1]);
        DontDestroyOnLoad(input.gameObject);
    }

    // OnJoin event callback function
    public void OnPlayerLeft(PlayerInput input)
    {
        int playerIndex = input.playerIndex + 1;
        --numPlayers;
    }

    public void OnSubmit(int playerIndex)
    {
        if (playerIndex == 1) ++readyP1;
        else if (playerIndex == 2) ++readyP2;

        if (readyP1 >= 1 && readyP2 >= 1)
        {
            SceneManager.LoadScene("FinalBoss");
        }
    }
}
