using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHPBar : UI_WorldSpaceHPBar
{
    public override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        // �θ�Ŭ������ Update �� !ȣ�����־����
        // ü���ǹ��� ���� ��ġ ������Ʈ �Լ��� �θ� Update�Լ��� �����ϱ� ����
        base.Update();

        // ���� hp�� �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (m_hp > 0)
            {
                HP = m_hp - 10;
            }

        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (m_hp < m_maxHp)
            {
                HP = m_hp + 10;
            }
        }
    }
}
