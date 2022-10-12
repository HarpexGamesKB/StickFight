using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Reflection;

public static class Log
{
    private static bool debug = true;
    private static List<string> _blackList = new List<string>();

    public static void AddToBlackList()
    {
        string className = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
        if (_blackList.Contains(className)) return;
        _blackList.Add(className);
    }

    public static void Event(object obj)
    {
        if (!debug) return;
        MethodBase method = new StackTrace().GetFrame(1).GetMethod();
        string methodName = method.Name;
        string className = method.DeclaringType.FullName;
        //UnityEngine.Debug.Log($"<color=#f00>> {className} > {methodName} > {obj}</color>");
    }

    public static void Event(bool needParams = true)
    {
        if (!debug) return;
        MethodBase method = new StackTrace().GetFrame(1).GetMethod();
        string methodName = method.Name;
        string parametrs = null;
        if (needParams)
        {
            var _parametrs = method?.GetParameters();
            if (parametrs != null)
            {
                parametrs = " > " + _parametrs?.GetValue(0)?.ToString();
            }
        }
        string className = method.DeclaringType.FullName;
        Logging($"> {className} > {methodName}{parametrs}", Color.yellow);
    }

    public static void _(object _object, UnityEngine.Object context = null)
    {
        Logging(_object, Color.red, context);
    }

    public static void _(params object[] objects)
    {
        string text = "";
        for (int i = 0; i < objects.Length; i++)
        {
            if (i > 0 && i < objects.Length)
            {
                text += " ";
            }
            text += objects[i];
        }
        Logging(text, Color.red);
    }

    public static void _(object _object, Color color, UnityEngine.Object context = null)
    {
        Logging(_object, color, context);
    }

    private static void Logging(object _object, Color color, UnityEngine.Object context = null)
    {
        if (!debug) return;
        string className = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
        if (_blackList.Contains(className)) return;
        //UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{_object}</color>", context);
    }

    public static void _<T>(IEnumerable<T> ienum, object _object = null)
    {
        string finalString = "";
        if (ienum.GetType() != finalString.GetType())
        {
            foreach (T t in ienum)
            {
                finalString += t.ToString() + " ";
            }
        }
        else
        {
            finalString = ienum.ToString();
        }

        _(_object, finalString);
    }
}