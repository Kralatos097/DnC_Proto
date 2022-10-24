using UnityEngine;

public abstract class Stuff : ScriptableObject
{
    private string Name;
    public Sprite Logo;
    //public EquipType equipType;
    [HideInInspector] public string stuffType;

    public abstract void Effect();

    protected void Start()
    {
        string Name = name;
    }
    
    public abstract void Effect(CombatStat cs);
}
