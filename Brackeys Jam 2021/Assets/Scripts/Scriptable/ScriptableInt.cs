using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

[CreateAssetMenu(menuName="Scriptable Int")]

public class ScriptableInt : ScriptableObject
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
    [SerializeField] private string _saveDataPath;
    public string SaveDataPath {
        get {
            return _saveDataPath;
        }
    }
    [SerializeField] GameEvent onVariableChange;

    public void SaveData() {
        PlayerPrefs.SetInt(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetInt(SaveDataPath);
    }
}
