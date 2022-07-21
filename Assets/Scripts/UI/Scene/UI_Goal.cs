using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Goal : UI_Scene
{
    enum Images
    {
        Background
    }

    enum Texts
    {
        GoalText
    }


    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
    }

    // _allMonsterCount => �� ���������� ������ ������ �� ����
    public void GoalTextUpdate(int _allMonsterCount, int _monsterKillCount)
    {
        Get<Text>((int)Texts.GoalText).text = $"���� ����\n{_allMonsterCount - _monsterKillCount} / {_allMonsterCount}";
    }

}
