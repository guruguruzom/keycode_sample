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
                    //��Ŭ�� ����
                    continue;
                }

                if (Input.GetKey(keyCode))
                {
                    UserInputManager.Instance.PreventKeystrokes = true;
                    //Ű�� ���� ���� �ʴ´ٸ� ����
                    if (!modifyInputList.ContainsKey(keyCode))
                    {
                        modifyInputList.Add(keyCode, modifyInputList.Count);
                    }
                    isPressKey = true;
                }
            }

            //�Էµ� Ű�� ���ٸ� ���� �� ����Ʈ ����
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
        //list item ���� ����Ʈ�� �������� �ʾ� ���Ƿ� ��ü �ı�
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
            //����� ������ ���̶�� Ű�Է� ������  up���� ����
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
