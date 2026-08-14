using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;

public static class DebugLogExtensions
{
    private const string LogColor = nameof(Color.white);
    private const string LogWarningColor = nameof(Color.yellow);
    private const string LogErrorColor = nameof(Color.red);

    public static void Log(this object msg, Object context = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        Debug.Log($"<color={LogColor}>[{fileName} | {methodName}]</color>: {msg}", context);
    }

    public static void LogWarning(this object msg, Object context = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        Debug.LogWarning($"<color={LogWarningColor}>[{fileName} | {methodName}]</color>: {msg}", context);
    }

    public static void LogError(this object msg, Object context = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        Debug.LogError($"<color={LogErrorColor}>[{fileName} | {methodName}]</color>: {msg}", context);
    }
}