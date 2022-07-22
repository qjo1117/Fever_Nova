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

    private int m_allMonsterCount = 0;
    private int m_monsterKillCount = 0;

    public int AllMonsterCount
    {
        get 
        { 
            return m_allMonsterCount; 
        }
        set
        {
            m_allMonsterCount = value;
            UpdateGoalText();
        }
    }

    public int MonsterKillCount
    {
        get
        {
            return m_monsterKillCount;
        }
        set
        {
            m_monsterKillCount = value;
            UpdateGoalText();
        }
    }


    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        Managers.Game.Monster.RemainCount++;
        Managers.Game.Monster.RemainCount++;
        Managers.Game.Monster.RemainCount++;
    }

    // _allMonsterCount => �� ���������� ������ ������ �� ����
    private void UpdateGoalText()
    {
        Get<Text>((int)Texts.GoalText).text = $"���� ����\n{m_allMonsterCount - m_monsterKillCount} / {m_allMonsterCount}";
    }

}
