using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void LoadStory()
    {
        GameObject.Find("Player Manager").GetComponent<PlayerManager>().StoryMode();
    }
}
