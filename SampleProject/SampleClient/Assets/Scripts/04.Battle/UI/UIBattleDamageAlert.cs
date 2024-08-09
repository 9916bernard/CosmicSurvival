using UnityEngine;
using UnityEngine.UI;

public class UIBattleDamageAlert : UIBase
{
    //[Header("[ Bind Property ]")]
    //[SerializeField] private Color _ColorDamage = Color.white;

    protected override void OnOpenStart()
    {
        PlayTween();
    }
}
