using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class CustomScriptable : ScriptableObject
{
    
    public int libraryIndex;

    [SerializeField] private string _saveDataPath;
    public string SaveDataPath {
        get {
            return _saveDataPath;
        }
    }

    [SerializeField] protected GameEvent onVariableChange;

    public void Initialize(int index) {
        libraryIndex = index;
    }
}
