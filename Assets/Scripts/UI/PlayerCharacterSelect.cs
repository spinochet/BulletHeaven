using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterSelect : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private Text name;

    private Vector3 position = new Vector3(-122.5f, 0.0f, -250.0f);
    private Quaternion rotation = Quaternion.Euler(0.0f, 15.0f, 0.0f);
    private Vector3 scale = new Vector3(65.0f, 65.0f, 65.0f);

    public void UpdateSelection(GameObject _model, string _name)
    {
        if (model)
            Destroy(model);

        model = Instantiate(_model, transform);
        model.transform.localPosition = position;
        model.transform.localRotation = rotation;
        model.transform.localScale = scale;

        name.text = _name;
    }
}
