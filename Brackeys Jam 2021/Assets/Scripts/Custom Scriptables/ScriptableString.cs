using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Scriptable String")]

public class ScriptableString : CustomScriptable
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

    public void SaveData() {
        PlayerPrefs.SetString(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetString(SaveDataPath);
    }
}
