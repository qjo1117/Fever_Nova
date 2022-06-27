using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Decorator
{
	public Inverter(BehaviorNode p_child) : base(p_child) { }

	public override BehaviorStatus Update()
	{
		BehaviorStatus status = m_child.Update();

		// ��������� ����� Not�����Ѵ�.
		if(status == BehaviorStatus.Failure) {
			m_status = BehaviorStatus.Success;
		}
		else if (status == BehaviorStatus.Success) {
			m_status = BehaviorStatus.Failure;
		}
		else {
			m_status = BehaviorStatus.Running;
		}

		return m_status;
	}
}
