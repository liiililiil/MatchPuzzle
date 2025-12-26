using System;
using UnityEngine;


[CreateAssetMenu(fileName = "StageClearTargetPreset", menuName = "Presets/StageClearTargetPreset", order = 0)]
public class StageClearTargetPreset : ScriptableObject
{
    [SerializeField]
    private StageClearTarget[] _stageClearTargets;
    public StageClearTarget[] stageClearTargets => _stageClearTargets;
}

[Serializable]
public class StageClearTarget
{
    public TileType type;
    public byte targetCount;
}