using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Combat_Chase : BT_Action
{
    private GameObject m_object;
    private float m_moveSpeed;
    private Rigidbody m_rigid = null;

    public AI_Combat_Chase(GameObject _object, float _moveSpeed)
    {
        m_object = _object;
        m_moveSpeed = _moveSpeed;
        m_rigid = _object.GetComponent<Rigidbody>();
    }

    public override void Initialize()
    {

    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        OnChase();
        return AI.State.RUNNING;
    }

    private void OnChase()
    {
        // TODO : -박- 부탁하나 하자고 하면 Player를 제발 Find해서 실시간으로 찾지말아줘
        GameObject player = GameObject.Find("Player");
        if (player)
        {
            Vector3 Dir = player.transform.position - m_object.transform.position;
            Dir = new Vector3(Dir.x, 0, Dir.z);

            m_object.transform.rotation = Quaternion.Slerp
                (m_object.transform.rotation,
                Quaternion.LookRotation(Dir),
                Time.deltaTime * 4);

            m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);
            //m_rigid.AddForce(Vector3.forward * m_moveSpeed);
        }
    }
}
