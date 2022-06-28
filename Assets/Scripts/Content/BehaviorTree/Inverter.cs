using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Decorator
{
	public Inverter(BehaviorNode p_child) : base(p_child) { }

	public override BehaviorStatus Update()
	{
		// ������ ���� ���� / ������ ���� ����
		m_status = base.Update() == BehaviorStatus.Failure ? BehaviorStatus.Success : BehaviorStatus.Failure;
		return m_status;
	}
}
