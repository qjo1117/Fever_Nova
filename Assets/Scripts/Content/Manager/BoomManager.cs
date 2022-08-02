using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ŵ����� ���� �� ���� : ����
// ��Ŷ ��� ��������

public class BoomManager : MonoBehaviour
{
    private List<Boom> m_listBoom = new List<Boom>();

	[SerializeField]
    private float m_speed = 600;                         // 
	[SerializeField]
    private float m_explosionMaxDelayTime = 5.0f;           // �ִ� �����ð�
	[SerializeField]
    private float m_explosionForce = 1000.0f;           //  �Ϲ����� ��
	[SerializeField]
	private float m_jumpForce = 3000.0f;
	[SerializeField]
	private float m_ratio = 10.0f;

    public Boom ShootSpawn(Vector3 _position, Vector3 _direction, float _dist)
	{
		Boom l_boom = Managers.Resource.Instantiate("Boom", transform).GetComponent<Boom>();
		
		// �⺻���� ���� ����
		l_boom.Speed = m_speed;
		l_boom.MaxDelayTime = m_explosionMaxDelayTime;
		l_boom.ExplosionForce = m_explosionForce;

		// ��ź�� ������.
		l_boom.Shoot(_position, _direction, _dist / m_ratio);
		m_listBoom.Add(l_boom);

		// TODO : �׽�Ʈ
		Managers.UI.Root.GetComponentInChildren<UI_BombRange>().RangeRadius = _dist;

		return l_boom;
	}

	public Boom JumpSpawn(Vector3 _position)
	{
		Boom l_boom = Managers.Resource.Instantiate("Boom", transform).GetComponent<Boom>();

		// �⺻���� ���� ����
		l_boom.Speed = m_speed;
		l_boom.ExplosionForce = m_jumpForce;

		// ��ź�� ������.
		l_boom.JumpShoot(_position);
		m_listBoom.Add(l_boom);

		return l_boom;
	}

	public void DeSpawn(Boom _boom)
	{
		m_listBoom.Remove(_boom);

		// ��ƼŬ
		GameObject particle = Managers.Resource.Instantiate(Path.Boom_Particle, transform);
		particle.transform.position = _boom.transform.position;
		Managers.Resource.Destroy(particle, 7.0f);

		Managers.Resource.Destroy(_boom.gameObject);
	}
}
