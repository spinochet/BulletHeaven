using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject princess;
    [SerializeField] private GameObject robot;

    public void Join (PlayerInput input)
    {
        if (input.playerIndex == 0)
        {
            input.transform.position = new Vector3(-20.0f, 3.0f, -7.0f);
            GameObject obj = Instantiate(princess, Vector3.zero, Quaternion.Euler(-40.0f, 180.0f, 0.0f), input.transform);
            obj.transform.localPosition = Vector3.zero;

            input.gameObject.GetComponent<NewPlayerController>().SetUI(GameObject.Find("P1 HP Bar").GetComponent<Slider>(), GameObject.Find("P1 Stamina Bar").GetComponent<Slider>());
        }
        else if (input.playerIndex == 1)
        {
            input.transform.position = new Vector3(20.0f, 3.0f, -7.0f);
            GameObject obj = Instantiate(robot, new Vector3(20.0f, 3.0f, -7.0f), Quaternion.Euler(-40.0f, 180.0f, 0.0f), input.transform);
            obj.transform.localPosition = Vector3.zero;

            input.gameObject.GetComponent<NewPlayerController>().SetUI(GameObject.Find("P1 HP Bar").GetComponent<Slider>(), GameObject.Find("P1 Stamina Bar").GetComponent<Slider>());
        }
    }
}
