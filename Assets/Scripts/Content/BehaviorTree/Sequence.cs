using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite
{
	public Sequence() : base() { }
	public Sequence(List<BehaviorNode> p_listChild) : base(p_listChild) { }


	public override BehaviorStatus Update()
	{
		// ������ ���ٸ� �������� ���ְ� ���� ��忡�� ������ �ѱ��.
		if (EmptyChildren() == true) {
			return BehaviorStatus.Success;
		}

		int size = m_listChildren.Count;

		for (int i = 0; i < size; ++i) {
			BehaviorStatus state = m_listChildren[i].Update();

			// Selector�̶� ������� ��ȸ�� ���а� ������ ��� �ٷ� �����Ѵ�.
			switch (state) {
				case BehaviorStatus.Success:
					continue;
				case BehaviorStatus.Running:
					continue;
				case BehaviorStatus.Failure:
					m_status = BehaviorStatus.Failure;
					return m_status;
			}
		}

		// ���� ���������� Selector�� ���´� �����̴�.
		m_status = BehaviorStatus.Success;
		return m_status;
	}


}
