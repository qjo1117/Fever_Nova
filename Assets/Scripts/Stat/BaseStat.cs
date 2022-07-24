using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseStat
{
    public int      Id = -1;
    public string   Name = "Unkown";
    public int      Hp = 100;
    public int      MaxHp = 100;
    public float    MoveSpeed = 10.0f;
    public float    Mass = 1.0f;
}

[System.Serializable]
public class MonsterStat : BaseStat 
{
    public int      Score = 1;
    public int      TreeId = 0;
    public string   Path = "";
}

public class MonsterStatTable {
    public Dictionary<int, MonsterStat> m_dicStat;
}


// ��ų�� ���� ������ ���� ������������
// ���� : ���Ÿ� �ٰŸ��� ���������� ����