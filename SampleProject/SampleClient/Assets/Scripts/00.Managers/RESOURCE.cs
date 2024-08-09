using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class RESOURCE : Singleton<RESOURCE>
{
    // Atlas
    private readonly List<(string, SpriteAtlas)> _AtlasList = new();

    // Texture
    private readonly List<string> _TexturePathList = new();

    // SPUM Unit
    private readonly string _SpumPath = "Unit/{0}";
    private readonly Dictionary<int, SPUM_Prefabs> _SpumDic = new();

    public override void Init()
    {
        // 순서에 주의 하시오 : EUI_AtlasType
        // COMMON = 0,
        // UI = 1,
        // CHARACTER = 2,
        // EQUIPMENT = 3,
        // ITEM = 4,
        _AtlasList.Add(("", Resources.Load<SpriteAtlas>("Atlas/atlas_common")));
		_AtlasList.Add(("", Resources.Load<SpriteAtlas>("Atlas/atlas_ui")));
        _AtlasList.Add(("", Resources.Load<SpriteAtlas>("Atlas/atlas_character")));
        _AtlasList.Add(("", Resources.Load<SpriteAtlas>("Atlas/atlas_equipment")));
        _AtlasList.Add(("", Resources.Load<SpriteAtlas>("Atlas/atlas_item")));

		// 순서에 주의 하시오 : EUI_TextureType
		// BATTLE = 0,
		_TexturePathList.Add("Texture/battle/{0}");
    }

    public Sprite GetSprite(EUI_AtlasType InAtlasType, string InName)
    {
        var info = _AtlasList[(int)InAtlasType];
		return info.Item2.GetSprite($"{info.Item1}{InName}");
    }

    public Sprite GetSprite(EUI_AtlasType InAtlasType, int InID)
    {
		var info = _AtlasList[(int)InAtlasType];
		return info.Item2.GetSprite($"{info.Item1}{InID}");
    }

    public Sprite GetTexture(EUI_TextureType InTextureType, string InName)
    {
        return Resources.Load<Sprite>(string.Format(_TexturePathList[(int)InTextureType], InName));
    }

    public Sprite GetTexture(EUI_TextureType InTextureType, int InID)
    {
        return Resources.Load<Sprite>(string.Format(_TexturePathList[(int)InTextureType], InID));
    }

    public SPUM_Prefabs GetSpum(int InID)
    {
        if (_SpumDic.TryGetValue(InID, out SPUM_Prefabs spum) == false)
        {
            spum = Resources.Load<SPUM_Prefabs>(string.Format(_SpumPath, InID));

            _SpumDic.Add(InID, spum);
        }

        return spum;
	}

	public T GetObject<T>(string InPath) where T : UnityEngine.Object
    {
		return Resources.Load<T>(InPath);
	}
}