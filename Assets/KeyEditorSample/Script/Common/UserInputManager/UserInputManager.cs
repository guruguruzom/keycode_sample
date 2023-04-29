using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MappingKeyProfile
{
    public UnityEvent<bool> KeyEvent;
    public string comment;
    public List<KeyMember> keyCodes;// = new List<KeyMember>();

    public MappingKeyProfile(List<KeyMember> _keyCodes, string _comment, UnityEvent<bool> _keyEvent)
    {
        keyCodes = _keyCodes;
        comment = _comment;
        KeyEvent = _keyEvent;
    }

    public void SetKeyCodes(List<KeyMember> _keyCodes)
    {
        keyCodes = _keyCodes;
    }
}

public struct KeyMember
{
    public KeyCode keycode;
    public PressType pressType;

    public KeyMember(KeyCode _keycode, PressType _pressType = PressType.up)
    {
        keycode = _keycode;
        pressType = _pressType;
    }
}

public enum PressType
{
    down, up
}
public class UserInputManager : MonoSingleton<UserInputManager>
{
    Dictionary<UserInputKeyCode, MappingKeyProfile> m_inputKeyMappingList = new Dictionary<UserInputKeyCode, MappingKeyProfile>();

    public UnityEvent MouseWheelUpEvent = new UnityEvent();
    public UnityEvent MouseWheelDownEvent = new UnityEvent();

    UserInputKeyCode m_preInputKeyCode = UserInputKeyCode.NONE;

    readonly float INPUT_KEY_EVENT_INTERVAL_TIME_SEC = 0.25f;

    private string keyListFilePath;
    public string KeyListFilePath { get => keyListFilePath; set => keyListFilePath = value; }

    private bool preventKeystrokes = true;
    public bool PreventKeystrokes { get => preventKeystrokes; set => preventKeystrokes = value; }

    public Dictionary<UserInputKeyCode, MappingKeyProfile> InputKeyMappingList
    {
        get => m_inputKeyMappingList;
    }

    private new void Awake()
    {
        base.Awake();

        KeyListFilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Config/keylist.json");
        Debug.Log(KeyListFilePath);

        if (!File.Exists(KeyListFilePath))
        {
            File.Create(KeyListFilePath);
        }

        InputKeyMappingList[UserInputKeyCode.LEFT_MOVE] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.LeftArrow) }, "move frame left", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.RIGHT_MOVE] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.RightArrow) }, "move frame right", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.FROBT_MOVE] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.UpArrow) }, "move frame front", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.BACK_MOVE] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.DownArrow) }, "move frame back", new UnityEvent<bool>());

        InputKeyMappingList[UserInputKeyCode.KEY_1] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.Q) }, "key 1", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.KEY_2] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.W) }, "key 2", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.KEY_3] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.E) }, "key 3", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.KEY_4] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.R) }, "key 4", new UnityEvent<bool>());

        InputKeyMappingList[UserInputKeyCode.MULTI_KEY_1] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.LeftControl, PressType.down), new KeyMember(KeyCode.Z) }, "multi key 1", new UnityEvent<bool>());
        InputKeyMappingList[UserInputKeyCode.MULTI_KEY_2] = new MappingKeyProfile(new List<KeyMember>() { new KeyMember(KeyCode.LeftAlt, PressType.down), new KeyMember(KeyCode.A) }, "multi key 2", new UnityEvent<bool>());
        preventKeystrokes = false;
    }

    private void Start()
    {
        string strJsonData = File.ReadAllText(KeyListFilePath, System.Text.Encoding.UTF8);

        if (string.IsNullOrEmpty(strJsonData))
        {
            string json = JsonConvert.SerializeObject(InputKeyMappingList);
            File.WriteAllText(KeyListFilePath, json, System.Text.Encoding.UTF8);
        }
        else
        {
            Dictionary<UserInputKeyCode, MappingKeyProfile> inputKeyMappingList = new Dictionary<UserInputKeyCode, MappingKeyProfile>();
            inputKeyMappingList = JsonConvert.DeserializeObject<Dictionary<UserInputKeyCode, MappingKeyProfile>>(strJsonData);

            foreach (var inputKeyItem in inputKeyMappingList)
            {
                m_inputKeyMappingList[inputKeyItem.Key].SetKeyCodes(inputKeyItem.Value.keyCodes);
            }
        }
    }

    public void SetInputKey(UserInputKeyCode userInputKeyCode, List<KeyMember> keyMembers)
    {
        m_inputKeyMappingList[userInputKeyCode].SetKeyCodes(keyMembers);
    }


    public void Save()
    {
        string json = JsonConvert.SerializeObject(InputKeyMappingList);
        File.WriteAllText(KeyListFilePath, json, System.Text.Encoding.UTF8);
    }

    public void Restore()
    {
        string json = JsonConvert.SerializeObject(InputKeyMappingList);
        File.WriteAllText(KeyListFilePath, json, System.Text.Encoding.UTF8);
    }

    public void InitInputKeyMappingList()
    {
        InputKeyMappingList[UserInputKeyCode.LEFT_MOVE].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.LeftArrow) });
        InputKeyMappingList[UserInputKeyCode.RIGHT_MOVE].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.RightArrow) });
        InputKeyMappingList[UserInputKeyCode.FROBT_MOVE].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.UpArrow) });
        InputKeyMappingList[UserInputKeyCode.BACK_MOVE].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.DownArrow) });

        InputKeyMappingList[UserInputKeyCode.KEY_1].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.Q) });
        InputKeyMappingList[UserInputKeyCode.KEY_2].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.W) });
        InputKeyMappingList[UserInputKeyCode.KEY_3].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.E) });
        InputKeyMappingList[UserInputKeyCode.KEY_4].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.R) });

        InputKeyMappingList[UserInputKeyCode.MULTI_KEY_1].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.LeftControl, PressType.down), new KeyMember(KeyCode.Z) });
        InputKeyMappingList[UserInputKeyCode.MULTI_KEY_2].SetKeyCodes(new List<KeyMember>() { new KeyMember(KeyCode.LeftAlt, PressType.down), new KeyMember(KeyCode.A) });
    }

    void Update()
    {
        bool bRet = false;

        do
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;

            if (selectedGameObject == null)
            {
                bRet = true;
                break;
            }

            var tmpInputField = selectedGameObject.GetComponent<TMPro.TMP_InputField>();

            if (tmpInputField != null)
                break;

            var inputField = selectedGameObject.GetComponent<InputField>();

            if (inputField != null)
                break;

            bRet = true;
        }
        while (false);

        if (bRet == false)
            return;

        OnProcessUserInputKeyEvent();
        OnProcessUserInputMouseEvent();
    }



    void OnProcessUserInputKeyEvent()
    {
        var sortedInputKeyMappingList = InputKeyMappingList.OrderByDescending(item => item.Value.keyCodes.Count).ToList();

        foreach (KeyValuePair<UserInputKeyCode, MappingKeyProfile> userInputKey in sortedInputKeyMappingList)
        {
            int keyHitCount = 0;
            foreach (KeyMember keyMember in userInputKey.Value.keyCodes)
            {
                if (keyMember.pressType == PressType.down)
                {
                    if (Input.GetKey(keyMember.keycode))
                    {
                        keyHitCount++;
                    }
                }
                else if (keyMember.pressType == PressType.up)
                {
                    if (Input.GetKeyUp(keyMember.keycode))
                    {
                        keyHitCount++;
                    }

                }
            }
            if (keyHitCount == userInputKey.Value.keyCodes.Count)
            {
                if (!preventKeystrokes)
                {
                    userInputKey.Value.KeyEvent?.Invoke(true);
                }

                return;
            }
        }
    }

    void OnProcessUserInputMouseEvent()
    {
        float fWheelInputValue = Input.GetAxis("Mouse ScrollWheel");

        // Wheel Up
        if (fWheelInputValue > 0)
        {
            MouseWheelUpEvent?.Invoke();
        }
        // Wheel Down
        else if (fWheelInputValue < 0)
        {
            MouseWheelDownEvent?.Invoke();
        }
    }
}
