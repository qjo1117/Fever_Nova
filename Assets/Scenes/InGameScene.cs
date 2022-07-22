using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{

    protected override void LoadGameObject()
	{
        Managers.Resource.RegisterPoolGameObject("Boom");
        Managers.Resource.RegisterPoolGameObject("Monster");
        Managers.Resource.RegisterPoolGameObject("Asset/Prefabs/Player");

        Managers.Resource.RegisterPoolGameObject("UI/Popup/UI_PopupMsg");

        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Aim");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombRange");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombJumpRange");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombJumpArrow");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombDropPoint");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_MonsterHPBar");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BossMonsterHPBar");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_PlayerHPBar");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Goal");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Score");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Pause");
    }

	protected override void Init()
	{
        Managers.Game.InGameInit();

        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);

        // boss ���� ���� �׽�Ʈ��
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform).GetComponent<MonsterController>().IsBoss = true;

        UIInit();
    }

    private void UIInit()
    {
        UI_PopupMsg l_popupMsg = Managers.UI.ShowPopupUI<UI_PopupMsg>("UI_PopupMsg");

        Managers.UI.ShowSceneUI<UI_Aim>("UI_Aim");

        Managers.UI.ShowSceneUI<UI_Goal>("UI_Goal");
        // ���� ui ����
        Managers.UI.ShowSceneUI<UI_Score>("UI_Score");
        Managers.UI.ShowSceneUI<UI_Pause>("UI_Pause");

        // �÷��̾� ��ź ��Ÿ� ����
        UI_BombRange l_bombRange = Managers.UI.ShowSceneUI<UI_BombRange>("UI_BombRange");
        l_bombRange.RangeRadius = 5.0f; // ?

        UI_BombJumpRange l_bombJumpRange = Managers.UI.ShowSceneUI<UI_BombJumpRange>("UI_BombJumpRange");
        l_bombJumpRange.RangeRadius = 2.0f;

        // ��ź���� ui ���� 
        UI_BombDropPoint l_dropPoint = Managers.UI.ShowSceneUI<UI_BombDropPoint>("UI_BombDropPoint");
        l_dropPoint.BombRange = l_bombRange;
        l_dropPoint.BombJumpRange = l_bombJumpRange;
    }


    public override void Clear()
    {

        Managers.Log("InGame Clear");
    }
}
