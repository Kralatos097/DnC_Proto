using UnityEngine;

public abstract class Stuff : ScriptableObject
{
    private string Name;
    public Sprite Logo;
    public EquipType equipType;

    public abstract void Effect();

    public void Start()
    {
        string Name = name;
    }
    
    public abstract void Effect(CombatStat cs);
}
