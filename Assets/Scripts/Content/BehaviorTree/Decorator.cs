using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : BehaviorNode
{
	// �ൿ������ ���ڷ����ʹ� �ڽ� ��带 �ϳ��� ���� �� �ִ�.
	protected BehaviorNode m_child = null;

	public Decorator(BehaviorNode p_child) : base()
	{
		m_child = p_child;
	}

	public override BehaviorStatus Update() => BehaviorStatus.Invaild;
}
