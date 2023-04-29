using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class UserKeyListViewDescription : INotifyPropertyChanged
{

    [SerializeField]
    [FormerlySerializedAs("KeyCode")]
    UserInputKeyCode keyCode;
    public UserInputKeyCode KeyCode
    {
        get
        {
            return keyCode;
        }

        set
        {
            if (keyCode != value)
            {
                keyCode = value;
                OnPropertyChangedChanged("KeyCode");
            }
        }
    }

    [SerializeField]
    [FormerlySerializedAs("KeyProfile")]
    MappingKeyProfile keyProfile;

    public MappingKeyProfile KeyProfile
    {
        get
        {
            return keyProfile;
        }

        set
        {
            if (keyProfile != value)
            {
                keyProfile = value;
                OnPropertyChangedChanged("KeyProfile");
            }
        }
    }

    [SerializeField]
    [FormerlySerializedAs("ID")]
    int id;

    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChangedChanged("ID");
            }
        }
    }



    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChangedChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
