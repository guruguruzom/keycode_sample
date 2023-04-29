using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserKeyListViewComponent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public UserKeyListViewDescription Item;
    // Start is called before the first frame update

    [SerializeField]
    private Text TextName;

    [SerializeField]
    private Text TextKeyCode;

    [SerializeField]
    private SelectListEvent selectListEvent;


    public void SetData(UserKeyListViewDescription item)
    {
        string strKeyCodes = string.Empty;
        foreach (KeyMember keyMember in item.KeyProfile.keyCodes) {
            strKeyCodes += keyMember.keycode + "+";
        }

        strKeyCodes.Substring(0, strKeyCodes.Length - 1);
        Item = item;

        if (Item != null)
        {
            TextName.text = item.KeyProfile.comment;
            TextKeyCode.text = strKeyCodes;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectListEvent.Invoke(this);
    }
}
