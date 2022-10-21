public abstract class PlayersInfo
{
    public static int MaxHp = 0;
    public static int CurrentHp = 0;

    public static int Init = 0;
    public static int Movement = 0;

    public static Equipment EquipmentOne = null;
    public static Equipment EquipmentTwo = null;
    public static Passif Passif = null;
    public static Consummable Consumable = null;
}

public class WarriorInfo : PlayersInfo
{
    
}

public class ThiefInfo : PlayersInfo
{
    
}

public class ClericInfo : PlayersInfo
{
    
}

public class WizardInfo : PlayersInfo
{
    
}
