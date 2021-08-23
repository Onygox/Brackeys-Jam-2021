using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Scriptable Int")]

public class ScriptableInt : CustomScriptable
{
    [SerializeField] private int _value;

    public int Value {
        get {
            return _value;
        }
        set {
            _value = isClamped ? Mathf.Clamp(value, lowClampValue, highClampValue) : value;
            if (onVariableChange != null) onVariableChange.Raise();
        }
    }

    [SerializeField] private int _defaultValue;
    public int DefaultValue {
        get {
            return _defaultValue;
        }
    }

    public bool isClamped;
    public int lowClampValue, highClampValue;

    public void SaveData() {
        PlayerPrefs.SetInt(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetInt(SaveDataPath);
    }
}
