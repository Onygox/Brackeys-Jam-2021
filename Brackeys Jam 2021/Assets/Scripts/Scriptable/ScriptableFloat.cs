using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SaveData() {
        PlayerPrefs.SetFloat(SaveDataPath, Value);
    }

    public void LoadData() {
        Value = PlayerPrefs.GetFloat(SaveDataPath);
    }
}
