using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillTree : MonoBehaviour 
{
    // Ʈ���� ��Ʈ ��带 �˾ƾ��Ѵ�.
    private SkillNode m_root = null;

    // ���� ����� ������ �����ش�.
    // ������ ���� : ���߿� �ִϸ��̼�ó�� ����ϱ� ���ؼ�
    [SerializeField]
    protected Dictionary<string, object> m_dicDataContext = new Dictionary<string, object>();

    public void SetData(string p_type, object p_data)
	{
        // ���� ������ �����Ͱ� ���� ���
        if(m_dicDataContext.ContainsKey(p_type) == true) {
            m_dicDataContext[p_type] = p_data;
        }
        // ���� ������ �����Ͱ� ������� �߰��� ���ش�.
        else {
            m_dicDataContext.Add(p_type, p_data);
		}
	}

    public object GetData(string p_type)
	{
        object value = null;
        m_dicDataContext.TryGetValue(p_type, out value);
        return value;
    }

    protected void Start()
    {
        m_root = SetupTree();
    }

    private void Update()
    {
        if (m_root != null) {
            m_root.Evaluate();
        }
    }

    protected abstract SkillNode SetupTree();

}
