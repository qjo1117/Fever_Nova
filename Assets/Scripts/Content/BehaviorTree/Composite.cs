using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : BehaviorNode
{
	protected List<BehaviorNode> m_listChildren = new List<BehaviorNode>();
	protected int m_index = 0;

	public Composite() : base() { }
	public Composite(List<BehaviorNode> p_listChild) 
	{ 
		foreach(BehaviorNode child in p_listChild) {
			Attach(child);
		}
	}

	// ������ ������ �����
	public bool EmptyChildren()
	{
		return m_listChildren.Count == 0;
	}

	// ����
	public void Attach(BehaviorNode p_node)
	{
		m_listChildren.Add(p_node);
	}
	
	// ���� ����
	public void UnAttach(BehaviorNode p_node)
	{
		m_listChildren.Remove(p_node);
	}

	// ���� �߻�ܰ��
	public override BehaviorStatus Update() => BehaviorStatus.Invaild;

}
