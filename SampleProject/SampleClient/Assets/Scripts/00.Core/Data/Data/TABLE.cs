using UnityEngine;


public class TABLE : Singleton<TABLE>
{
    static public UTBConfig_Setting config_setting = null;
    static public UTBArtifact artifact = null;
    static public UTBCharacter character = null;
    static public UTBEnemy enemy = null;
    static public UTBEnemySpawn enemyspawn = null;
    static public UTBEquipment equipment = null;
    static public UTBFund fund = null;
    static public UTBString_Common_Chs string_common_chs = null;
    static public UTBString_Common_Cht string_common_cht = null;
    static public UTBString_Common_En string_common_en = null;
    static public UTBString_Common_Ja string_common_ja = null;
    static public UTBString_Common_Ko string_common_ko = null;
    static public UTBTutorial tutorial = null;
    static public UTBUpgrade upgrade = null;

    public UTBConfig_Setting o_config_setting = new UTBConfig_Setting();
    public UTBArtifact o_artifact = new UTBArtifact();
    public UTBCharacter o_character = new UTBCharacter();
    public UTBEnemy o_enemy = new UTBEnemy();
    public UTBEnemySpawn o_enemyspawn = new UTBEnemySpawn();
    public UTBEquipment o_equipment = new UTBEquipment();
    public UTBFund o_fund = new UTBFund();
    public UTBString_Common_Chs o_string_common_chs = new UTBString_Common_Chs();
    public UTBString_Common_Cht o_string_common_cht = new UTBString_Common_Cht();
    public UTBString_Common_En o_string_common_en = new UTBString_Common_En();
    public UTBString_Common_Ja o_string_common_ja = new UTBString_Common_Ja();
    public UTBString_Common_Ko o_string_common_ko = new UTBString_Common_Ko();
    public UTBTutorial o_tutorial = new UTBTutorial();
    public UTBUpgrade o_upgrade = new UTBUpgrade();

    public void LoadAll(bool InForceResources)
    {
        byte[] buffer = new byte[4096];

        o_config_setting.Load(buffer, InForceResources);
        o_artifact.Load(buffer, InForceResources);
        o_character.Load(buffer, InForceResources);
        o_enemy.Load(buffer, InForceResources);
        o_enemyspawn.Load(buffer, InForceResources);
        o_equipment.Load(buffer, InForceResources);
        o_fund.Load(buffer, InForceResources);
        o_string_common_chs.Load(buffer, InForceResources);
        o_string_common_cht.Load(buffer, InForceResources);
        o_string_common_en.Load(buffer, InForceResources);
        o_string_common_ja.Load(buffer, InForceResources);
        o_string_common_ko.Load(buffer, InForceResources);
        o_tutorial.Load(buffer, InForceResources);
        o_upgrade.Load(buffer, InForceResources);

        RestoreStaticModule();
    }

    public override void RestoreStaticModule()
    {
        base.RestoreStaticModule();

        config_setting = o_config_setting;
        artifact = o_artifact;
        character = o_character;
        enemy = o_enemy;
        enemyspawn = o_enemyspawn;
        equipment = o_equipment;
        fund = o_fund;
        string_common_chs = o_string_common_chs;
        string_common_cht = o_string_common_cht;
        string_common_en = o_string_common_en;
        string_common_ja = o_string_common_ja;
        string_common_ko = o_string_common_ko;
        tutorial = o_tutorial;
        upgrade = o_upgrade;
    }

    public int GetLastVersion(string InTableName)
    {
        string tableName = InTableName.ToLower();

        int storageType = 0;

        switch (tableName)
        {
            case "config_setting": return config_setting.GetLastVersion(out storageType);
            case "artifact": return artifact.GetLastVersion(out storageType);
            case "character": return character.GetLastVersion(out storageType);
            case "enemy": return enemy.GetLastVersion(out storageType);
            case "enemyspawn": return enemyspawn.GetLastVersion(out storageType);
            case "equipment": return equipment.GetLastVersion(out storageType);
            case "fund": return fund.GetLastVersion(out storageType);
            case "string_common_chs": return string_common_chs.GetLastVersion(out storageType);
            case "string_common_cht": return string_common_cht.GetLastVersion(out storageType);
            case "string_common_en": return string_common_en.GetLastVersion(out storageType);
            case "string_common_ja": return string_common_ja.GetLastVersion(out storageType);
            case "string_common_ko": return string_common_ko.GetLastVersion(out storageType);
            case "tutorial": return tutorial.GetLastVersion(out storageType);
            case "upgrade": return upgrade.GetLastVersion(out storageType);
        }

        return 0;
    }
};
