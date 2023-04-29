using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInputTest : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textLog;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        SetLog(UserInputKeyCode.LEFT_MOVE);
        SetLog(UserInputKeyCode.FROBT_MOVE);
        SetLog(UserInputKeyCode.BACK_MOVE);
        SetLog(UserInputKeyCode.KEY_1);
        SetLog(UserInputKeyCode.KEY_2);
        SetLog(UserInputKeyCode.KEY_3);
        SetLog(UserInputKeyCode.KEY_4);
        SetLog(UserInputKeyCode.MULTI_KEY_1);
        SetLog(UserInputKeyCode.MULTI_KEY_2);
        SetLog(UserInputKeyCode.RIGHT_MOVE);
#endif
    }

    private void SetLog(UserInputKeyCode userInputKeyCode) {
        Debug.Log(UserInputManager.Instance.InputKeyMappingList[userInputKeyCode].comment);
        UserInputManager.Instance.InputKeyMappingList[userInputKeyCode].KeyEvent.AddListener((raiseEvent) =>
        {
            textLog.text = "key event" + UserInputManager.Instance.InputKeyMappingList[userInputKeyCode].comment;
        });
    }

}
