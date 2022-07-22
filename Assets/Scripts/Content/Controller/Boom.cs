using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class Boom : MonoBehaviour
{
    public enum BoomState
	{
        Default,
        Delay,
        Jump,
	}

	[SerializeField]
    private BoomState           m_state = BoomState.Default;

    [SerializeField]
    private float               m_explosionForce = 1000.0f;

    [SerializeField]
    private float               m_moveSpeed = 10000.0f;               // ��ź�� �������� ��
    [SerializeField]
    private float               m_explosionRange = 5.0f;            // ���� �ݰ�
    [SerializeField]
    private float               m_detectRange = 2.0f;               // ���� �ݰ�

    private float               m_explosionDelayTime = 0.0f;        // ���� ���� �ð�
    [SerializeField]
    private float               m_explosionMaxDelayTime = 5.0f;     // �ִ� ���� �ð�
    private bool                m_isExplosion = false;

    private PlayerController    m_player = null;
    private Rigidbody           m_rigid = null;

    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    private bool                m_isDelayState = false;

    private bool                m_isGround = false;
    private Vector3             m_groundPoint = Vector3.zero;
    private Vector3             m_reflectionNormal = Vector3.zero;


    #region ������Ƽ

    public BoomState State { get => m_state; set => m_state = value; }

    #endregion

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckExplosionTime();
    }

    // �߻縦 ������ �ʿ��� ������ ������
    public void Shoot(Vector3 _position, Vector3 _direction)
	{
        transform.position = _position;
        m_rigid.AddForce(_direction * m_moveSpeed);
	}

    public void Jump(Vector3 _position)
	{
        transform.position = _position;
        m_rigid.AddForce(-Vector3.up * m_moveSpeed);
        m_state = BoomState.Jump;
    }

    public void Explosion()
    {
        RaycastHit[] l_colliders =  Physics.SphereCastAll(transform.position, m_explosionRange, Vector3.up, 1.0f, m_layer);

        // ��ȸ�� �Ͽ� üũ�� ����
        foreach(RaycastHit l_hit in l_colliders) {
            Transform l_trans = l_hit.transform;

            // ���� ���Ϳ� ��ź���� �Ÿ��� �˾Ƴ���.
            Vector3 l_subVec = l_trans.position - transform.position;
            l_subVec.y = 0;           // 2D�ν��� �Ÿ��� ����Ѵ�.
            l_subVec /= l_hit.collider.GetComponent<Rigidbody>().mass;         // ������ ������ �����ؼ� ����Ѵ�.
            l_subVec *= l_subVec.magnitude;             // �Ÿ��� �� ���� �� ũ�� ���� �޴´�.

            // TODO : ����� ��Ʈ ���� �����ϸ� ��
            int l_layer = l_hit.collider.gameObject.layer;
            if (l_layer == (int)Define.Layer.Player) {
                PlayerController l_player = Managers.Game.Player.FindPlayer(l_hit.collider.gameObject.GetInstanceID());
                l_player.GetComponent<Rigidbody>().AddForce(m_explosionForce * l_subVec);

            }
            else if (l_layer == (int)Define.Layer.Monster) {
                BehaviorTree l_monster = l_hit.collider.GetComponent<BehaviorTree>();
            }
        }

        m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;

        // ��ƼŬ
        GameObject particle = Managers.Resource.Instantiate(Path.Boom_Particle, Managers.Game.Boom.transform);
        particle.transform.position = transform.position;
        Managers.Resource.Destroy(particle, 7.0f);

        Managers.Resource.Destroy(gameObject);
    }


    // ���� ������ �� �������� ī�����Ѵ�.
    private void CheckExplosionTime()
	{
        // ���� �ð��� �����ϸ� �߰��Ѵ�.
        m_explosionDelayTime += Time.deltaTime;

        // ���������� ��� üũ ��ü�� ���Ѵ�.
        if (m_state == BoomState.Delay || m_explosionDelayTime < m_explosionMaxDelayTime) {
            return;
		}

        // ��� �ð��� �ٳ����� ����
        Explosion();
    }

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.gameObject.layer != (int)Define.Layer.Monster) {
            return;
		}

        Explosion();
	}

	private void OnDrawGizmos()
	{

    }
}
