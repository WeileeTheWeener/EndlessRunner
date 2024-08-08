public interface IState
{
    public void EnterState(PlayerController controller);
    public void UpdateState();
    public void ExitState();
    public string GetStateName();

    public void OnAnimatorMoveLogic();

}
