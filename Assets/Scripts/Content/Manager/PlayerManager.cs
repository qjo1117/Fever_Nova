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

    public PlayerController FindPlayer(int _id)
	{
        foreach(PlayerController l_player in m_listPlayers) {
            // ���̵� ������ �ν��Ͻ� ���̵�� �����Ѵ�.
            if(l_player.gameObject.GetInstanceID() == _id) {
                return l_player;
			}
		}

        return null;
	}

    // ������ ��Ų��.
    public PlayerController Spawn(Vector3 _position, PlayerStat _stat)
	{
        PlayerController l_player = Managers.Resource.Instantiate(Path.Player, transform).GetComponent<PlayerController>();
        l_player.transform.position = _position;        // ��ǥ �ݿ�
        l_player.Stat.id = m_listPlayers.Count;         // ���̵� �߱�
        l_player.Stat = _stat;                          // ���� �ݿ�
        m_listPlayers.Add(l_player);                    // �Ŵ��� �߰�

        return l_player;
    }

    // �ʱ�ȭ �۾��� �Ѵ�.
    public void Init()
    {
        // ������ ������ �����Ѵ�.

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
