using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StoredValue<T>
{
    public Action onValueChanged;
    public string key;
    public T defaultValue;

    public StoredValue(string key, T defaultValue)
    {
        this.key = key;
        this.defaultValue = defaultValue;
    } 

    public void ChangeValue(T delta)
    {
        if (typeof(T) == typeof(int))
        {
            int current = CPlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue));
            CPlayerPrefs.SetInt(key, current + Convert.ToInt32(delta));
        }
        else if (typeof(T) == typeof(float))
        {
            float current = CPlayerPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
            CPlayerPrefs.SetFloat(key, current + Convert.ToSingle(delta));
        }
        else if (typeof(T) == typeof(double))
        {
            double current = CPlayerPrefs.GetDouble(key, Convert.ToDouble(defaultValue));
            CPlayerPrefs.SetDouble(key, current + Convert.ToDouble(delta));
        }
        if (onValueChanged != null) onValueChanged();
    }

    public void SetValue(T value)
    {
        if (typeof(T) == typeof(int))
        {
            CPlayerPrefs.SetInt(key, Convert.ToInt32(value));
        }
        else if (typeof(T) == typeof(float))
        {
            CPlayerPrefs.SetFloat(key, Convert.ToSingle(value));
        }
        else if (typeof(T) == typeof(double))
        {
            CPlayerPrefs.SetDouble(key, Convert.ToDouble(value));
        }
    }

    public T GetValue()
    {
        if (typeof(T) == typeof(int))
        {
            int value = CPlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue));
            return (T)Convert.ChangeType(value, typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            float value = CPlayerPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
            return (T)Convert.ChangeType(value, typeof(T));
        }
        else
        {
            double value = CPlayerPrefs.GetDouble(key, Convert.ToDouble(defaultValue));
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}