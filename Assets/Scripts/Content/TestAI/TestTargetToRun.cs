using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTargetToRun : SkillNode
{
	private Transform m_transform = null;		// ĳ��
	private SkillTree m_skillTree = null;		// �뵵

	private float m_moveSpeed = 0.0f;			// ���������� ȣ�⺸�ٴ� ĳ���ϴ� �� ���ƺ���
	private float m_range = 0.0f;

    public TestTargetToRun(Transform p_transform)
	{
		m_transform = p_transform;
		m_skillTree = p_transform.GetComponent<SkillTree>();

		m_moveSpeed = (float)m_skillTree.GetData("MoveSpeed");
		m_range = (float)m_skillTree.GetData("CheckRange");
	}

	public override SkillNodeState Evaluate()
	{
		PlayerController target = (PlayerController)GetData("Target");
		// ������� ����
		if (target == null || target.IsDead == true) {
			m_state = SkillNodeState.FAILURE;
			return m_state;
		}

		Vector3 pos = target.transform.position;
		Vector3 dist = pos - m_transform.position;

		if (dist.sqrMagnitude <= m_range * m_range) {
			m_state = SkillNodeState.SUCCESS;
			return m_state;
		}

		m_transform.position = Vector3.MoveTowards(m_transform.position, pos, m_moveSpeed * Time.deltaTime);
		m_transform.LookAt(pos);

		m_state = SkillNodeState.RUNNING;
		return m_state;
	}
}
