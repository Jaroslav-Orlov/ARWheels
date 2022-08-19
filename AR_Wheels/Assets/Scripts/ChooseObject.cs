using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseObject : MonoBehaviour
{
    private ProgramManager _programManager;

    private Button _button;

    [SerializeField] private GameObject _choosedObject;
    void Start()
    {
        _programManager = FindObjectOfType<ProgramManager>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ChooseObjectFX);
    }

    public void ChooseObjectFX()
    {
        _programManager._objectToSpawn = _choosedObject;
        _programManager._chooseObject = true;
        _programManager._scrollViwe.SetActive(false);
    }
}
