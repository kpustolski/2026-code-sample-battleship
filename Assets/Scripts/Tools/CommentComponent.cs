using UnityEngine;

/// <summary>
/// Helpful script for adding comments to prefabs in the inspector.
/// </summary>
public class CommentComponent : MonoBehaviour
{
    [TextArea(5, 5)]
    [SerializeField]
    private string _comment;
}