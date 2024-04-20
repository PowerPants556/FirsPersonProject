using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemData
{
    public string name;
    [Multiline] public string description;
    public int id, count;
    public bool isUniq;

    public ItemData(string name, string description, int id, int count, bool isUniq)
    {
        this.name = name;
        this.description = description;
        this.id = id;
        this.count = count;
        this.isUniq = isUniq;
    }
}
