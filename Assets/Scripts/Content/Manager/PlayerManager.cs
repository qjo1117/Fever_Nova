using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerController>  m_listPlayers = new List<PlayerController>();
    private PlayerController        m_mainPlayer = null;

    // �÷��̾� ü�¹� ������Ʈ �Լ� ȣ��� ���
    private UI_PlayerHPBar m_playerHPBar = null;

    // �̸� ��õ ����
    public List<PlayerController>   List { get => m_listPlayers; }

    public PlayerController MainPlayer { get => m_mainPlayer; }


    void Update()
    {
        // �÷��̾� ü�¹� �׽�Ʈ�� ������ �ֱ�
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            m_mainPlayer.Demege(10);
            m_playerHPBar.HpBarUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            m_mainPlayer.Recover(10);
            m_playerHPBar.HpBarUpdate();
        }
    }

    // �ʱ�ȭ �۾��� �Ѵ�.
    public void Init()
    {
        PlayerController l_player;
        l_player = FindObjectOfType<PlayerController>();
        m_mainPlayer = l_player;

        // �÷��̾� hp �� ����
        m_playerHPBar = Managers.UI.ShowSceneUI<UI_PlayerHPBar>("UI_PlayerHPBar");
        Managers.UI.SetCanvas(m_playerHPBar.gameObject, false);

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
