using DG.Tweening;
using UnityEngine;

public enum DOSequenceType
{
    Append,
    Join
}

public enum DOLocationType
{
    Local,
    World
}

public enum DOTweenType
{
    Move,
    Rotate,
    Scale
}

[System.Serializable]
public class TweenData
{
    public DOTweenType type;
    public DOSequenceType sequenceType;
    public Vector3 value;
    public float duration;
    public Ease ease;
    public RotateMode rotateMode;
    public bool additive;
}
