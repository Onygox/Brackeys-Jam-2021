using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

[CreateAssetMenu(menuName="Scriptable String")]

public class ScriptableString : ScriptableObject
{
    [SerializeField] private string _value;

    public string Value {
        get {
            return _value;
        }
        set {
            _value = value;
            if (onVariableChange != null) onVariableChange.Raise();
        }
    }

    [SerializeField] private string _defaultValue;
    public string DefaultValue {
        get {
            return _defaultValue;
        }
    }

    [SerializeField] private string _saveDataPath;
    public string SaveDataPath {
        get {
            return _saveDataPath;
        }
    }
    [SerializeField] GameEvent onVariableChange;

    public void SaveData() {
        PlayerPrefs.SetString(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetString(SaveDataPath);
    }
}
