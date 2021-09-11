using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

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
    int readyP1 = -1;
    int readyP2 = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // OnJoin event callback function
    public void OnJoin(PlayerInput input)
    {
        int playerIndex = input.playerIndex + 1;
        ++numPlayers;

        // Update character select menu
        input.gameObject.name = "Player " + playerIndex.ToString();
        GameObject.Find("P" + playerIndex.ToString() + " Join Text").SetActive(false);
        GameObject.Find("P" + playerIndex.ToString() + " Portrait").GetComponent<SpriteRenderer>().enabled = true;

        // InputSystemUIInputModule uiInputModule = GameObject.Find("P" + playerIndex.ToString()).GetComponent<InputSystemUIInputModule>();
        // uiInputModule.actionsAsset = input.actions;

        input.transform.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(GameObject.Find("P" + playerIndex.ToString() + " Portrait"));

        // Setup player character
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
        Debug.Log(playerIndex.ToString());
    }
}
