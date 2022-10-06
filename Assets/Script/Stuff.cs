using UnityEngine;

public abstract class Stuff : ScriptableObject
{
    public string Name;
    public Sprite Logo;
    public EquipType equipType;

    public abstract void Effect();
}
