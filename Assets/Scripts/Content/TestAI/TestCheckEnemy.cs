using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheckEnemy : SkillNode 
{
    private Transform m_transform = null;
    private SkillTree m_skillTree = null;
    private float m_range = 0.0f;


    public TestCheckEnemy(Transform p_transform)
    {
        m_transform = p_transform;
        m_skillTree = p_transform.GetComponent<SkillTree>();

        m_range = (float)m_skillTree.GetData("CheckRange");
        if((object)m_range == null) {
            m_skillTree.SetData("CheckRange", 2.0f);
		}
    }

    public override SkillNodeState Evaluate()
    {
        // Ÿ���� �ִ��� üũ�Ѵ�.
        object target = GetData("Target");
        if (target == null) {
            // ������ Player�� ��ȸ�ؼ� üũ�Ѵ�.
            foreach(PlayerController player in Managers.Game.Player.List) {
                // �÷��̾� �Ÿ��� ���� �Ÿ��� ����
                Vector3 dist = player.transform.position - m_transform.position;

                // 
                if(dist.sqrMagnitude <= m_range * m_range) {
                    m_parent.SetData("Target", player);
                    m_state = SkillNodeState.SUCCESS;
                    return m_state;
                }
            }
            
            m_state = SkillNodeState.FAILURE;
            return m_state;
        }

        m_state = SkillNodeState.SUCCESS;
        return m_state;
    }

}