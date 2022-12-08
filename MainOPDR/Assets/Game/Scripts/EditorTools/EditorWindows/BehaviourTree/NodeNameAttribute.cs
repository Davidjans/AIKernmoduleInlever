using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class NodeNameAttribute : Attribute
{
    public readonly string mName;

    public NodeNameAttribute(string name) => this.mName = name;

    public string Name => this.mName;
}
