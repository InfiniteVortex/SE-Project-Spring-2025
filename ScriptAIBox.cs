using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptAIBox : MonoBehaviour
{
    [SerializedField]
    [TextArea]
    private List<string> _dialogueLines;
    private int _lineIndex;

    private TMP_text _text;
    private CanvasGroup _group;
    private bool _started;

    // Start is called before the first frame update
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        _group = GetComponent<TMP_Text>();
        _group.alpha = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!_started) {
                _lineIndex = 0;
                _text.SetText(_dialogueLines[_lineIndex]);
                _group.alpha = 1;
                _started = true;
        } else if (_lineIndex < _dialogueLines.Count) {
                _text.SetText(_dialogueLines[_lineIndex++]);
    }
            else {
                _group.alpha = 0;
}
