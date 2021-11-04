using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public InputAction space;
        private bool spacePressed = false;

        public bool finished { get; private set; }

        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay)
        {
            space.Enable();
            space.performed += ctx => OnSpace();

            textHolder.color = textColor;
            textHolder.font = textFont;

            spacePressed = false;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];

                //SoundManager.instance.PlaySound(sound);
                if (!spacePressed)
                    yield return new WaitForSeconds(delay);
            }

            spacePressed = false;
            while (!spacePressed) yield return null;

            finished = true;
        }

        void OnSpace()
        {
            spacePressed = true;
        }
    }
}

