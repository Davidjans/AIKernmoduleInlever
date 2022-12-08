using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class StateDisplayAttribute : Attribute
{
    public readonly string mState;
    public readonly int mPriority;
    public StateDisplayAttribute(string state,int priority)
    {
        this.mState = state;
        this.mPriority = priority;
    }

    public string State => this.mState;
    public int Priority => this.mPriority;
}
