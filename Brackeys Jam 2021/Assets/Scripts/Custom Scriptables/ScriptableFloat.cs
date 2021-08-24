using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Brackeys Jam/Scriptable Float")]

public class ScriptableFloat : CustomScriptable
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

    public void SaveData() {
        PlayerPrefs.SetFloat(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetFloat(SaveDataPath);
    }
}
