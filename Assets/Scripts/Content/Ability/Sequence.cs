using System.Collections.Generic;

public class Sequence : SkillNode {
    public Sequence() : base() { }
    public Sequence(List<SkillNode> p_children) : base(p_children) { }

    public override SkillNodeState Evaluate()
    {
        bool anyChildIsRunning = false;

        foreach (SkillNode node in m_listChildren) {
            switch (node.Evaluate()) {
                case SkillNodeState.FAILURE:                 // �����ߴٸ� failure�� �ش�.
                    m_state = SkillNodeState.FAILURE;
                    return m_state;
                case SkillNodeState.SUCCESS:                 // ���� �����ϸ� �ѱ��.
                    continue;
                case SkillNodeState.RUNNING:                 // �������̸� � ���� �������̴ٶ�� üũ�ϰ� ����
                    anyChildIsRunning = true;
                    continue;
                default:                                // ���ܰ��� ����
                    m_state = SkillNodeState.SUCCESS;
                    return m_state;
            }
        }

        // ���� �������̶�� �������̶�� ���¸� �����Ѵ�.
        m_state = anyChildIsRunning ? SkillNodeState.RUNNING : SkillNodeState.SUCCESS;
        return m_state;
    }

}