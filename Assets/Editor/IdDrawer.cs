using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Note: Generated with help from Claude Chat.
/// </summary>
[CustomPropertyDrawer(typeof(Id<>))]
public class IdDrawer : PropertyDrawer
{
    private struct Option { public string name; public string value; }

    // tag type -> options; rebuilt automatically after a domain reload (recompile).
    private static readonly Dictionary<Type, List<Option>> Cache = new Dictionary<Type, List<Option>>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var valueProp = property.FindPropertyRelative("_value");
        var tag = GetTagType();

        EditorGUI.BeginProperty(position, label, property);

        if (tag == null)
        {
            EditorGUI.LabelField(position, label, new GUIContent("Unsupported field type"));
            EditorGUI.EndProperty();
            return;
        }

        var options = GetOptions(tag);
        if (options.Count == 0)
        {
            EditorGUI.LabelField(position, label,
                new GUIContent($"No [IdSource(typeof({tag.Name}))] class found"));
            EditorGUI.EndProperty();
            return;
        }

        var names = new List<string> { "None" };
        var values = new List<string> { "" };
        foreach (var o in options) 
        { 
            names.Add(o.name); 
            values.Add(o.value); 
        }

        string current = valueProp.stringValue;
        int index = values.IndexOf(current);
        if (index < 0)
        {
            names.Add($"Missing (\"{current}\")");
            values.Add(current);
            index = names.Count - 1;
        }

        EditorGUI.BeginChangeCheck();
        int picked = EditorGUI.Popup(position, label.text, index, names.ToArray());
        if (EditorGUI.EndChangeCheck())
            valueProp.stringValue = values[picked];

        EditorGUI.EndProperty();
    }

    private static List<Option> GetOptions(Type tag)
    {
        if (Cache.TryGetValue(tag, out var cached)) return cached;

        var list = new List<Option>();
        var idType = typeof(Id<>).MakeGenericType(tag);

        // TypeCache is Unity's fast, domain-reload-aware type lookup.
        foreach (var type in TypeCache.GetTypesWithAttribute<IdSourceAttribute>())
        {
            bool matches = false;
            foreach (var attr in type.GetCustomAttributes<IdSourceAttribute>())
                if (attr.Tag == tag) { matches = true; break; }
            if (!matches) continue;

            foreach (var f in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (f.FieldType != idType) continue;
                var id = f.GetValue(null);
                string value = (string)idType.GetProperty("Value").GetValue(id);
                if (string.IsNullOrEmpty(value)) continue;
                list.Add(new Option { name = ObjectNames.NicifyVariableName(f.Name), value = value });
            }
        }

        list.Sort((a,b) => string.CompareOrdinal(a.value, b.value));
        Cache[tag] = list;
        return list;
    }

    private Type GetTagType()
    {
        var t = fieldInfo.FieldType;
        if (t.IsArray) t = t.GetElementType();
        else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            t = t.GetGenericArguments()[0];
        return t.IsGenericType ? t.GetGenericArguments()[0] : null;
    }
}