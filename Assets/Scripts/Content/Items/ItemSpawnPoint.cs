using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
	// ���� ��ȯ�� �ε���
	public int m_index = -1;


	public Color GetColor()
	{
		// -1 �� ���, �� ��찡 ���� ��� magenta / �����ؾ��Ѵٰ� �˸��� �뵵
		switch (m_index)
		{
			case -1:
				return Color.magenta;
			case 0:
				return Color.red;
			case 1:
				return Color.green;
			case 2:
				return Color.blue;
			case 3:
				return Color.cyan;
			case 4:
				return Color.yellow;
			case 5:
				return Color.white;
			default:
				return Color.magenta;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = GetColor();

		Gizmos.DrawWireCube(transform.position, new Vector3(2.0f, 2.0f, 2.0f));
	}
}
