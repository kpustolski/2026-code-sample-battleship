using System;

/// <summary>
/// Note: Generated with help from Claude Chat to help create
/// a dropdown of strongly typed ids (Id<T>) in the Unity inspector.
/// Marks a static class as the source of known Id<tag> values.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class IdSourceAttribute : Attribute
{
    public Type Tag { get; }
    public IdSourceAttribute(Type tag) 
    { 
        Tag = tag; 
    }
}