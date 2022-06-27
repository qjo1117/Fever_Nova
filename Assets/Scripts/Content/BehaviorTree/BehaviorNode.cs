
// ����
public enum BehaviorStatus
{
	Invaild,
	Running,
	Success,
	Failure,
}


// ���� �⺻���� ������ Ŭ����

public abstract class BehaviorNode
{
	// �θ� ��带 ����ŵ�ϴ�.
	protected BehaviorNode m_parent = null;
	// ���� ����� ���¸� ǥ���մϴ�.
	protected BehaviorStatus m_status = BehaviorStatus.Invaild;


	// ���¸� ��ȯ�մϴ�.
	public virtual BehaviorStatus Update() => BehaviorStatus.Invaild;

	// ���� �������� �˻��ϴ� �Լ� �Դϴ�.
	public bool IsSuccess() => m_status == BehaviorStatus.Success;
	public bool IsFailure() => m_status == BehaviorStatus.Failure;
	public bool IsRunning() => m_status == BehaviorStatus.Running;

	// ���¸� �ʱ�ȭ�մϴ�.
	public void StateReset() { m_status = BehaviorStatus.Invaild; }
}
