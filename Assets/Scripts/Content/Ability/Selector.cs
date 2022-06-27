using System.Collections.Generic;

public class Selector : SkillNode {
    public Selector() : base()  { }

    public Selector(List<SkillNode> p_children) : base(p_children) { }

    public override SkillNodeState Evaluate()
    {
        // ��带 ��ȸ�ؼ� ���� Ȯ���մϴ�.
        foreach (SkillNode node in m_listChildren) {
            switch (node.Evaluate()) {
                case SkillNodeState.FAILURE:            // ������ ��� ���� ��츦 Ȯ��
                    continue;
                case SkillNodeState.SUCCESS:            // ������ ��� ������ ����
                    m_state = SkillNodeState.SUCCESS;
                    return m_state;
                case SkillNodeState.RUNNING:            // �������̸� �������̶�� �˷��ش�.
                    m_state = SkillNodeState.RUNNING;
                    return m_state;
                default:                                // �̻��� ���� �ѱ��.
                    continue;
            }
        }

        // ��� ���������� ���ж�� �˷��ش�.
        m_state = SkillNodeState.FAILURE;
        return m_state;
    }

}

