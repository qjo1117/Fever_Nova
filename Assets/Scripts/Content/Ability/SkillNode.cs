using System.Collections;
using System.Collections.Generic;

public enum SkillNodeState {
    RUNNING,
    SUCCESS,
    FAILURE
}

public class SkillNode {
    protected SkillNodeState m_state = SkillNodeState.FAILURE;          // �������� �������

    protected SkillNode m_parent = null;                                // �θ� ��带 �˷��ش�.
    protected List<SkillNode> m_listChildren = new List<SkillNode>();   // �ڽ� ��带 �˷��ش�.

    private Dictionary<string, object> m_dicDataContext = new Dictionary<string, object>();     // ������ �ִ� ������ ���ο뵵�ε� SkillTree�ʿ��� ����ص��ȴ�.

    public SkillNode()
    {
        m_parent = null;
    }

    // ��ų ��帮��Ʈ�� ������ ������ ���ش�.
    public SkillNode(List<SkillNode> children)
    {
        foreach (SkillNode child in children) {
            Attach(child);
        }
    }

    // �������ش�.
    private void Attach(SkillNode p_node)
    {
        p_node.m_parent = this;
        m_listChildren.Add(p_node);
    }

    // �⺻ ���¸� ������ ���з� ������.
    public virtual SkillNodeState Evaluate()
	{
        return SkillNodeState.FAILURE;
	}

    // �����͸� �ֽ��ϴ�.
    public void SetData(string p_type, object p_value)
    {
        if(m_dicDataContext.ContainsKey(p_type) == true) {
            m_dicDataContext[p_type] = p_value;
        }
        else {
            m_dicDataContext.Add(p_type, p_value);
		}
    }

    // �����͸� �����ɴϴ�.
    public object GetData(string p_type)
    {
        object value = null;
        if (m_dicDataContext.TryGetValue(p_type, out value)) {
            return value;
        }

        // �����͸� ��ȸ�ؼ� �����Ͱ� �ִ��� üũ�Ѵ�.
        SkillNode node = m_parent;
        while (node != null) {
            value = node.GetData(p_type);
            if (value != null)
                return value;
            node = node.m_parent;
        }
        return null;
    }

    // ������ ������ �ִ� �����͸� �����մϴ�.
    public bool ClearData(string p_type)
    {
        if (m_dicDataContext.ContainsKey(p_type)) {
            m_dicDataContext.Remove(p_type);
            return true;
        }

        SkillNode node = m_parent;
        while (node != null) {
            // ������ �����͸� �����մϴ�.
            bool cleared = node.ClearData(p_type);
            if (cleared)
                return true;
            node = node.m_parent;
        }
        return false;
    }
}

