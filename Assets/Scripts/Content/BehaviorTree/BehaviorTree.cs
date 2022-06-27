using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private Dictionary<string, object>  m_dicData = new Dictionary<string, object>();
    private BehaviorNode                m_root = null;

    protected void Start()
    {
        m_root = SetupTree();
    }

    protected void Update()
    {
        if (m_root != null) {
            m_root.Update();
        }
    }

    // �߻� Ŭ����
    protected abstract BehaviorNode SetupTree();

    public void SetData(string p_key, object p_data)
	{
        // ���� ������ ��ϵǾ������� �� ���� �����Ѵ�.
        if (m_dicData.ContainsKey(p_key) == true){
            m_dicData[p_key] = p_data;
        }
        // ��� �ȵǾ��ٸ� �߰�
        else {
            m_dicData.Add(p_key, p_data);
        }
    }

    public object GetData(string p_key)
	{
        // ������ ��ϵǾ� ������ ��ȯ
        object data = null;
        if(m_dicData.TryGetValue(p_key, out data) == true) {
            return data;
        }

        // �ƴϸ� Null��ȯ
        return null;
	}

    public T GetData<T>(string p_key)
    {
        return (T)GetData(p_key);
    }

}
