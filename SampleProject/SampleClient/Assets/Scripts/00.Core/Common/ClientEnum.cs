
//============================================================================
// UI
//============================================================================
public enum EUI_RefreshType : int
{
    // 중간에 추가하지 마시오
    NONE = 0,
    ACCOUNT = 1,
    FUND = 2,
    PLAYER = 3,
};

public enum EUI_AreaType : int
{
    // 배열의 인덱스로 사용됩니다.
    BASE = 0,
    POPUP = 1,
    OVERLAY = 2,
    MANAGED = 3,
    MAX = 4,
};

public enum EUI_LoadType: int
{
    // 배열의 인덱스로 사용됩니다.
    COMMON = 0,
    MAIN = 1,
    RANKING = 2,
    UPGRADE = 3,
    COLLECTION = 4,
    SHOP = 5,
    SETTING = 6,
    BATTLE = 7,
    MAX = 8,
}

public enum EUI_LanguageType : int
{
    NONE = 0,                   //없음(초기 세팅)
    ENGLISH = 1,                //영어
    KOREAN = 2,                 //한국어
    JAPANESE = 3,               //일본어
    CHINESE_TRADITIONAL = 4,    //중국어 번체
    CHINESE_SIMPLIFIED = 5,     //중국어 간체
};

public enum EUI_AtlasType : int
{
    // 순서 바꾸지 마시오
    COMMON = 0,
    UI = 1,
    CHARACTER = 2,
	EQUIPMENT = 3,
    ITEM = 4,
};

public enum EUI_TextureType : int
{
    // 순서 바꾸지 마시오
    BATTLE = 0,
};

public enum EUI_SFX : int
{
    // 순서 바꾸지 마시오
    NONE = 0,
    CLICK_POSITIVE = 1,
    CLICK_NEGATIVE = 2,
    HIT_TEST = 3,
    HIT_TEST_2 = 4,
    WP_WIND = 5,
    LEVEL_UP = 6,
    GAME_START = 7,
    DEAD_TUTO = 8,
    ENEMY_DEAD = 9,
    EXP_PICKUP = 10,
    HP_PICKUP = 11,
    METAL_PICKUP = 12,
    SWORD_WP = 13,
    DAMAGE_PLAYER = 14,
    
};

public enum EUI_BGM : int
{
    // 순서 바꾸지 마시오
    NONE = 0,
    MAIN = 1,
    BATTLE = 2,
    LOBBY = 3,
};
