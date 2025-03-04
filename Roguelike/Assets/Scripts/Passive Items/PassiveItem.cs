using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;
    
    protected virtual void ApplyModifier()
    {
        //Применение значения boost к соответствующему состоянию в дочерних классах
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }
}
