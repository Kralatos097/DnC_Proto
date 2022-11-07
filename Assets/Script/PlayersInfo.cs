public class WarriorInfo
{
    public static Perso PlayerClass = Perso.Warrior;
    
    public static int MaxHp = 0;
    private static int _currentHp = 0;
    public static int CurrentHp
    {
        get => _currentHp;

        set
        {
            _currentHp = value;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
            else if (CurrentHp < 0)
                CurrentHp = 0;
        }
    }

    public static int Armor = 0;
    public static StatusEffect StatusEffect = StatusEffect.Nothing;

    public static int Init = 0;
    public static int Movement = 0;

    public static Equipment EquipmentOne = null;
    public static Equipment EquipmentTwo = null;
    public static Passif Passif = null;
    public static Consummable Consumable = null;
}

public class ThiefInfo
{
    public static Perso PlayerClass = Perso.Thief;
    
    public static int MaxHp = 0;
    private static int _currentHp = 0;
    public static int CurrentHp
    {
        get => _currentHp;

        set
        {
            _currentHp = value;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
            else if (CurrentHp < 0)
                CurrentHp = 0;
        }
    }

    public static int Init = 0;
    public static int Movement = 0;

    public static Equipment EquipmentOne = null;
    public static Equipment EquipmentTwo = null;
    public static Passif Passif = null;
    public static Consummable Consumable = null;
}

public class ClericInfo
{
    public static Perso PlayerClass = Perso.Cleric;
    
    public static int MaxHp = 0;
    private static int _currentHp = 0;
    public static int CurrentHp
    {
        get => _currentHp;

        set
        {
            _currentHp = value;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
            else if (CurrentHp < 0)
                CurrentHp = 0;
        }
    }

    public static int Init = 0;
    public static int Movement = 0;

    public static Equipment EquipmentOne = null;
    public static Equipment EquipmentTwo = null;
    public static Passif Passif = null;
    public static Consummable Consumable = null;
}

public class WizardInfo
{
    public static Perso PlayerClass = Perso.Wizard;

    public static int MaxHp = 0;
    private static int _currentHp = 0;
    public static int CurrentHp
    {
        get => _currentHp;

        set
        {
            _currentHp = value;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
            else if (CurrentHp < 0)
                CurrentHp = 0;
        }
    }

    public static int Init = 0;
    public static int Movement = 0;

    public static Equipment EquipmentOne = null;
    public static Equipment EquipmentTwo = null;
    public static Passif Passif = null;
    public static Consummable Consumable = null;
}
