using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        public InputAction skip;
        [SerializeField] private string nextLevel;

        private void Awake()
        {
            // Enable cutscene actions
            skip.Enable();

            // Assign cutscene actions
            skip.performed += ctx => Skip();

            StartCoroutine(dialogueSequence());
        }

        private IEnumerator dialogueSequence()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            gameObject.SetActive(false);

            SceneManager.LoadScene(nextLevel);
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void Skip()
        {
            skip.Disable();
            SceneManager.LoadScene(nextLevel);
        }
    }
}

