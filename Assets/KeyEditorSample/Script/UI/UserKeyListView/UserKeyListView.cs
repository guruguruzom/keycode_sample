using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserKeyListView : MonoBehaviour
{
    [SerializeField]
    private GameObject UserKeyListPrefab;

    [SerializeField]
    private SelectListEvent selectListEvent;

    private UserKeyListViewComponent m_selectItem;

    
    void Start() {
        Init();
    }

    public void Init() {
        
        foreach (KeyValuePair <UserInputKeyCode, MappingKeyProfile > mappingKeyProfile in UserInputManager.Instance.InputKeyMappingList) {
            GameObject UserKeyListItem = Instantiate(UserKeyListPrefab);

            UserKeyListViewDescription userOptionsListDescription = new UserKeyListViewDescription
            {
                KeyCode = mappingKeyProfile.Key,
                KeyProfile = mappingKeyProfile.Value
            };
            UserKeyListItem.GetComponent<UserKeyListViewComponent>().SetData(userOptionsListDescription);
        }
        UserKeyListPrefab.SetActive(false);
    }

    public void Select(UserKeyListViewComponent userKeyListViewComponent)
    {
        if (m_selectItem != null) { 
            
        }
        m_selectItem = userKeyListViewComponent;
    }
    
}
