using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerController>  m_listPlayers = new List<PlayerController>();
    private PlayerController        m_mainPlayer = null;

    // �̸� ��õ ����
    public List<PlayerController>   List { get => m_listPlayers; }


    void Update()
    {

    }

    // �ʱ�ȭ �۾��� �Ѵ�.
    public void Init()
    {
        //// TODO : ������ ��� �÷��̾������� Ȯ���ؼ� �߰�����.

        //// �ش��ϴ� �÷��̾� �������� �����Ѵ�.
        //// ���� �������� ������ ��� Pool����ϸ� �ȴ�.
        //PlayerController player = Managers.Resource.NewPrefab("Player", transform).GetComponent<PlayerController>();

        //// ����Ʈ�� �־��ְ� ó�� ������ �༮�� �������� ��Ͻ�Ų��.
        //m_listPlayers.Add(player);
        //m_mainPlayer = player;

        //// ���̵� �߱��� ���ش�.

        //// ��ȸ�ϸ鼭 �ʱ�ȭ �۾��� ���ش�.
        //int size = m_listPlayers.Count;
        //for (int i = 0; i < size; ++i) {
        //    // ��¦ �����ϴ°� �����������ѵ� �ϴ� �Ǻ��� �̷���
        //    if(m_mainPlayer == m_listPlayers[i]) {
        //        m_listPlayers[i].Init(i, true);
        //    }
        //    else {
        //        m_listPlayers[i].Init(i, false);
        //    }
        //}
    }

    public void Clear()
	{
  //      // ���� ���̶�Ű�� �÷��̾ ������ ������ ���� ���ѹ�����.
  //      foreach(PlayerController player in m_listPlayers) {
  //          Managers.Resource.DelPrefab(player.gameObject);
		//}
  //      // �ʱ�ȭ
  //      m_listPlayers.Clear();

    }

    // ������� ������ ���δ�.
    public void Demege(int p_id, int p_attack, Vector3 p_force)
	{
        //m_listPlayers[p_id].Demege(p_attack);
        //m_listPlayers[p_id].Rigid.AddForce(p_force);
    }

    public void Demege(PlayerController p_player, int p_attack, Vector3 p_force)
	{
        //// ���� �ִ� �Լ� ȣ��
        //Demege(p_player.ID, p_attack, p_force);
    }



}
