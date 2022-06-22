using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerController> m_listPlayers = new List<PlayerController>();
    private PlayerController m_mainPlayer = null;

    void Update()
    {

    }

    // �ʱ�ȭ �۾��� �Ѵ�.
    public void Init()
    {
        // TODO : ������ ��� �÷��̾������� Ȯ���ؼ� �߰�����.

        // �ش��ϴ� �÷��̾� �������� �����Ѵ�.
        // ���� �������� ������ ��� Pool����ϸ� �ȴ�.
        PlayerController player = Managers.Resource.NewPrefab("Player", transform).GetComponent<PlayerController>();

        // ����Ʈ�� �־��ְ� ó�� ������ �༮�� �������� ��Ͻ�Ų��.
        m_listPlayers.Add(player);
        m_mainPlayer = player;

        // ���̵� �߱��� ���ش�.

        // ��ȸ�ϸ鼭 �ʱ�ȭ �۾��� ���ش�.
        int size = m_listPlayers.Count;
        for (int i = 0; i < size; ++i) {
            // ��¦ �����ϴ°� �����������ѵ� �ϴ� �Ǻ��� �̷���
            if(m_mainPlayer == m_listPlayers[i]) {
                m_listPlayers[i].Init(i, true);
            }
            else {
                m_listPlayers[i].Init(i, false);
            }
        }
    }

    public void Clear()
	{
        // ���� ���̶�Ű�� �÷��̾ ������ ������ ���� ���ѹ�����.
        foreach(PlayerController player in m_listPlayers) {
            Managers.Resource.DelPrefab(player.gameObject);
		}
        // �ʱ�ȭ
        m_listPlayers.Clear();

    }




}
