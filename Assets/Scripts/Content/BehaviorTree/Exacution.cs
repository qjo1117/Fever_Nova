using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exacution : BehaviorNode
{
	protected BehaviorTree	m_tree = null;
	protected Transform m_transform = null;

	// ������, �Ʒ��� Update�� �ൿ�� ���鶧 �� ��������. 
    public Exacution(BehaviorTree p_tree)
	{
		m_tree = p_tree;
		m_transform = p_tree.transform;
	}

	public override BehaviorStatus Update() => BehaviorStatus.Invaild;
}
