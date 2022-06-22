using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������

public enum SkillRangeType 
{
    Ray,
    Box,
    Shpere
}

public class Skill 
{
    public string name = "Name";
    public float coolTime = 0.0f;
    public float activeTime = 0.0f;

    public float _range = 0.0f;
    public float _demage = 0.0f;
    public SkillRangeType _rangeType = SkillRangeType.Ray;

    public List<Action> _skillAction = new List<Action>();

    // �Լ� �����͸� �̿��ؼ� ��ų�� ����Ѵ�.
    public void RegisterActive(Action p_active)
	{
        if(_skillAction.Contains(p_active)) {
            return;
		}

        _skillAction.Add(p_active);
	}

}