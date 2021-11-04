using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Pause pause = null;

    private Slider hpBar = null;
    private Slider staminaBar = null;
    private RawImage portrait = null;
    private Text text = null;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        hpBar = transform.Find("HP Bar").GetComponent<Slider>();
        staminaBar = transform.Find("Stamina Bar").GetComponent<Slider>();
        portrait = transform.Find("Portrait/Character").GetComponent<RawImage>();
        text = transform.Find("Score/ScoreText").GetComponent<Text>();
    }

    // Update current health
    public void UpdateHealth(float value)
    {
        if (hpBar == null)
            hpBar = transform.Find("HP Bar").GetComponent<Slider>();
            
        hpBar.value = value;
    }

    // Update current stamina
    public void UpdateStamina(float value)
    {
        if (staminaBar == null)
            transform.Find("Stamina Bar").GetComponent<Slider>();
        
        staminaBar.value = value;
    }

    // Update character portrait
    public void UpdatePortrait(Texture texture)
    {
        if (portrait == null)
            portrait = transform.Find("Portrait/Character").GetComponent<RawImage>();

        portrait.texture = texture;
    }

    // Update current score
    public void UpdateScore(int score)
    {
        if (text == null)
            transform.Find("Score/ScoreText").GetComponent<Text>();
        
        text.text = score.ToString();
    }
}
