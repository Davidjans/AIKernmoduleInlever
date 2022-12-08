using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class TaskDescriptionAttribute : Attribute
{
    public readonly string mDescription;

    public TaskDescriptionAttribute(string description) => this.mDescription = description;

    public string Description => this.mDescription;
}