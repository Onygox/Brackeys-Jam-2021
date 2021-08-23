using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

[CreateAssetMenu(menuName="Scriptable Float")]

public class ScriptableFloat : ScriptableObject
{
    [SerializeField] private float _value;

    public float Value {
        get {
            return _value;
        }
        set {
            _value = isClamped ? Mathf.Clamp(value, lowClampValue, highClampValue) : value;
            if (onVariableChange != null) onVariableChange.Raise();
        }
    }

    [SerializeField] private float _defaultValue;
    public float DefaultValue {
        get {
            return _defaultValue;
        }
    }

    public bool isClamped;
    public float lowClampValue, highClampValue;
    [SerializeField] private string _saveDataPath;
    public string SaveDataPath {
        get {
            return _saveDataPath;
        }
    }
    [SerializeField] GameEvent onVariableChange;

    public void SaveData() {
        PlayerPrefs.SetFloat(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetFloat(SaveDataPath);
    }
}
