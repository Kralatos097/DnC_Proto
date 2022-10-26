using UnityEngine;
using UnityEngine.Serialization;

public abstract class Stuff : ScriptableObject
{
    private string Name;
    public Sprite Logo;
    //public EquipType equipType;
    public string description;
    [HideInInspector] public string stuffType;

    public abstract void Effect();

    protected void Start()
    {
        string Name = name;
    }
    
    public abstract void Effect(CombatStat cs);
}
