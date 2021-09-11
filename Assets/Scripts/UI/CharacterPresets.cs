using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterPresets : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterPreset
    {
        [SerializeField] public Sprite portrait;
    }

    [SerializeField] private List<CharacterPreset> characterPresets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // OnJoin event callback function
    public void OnJoin(PlayerInput input)
    {
        if (input.playerIndex == 0)
        {
            input.gameObject.name = "Player 1";
            GameObject.Find("P1 Join Text").SetActive(false);
            GameObject.Find("P1 Portrait").GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (input.playerIndex == 1)
        {
            input.gameObject.name = "Player 2";
            GameObject.Find("P2 Join Text").SetActive(false);
            GameObject.Find("P2 Portrait").GetComponent<SpriteRenderer>().enabled = true;
        }

        input.transform.GetComponent<SpriteRenderer>().enabled = false;
        input.transform.GetComponent<PlayerController>().enabled = true;
        DontDestroyOnLoad(input.gameObject);
    }

    void OnSubmit()
    {
        Debug.Log("Submit!");
    }
}
