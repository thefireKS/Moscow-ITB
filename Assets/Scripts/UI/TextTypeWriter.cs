using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TextTypeWriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    [Space(5)]
    [SerializeField] private float delay = 0.05f;
    [SerializeField] private List<string> fullText;

    [Header("GameObjects that prevent text changing")]
    [SerializeField] private GameObject raycastBlocker;
    [SerializeField] private GameObject[] interfaceObjects;
    [Space(5)] 
    [SerializeField] private List<int> numbersOfTextToStopDialogue;

    private int _currentTextForDisplay = 0;
    private int _currentTextForDisplayBlocker = 0;
    
    private string _currentText = "";

    private int _length = 1;

    private float _currentTime = 0f;

    private void OnEnable() => TurnSystem.OnChangingTurn += StopGameForDialogue;
    
    private void OnDisable() => TurnSystem.OnChangingTurn -= StopGameForDialogue;

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            UpdateText();
            CheckToStopDialogue();
        }
        
        TypeWriterEffect();
    }

    private void TypeWriterEffect()
    {
        if (!(_currentTime > delay) || (fullText[_currentTextForDisplay] == _currentText)) return;
        _currentText = fullText[_currentTextForDisplay][.._length];
        _length++;
        _currentTime = 0f;
        _textField.text = _currentText;
    }

    private void UpdateText()
    {
        if(CheckForActiveObjects()) return;
        

        if (fullText[_currentTextForDisplay] != _currentText)
        {
            _length = fullText[_currentTextForDisplay].Length;
            return;
        }

        _textField.text = "";
        _currentText = "";
        _length = 1;
        
        if(_currentTextForDisplay < fullText.Count - 1)
            _currentTextForDisplay++;
    }

    private void CheckToStopDialogue()
    {
        if(numbersOfTextToStopDialogue[_currentTextForDisplayBlocker] + 1 != _currentTextForDisplay) return;
        
        raycastBlocker.SetActive(false);
        
        _currentTextForDisplayBlocker++;
        if (_currentTextForDisplayBlocker >= numbersOfTextToStopDialogue.Count - 1)
            _currentTextForDisplayBlocker = numbersOfTextToStopDialogue.Count - 1;
    }

    private void StopGameForDialogue()
    {
        raycastBlocker.SetActive(true);
    }

    private bool CheckForActiveObjects()
    {
        return !raycastBlocker.activeSelf || interfaceObjects.Any(ui => ui.activeSelf);
    }
}
