using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------------------------------------------ 
 *          �����ʿ��� �ʿ��� ��
 * 
 * 
------------------------------------------ */

// ������ �༮���� ������ ��� �ִ´�.
public struct SpawnerInfo 
{
    public int      Index;            // ���� ���� �ε���
    public Vector3  Position;         // ������ ��ġ (��ȹ���ʿ��� �����ϱ�� �ؼ�)
}

public class Spawner : MonoBehaviour 
{
    // --------- ��ȹ�� ���� UI ---------
    public Transform                            m_parentMonsterPoint = null; 

    // --------- Spawn�� ���õ� ���� ---------
    [SerializeField]
    private List<SpawnerInfo>                   m_listSpawnerInfo = new List<SpawnerInfo>();
	[SerializeField]
    private SpawnerTrigger                      m_trigger = null;
    [SerializeField]
    private Transform                           m_monsters = null;

    public void Start()
    {
        // Ʈ���Ű� ���� ��� �����غ���.
        if (m_trigger.gameObject.IsValid() == false) {
            m_trigger = Util.FindChild<SpawnerTrigger>(gameObject);
        }
        m_trigger?.SetSpawner(this);

        // ���� ��� ����
        if (m_monsters == null) {
            m_monsters = Util.FindChild<Transform>(gameObject, "Monsters");
        }

        RegisterMonster();
    }

    public void Update()
    {

        
    }

    // �� 3���� ���ļ� ����ȴ�.
    // 1. Parent�� �����ؼ� Spawner ������Ʈ�� ������ ��
    // 2. SpawnerPoint���� ������ �ʿ��� ������ �����Ѵ�. => SpawnInfo
    // 3. UI������ �����ִ� Transform�� ��� �����Ѵ�.
    // ���� : ���� ������ ������ Trasnform => Vec���� �ٲ���Ѵ�.
	public void TransformToPosition()
	{
        List<MonsterSpawnPoint> l_listMonsterPoint = new List<MonsterSpawnPoint>();
        // ���� Transform�� �ִ� SpawnerPoint�� �����´�.
        int l_size = m_parentMonsterPoint.childCount;
        for (int i = 0; i < l_size; ++i) {
            MonsterSpawnPoint l_spawnInfo = null;
            if(m_parentMonsterPoint.GetChild(i).TryGetComponent(out l_spawnInfo) == true) {
                l_listMonsterPoint.Add(l_spawnInfo);
            }
        }

        // ������ ���� ���� ����
        l_size = l_listMonsterPoint.Count;
        for (int i = 0; i < l_size; ++i) {
            m_listSpawnerInfo.Add(new SpawnerInfo { Index =  l_listMonsterPoint[i].m_index, Position =  l_listMonsterPoint[i].transform.position });
        }

        // Transform�� ǥ���ϴ� ������Ʈ�� ���� ������Ų��.
        l_size = m_parentMonsterPoint.childCount;
        for (int i = 0; i < l_size; ++i) {
            Managers.Resource.DelPrefab(m_parentMonsterPoint.GetChild(i).gameObject);
        }
        Managers.Resource.DelPrefab(m_parentMonsterPoint.gameObject);

        // �ʿ䰡 �����Ƿ� �����Ѵ�.
        l_listMonsterPoint.Clear();
    }
    

    // �÷��̾ Ʈ���ſ� ���������� ������ �����ϸ� ���͸� �����Ѵ�.
	public void StartSpawn()
	{
        // ������ �ƿ� ������ ���
        if(m_listSpawnerInfo.Count < 0) {
            return;
		}

        // �ش� �ε����� �°� ������ �����Ѵ�.
        int l_size = m_listSpawnerInfo.Count;
        for(int i = 0; i < l_size; ++i) {
			var l_monster = Managers.Game.Monster.Spawn(m_listSpawnerInfo[i].Index);
            l_monster.transform.position = m_listSpawnerInfo[i].Position;
            l_monster.Init();
        }

        l_size = m_monsters.transform.childCount;
        for (int i = 0; i < l_size; ++i) {
            BehaviorTree l_monster = null;
            if (m_monsters.GetChild(i).TryGetComponent(out l_monster) == true) {
                Managers.Game.Monster.Register(l_monster);
            }
            l_monster.Init();
        }


        // TODO : �ش� UI�� �����ؾ��ϴ� ��
        // ��ǥ ����
    }


    private void RegisterMonster()
	{
        int l_size = m_monsters.childCount;
        for (int i = 0; i < l_size; ++i){
            m_monsters.GetChild(i).gameObject.SetActive(false);
        }
    }


}
