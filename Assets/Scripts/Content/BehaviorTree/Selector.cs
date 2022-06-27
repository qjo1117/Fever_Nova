using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite 
{
	public Selector() : base() { }
	public Selector(List<BehaviorNode> p_listChild) : base(p_listChild) { }

	public override BehaviorStatus Update()
	{
		// ������ ���ٸ� �������� ���ְ� ���� ��忡�� ������ �ѱ��.
		if (EmptyChildren() == true) {
			return BehaviorStatus.Success;
		}

		int size = m_listChildren.Count;

		for (int i = 0; i < size; ++i) {
			BehaviorStatus state = m_listChildren[i].Update();

			// Selector�̶� ������� ��ȸ�� ����or���а� ������ ��� ������ �����ϰ� ����
			switch(state) {
				case BehaviorStatus.Success:
					m_status = BehaviorStatus.Success;
					return m_status;
				case BehaviorStatus.Running:
					m_status = BehaviorStatus.Running;
					return m_status;
				case BehaviorStatus.Failure:
					continue;
			}
		}

		// ���� �Ѿ���� ����
		m_status = BehaviorStatus.Failure;
		return m_status;
	}
}


