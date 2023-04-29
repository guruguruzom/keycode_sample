using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UserKeyListViewComponent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public UserKeyListViewDescription Item;
    // Start is called before the first frame update

    [SerializeField]
    private TMP_Text TextName;

    [SerializeField]
    private TMP_Text TextKeyCode;

    [SerializeField]
    private Color SelectColor;

    [SerializeField]
    private Color DefaultColor;

    public SelectListEvent selectListEvent;

    public void SetData(UserKeyListViewDescription item)
    {
        string strKeyCodes = string.Empty;
        foreach (KeyMember keyMember in item.KeyProfile.keyCodes) {
            strKeyCodes += keyMember.keycode + "+";
        }

        strKeyCodes = strKeyCodes.Substring(0, strKeyCodes.Length - 1);
        Item = item;

        if (Item != null)
        {
            gameObject.name = item.KeyProfile.comment;
            TextName.text = item.KeyProfile.comment;
            TextKeyCode.text = strKeyCodes;
        }
    }

    public void Select() {
        GetComponent<Image>().color = SelectColor;
    }
    public void DeSelect()
    {
        GetComponent<Image>().color = DefaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectListEvent?.Invoke(this);
    }
}
