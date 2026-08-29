using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;

public static class DebugLogExtensions
{
    private const string LogColor = nameof(Color.white);
    private const string LogWarningColor = nameof(Color.yellow);
    private const string LogErrorColor = nameof(Color.red);

    public static void Log(this object obj, string msg, Object context = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
    {
        // File name is the class and member name is the method name.
        // .ctor means the log call is being used in a constructor.
        string fileName = Path.GetFileName(filePath);
        Debug.Log($"<color={LogColor}>[{fileName} | {memberName}]</color>: {msg}", context);
    }

    public static void LogWarning(this object obj, string msg, Object context = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
    {
        string fileName = Path.GetFileName(filePath);
        Debug.LogWarning($"<color={LogWarningColor}>[{fileName} | {memberName}]</color>: {msg}", context);
    }

    public static void LogError(this object obj, string msg, Object context = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
    {
        string fileName = Path.GetFileName(filePath);
        Debug.LogError($"<color={LogErrorColor}>[{fileName} | {memberName}]</color>: {msg}", context);
    }
}