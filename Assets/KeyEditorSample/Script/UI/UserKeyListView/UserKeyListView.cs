using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UserKeyListView : MonoBehaviour
{
    [Header("Copy Target")]
    [SerializeField]
    private GameObject DefaultItem;

    [Header("List Content")]
    [SerializeField]
    private Transform Content;


    private UserKeyListViewComponent m_selectItem;

    private Dictionary<KeyCode, int> modifyInputList;
    
    void Start() {
        modifyInputList = new Dictionary<KeyCode, int>();
        Init();
    }

    void Update()
    {
        if (m_selectItem != null)
        {
            bool isPressKey = false;
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode == KeyCode.Mouse0)
                {
                    //좌클릭 무시
                    continue;
                }

                if (Input.GetKey(keyCode))
                {
                    UserInputManager.Instance.PreventKeystrokes = true;
                    //키가 존재 하지 않는다면 삽입
                    if (!modifyInputList.ContainsKey(keyCode))
                    {
                        modifyInputList.Add(keyCode, modifyInputList.Count);
                    }
                    isPressKey = true;
                }
            }

            //입력된 키가 없다면 저장 후 리스트 삭제
            if (!isPressKey)
            {
                UserInputManager.Instance.PreventKeystrokes = false;
                if (modifyInputList.Count != 0)
                {
                    Modify();
                    m_selectItem.DeSelect();
                    m_selectItem = null;
                    modifyInputList.Clear();
                }
            }
        }
    }

    public void Init() {
        //list item 별도 리스트가 존재하지 않아 임의로 객체 파괴
        for (int childIndex = 0; childIndex < Content.childCount; childIndex++) {
            Destroy(Content.GetChild(childIndex).gameObject);
        }
        DefaultItem.SetActive(true);
        foreach (KeyValuePair <UserInputKeyCode, MappingKeyProfile > mappingKeyProfile in UserInputManager.Instance.InputKeyMappingList) {
            GameObject UserKeyListItem = Instantiate(DefaultItem, Content);

            UserKeyListViewDescription userOptionsListDescription = new UserKeyListViewDescription
            {
                KeyCode = mappingKeyProfile.Key,
                KeyProfile = mappingKeyProfile.Value
            };
            UserKeyListItem.GetComponent<UserKeyListViewComponent>().SetData(userOptionsListDescription);
        }
        DefaultItem.SetActive(false);
    }

    private void Modify() {
        List<KeyMember> keyMembers = new List<KeyMember>();
        foreach (KeyValuePair<KeyCode, int> modifyInputkey in modifyInputList) {
            //요소의 마지막 값이라면 키입력 판정을  up으로 지정
            if (modifyInputkey.Value == modifyInputList.Count)
            {
                keyMembers.Add(new KeyMember(modifyInputkey.Key, PressType.up));
            }
            else {
                keyMembers.Add(new KeyMember(modifyInputkey.Key, PressType.down));
            }
        }

        m_selectItem.Item.KeyProfile.keyCodes = keyMembers;
        UserInputManager.Instance.SetInputKey(m_selectItem.Item.KeyCode, keyMembers);
        m_selectItem.SetData(m_selectItem.Item);
    }

    public void Restore() 
    {
        UserInputManager.Instance.InitInputKeyMappingList();
        Init();
    }

    public void Save()
    {
        UserInputManager.Instance.Save();
    }

    public void Select(UserKeyListViewComponent userKeyListViewComponent)
    {
        if (m_selectItem != null) {
            m_selectItem.DeSelect();
        }
        m_selectItem = userKeyListViewComponent;
        m_selectItem.Select();
    }
    
}
