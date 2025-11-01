using UnityEngine;


[CreateAssetMenu(fileName = "DestroyerSpawnPreset", menuName = "Presets/DestroyerSpawn", order = 0)]
public class DestroyerSpawnPreset : ScriptableObject
{
    [SerializeField]
    private ExplosionType[] destroyers;
    public ExplosionType[] Destroyers => destroyers;
   
}
