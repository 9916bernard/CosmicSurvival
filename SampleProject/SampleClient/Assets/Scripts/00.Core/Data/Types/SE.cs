
// [System Enum]


public enum ETB_TEAM : byte
{
    NONE     = 0,
    FRIENDLY = 1,
    OPPOSITE = 2,
};

public enum ETB_SPUM_ANIM : byte
{
    IDLE          = 0,
    RUN           = 1,
    ATTACK_BOW    = 2,
    ATTACK_MAGIC  = 3,
    ATTACK_NORMAL = 4,
    DEBUFF_STUN   = 5,
    DEATH         = 6,
    SKILL_BOW     = 7,
    SKILL_MAGIC   = 8,
    SKILL_NORMAL  = 9,
    HIT           = 10,
    HIT_CRI       = 11,
    MAX           = 12,
};

public enum ETB_CHARACTER : byte
{
    NONE    = 0,
    PLAYER  = 1,
    MONSTER = 2,
};

public enum ETB_CHARACTER_GRADE : byte
{
    NONE = 0,
    E    = 1,
    D    = 2,
    C    = 3,
    B    = 4,
    A    = 5,
    R    = 6,
    SR   = 7,
    SS   = 8,
};

public enum ETB_STAT : ushort
{
    NONE            = 0,
    ATTACK          = 1,
    ATTACK_RATE     = 2,
    DEFENSE         = 3,
    DEFENSE_RATE    = 4,
    LIFE            = 5,
    LIFE_RATE       = 6,
    SPEED           = 7,
    CRITICAL_RATE   = 8,
    CRITICAL_DAMAGE = 9,
    CONTINUOUS      = 10,
    DRAIN_LIFE      = 11,
    COUNTER         = 12,
    STUN            = 13,
    EVADE           = 14,
};

public enum ETB_SHIP_TYPE : byte
{
    NONE   = 0,
    PLAYER = 1,
    BASE   = 2,
    ENEMY  = 3,
};

public enum ETB_SHIP_GRADE : byte
{
    NONE   = 0,
    NORMAL = 1,
    ELITE  = 2,
    BOSS   = 3,
};

public enum ETB_FUND : byte
{
    NONE             = 0,
    GOLD             = 1,
    DIAMOND          = 2,
    MINERAL_RUBY     = 3,
    MINERAL_SAPPAIRE = 4,
    MINERAL_PEARL    = 5,
    MINERAL_AMBER    = 6,
    MINERAL_EMERALD  = 7,
    MINERAL_BLACK    = 8,
};

public enum ETB_UPGRADE_EFFECT : ushort
{
    NONE            = 0,
    PLAYER_ATK      = 1000,
    PLAYER_HP       = 1001,
    PLAYER_SPD      = 1002,
    PLAYER_FIRERATE = 1003,
    PLAYER_DURATION = 1004,
    PLAYER_PRJNUM   = 1005,
    PLAYER_REROLL   = 1006,
    PLAYER_DROPRT   = 1007,
    STATION_ATK     = 2000,
    STATION_FIRERT  = 2001,
    STATION_PRJNUM  = 2002,
    STATION_REPAIR  = 2003,
    STATION_PICKUP  = 2004,
};

public enum ETB_TUTORIAL : ushort
{
    NONE                   = 0,
    BATTLE_START_MODE_MAIN = 1,
    BATTLE_DROP_EXP        = 11,
    BATTLE_DROP_METAL      = 12,
    BATTLE_UPGRADE_UNIT    = 21,
    BATTLE_UPGRADE_BASE    = 22,
    BATTLE_DIE             = 31,
    BATTLE_MOVE_FAR        = 41,
    BATTLE_TIME_PASS       = 51,
    BATTLE_HIT             = 61,
    LOBBY_UPGRADE          = 101,
    LOBBY_RANKING          = 102,
};

