using System;

/// <summary>
/// Note: Generated with help from Claude Chat to help create
/// a dropdown of Id<T>s in the Unity inspector.
/// </summary>
/// 
/// Marks a static class as the source of known Id<tag> values.
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class IdSourceAttribute : Attribute
{
    public Type Tag { get; }
    public IdSourceAttribute(Type tag) { Tag = tag; }
}