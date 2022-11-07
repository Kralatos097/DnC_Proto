//Dungeon
public enum RoomType
{
    Normal,
    Boss,
    Treasure,
    Fighting,
    Starting,
}

public enum RoomEffect
{
    Default,
    Boss,
    Treasure,
    Fight,
    Rest,
    Loot,
}

//Arene
public enum EquipType
{
    Default,
    Self,
    Cible,
}

public enum Action
{
    Default,
    Move,
    Attack,
    Stay,
    Equip,
    CancelMove,
}
public enum StuffSelected
{
    Default,
    EquipOne,
    EquipTwo,
    Consum,
}

public enum Perso
{
    Default,
    Warrior,
    Thief,
    Cleric,
    Wizard,
}

public enum StatusEffect
{
    Nothing,
    Poison,
    Stun,
    Burn,
    Freeze,
}
