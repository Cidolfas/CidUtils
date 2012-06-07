using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FSMChaseBot : MonoBehaviour {
	
	public Transform target;
	public StateMachine<FSMChaseBot> sm = new StateMachine<FSMChaseBot>();
	public float chaseRadius = 2f;
	public float runRadius = 15f;
	public float speed = 10f;
	
	// Use this for initialization
	void Awake ()
	{
		sm.AddState("Idle", new IdleState(this));
		sm.AddState("Chase", new ChaseState(this));
		sm.AddState("Run", new RunState(this));
		sm.ChangeState("Idle");
	}
	
	// Update is called once per frame
	void Update ()
	{
		sm.Tick();
	}
	
	// Ooh look, interrupts!
	void PlayerEnterHulkMode()
	{
		sm.ChangeState("Run");
	}
	
	void PlayerExitHulkMode()
	{
		sm.ChangeState("Idle");
	}
	
	// Begin the states here
	protected class IdleState : State<FSMChaseBot> {
		
		public IdleState(FSMChaseBot owner) : base(owner)
		{
			
		}
		
		public override void Tick ()
		{
			Vector3 positionDiff = mp_owner.target.transform.position - mp_owner.transform.position;
			if (positionDiff.sqrMagnitude < mp_owner.runRadius * mp_owner.runRadius) {
				// Begin the hunt!
				mp_owner.sm.ChangeState("Chase");
			}
			else {
				// Stay idle
			}
		}
		
		public override void EnterState ()
		{
			
		}
		
		public override void ExitState ()
		{
			
		}
	}
	
	protected class ChaseState : State<FSMChaseBot> {
		
		public ChaseState(FSMChaseBot owner) : base(owner)
		{
			
		}
		
		public override void Tick ()
		{
			Vector3 positionDiff = mp_owner.target.transform.position - mp_owner.transform.position;
			if (positionDiff.sqrMagnitude < mp_owner.chaseRadius * mp_owner.chaseRadius) {
				// We're too close, do nothing
			}
			else {
				// We need to move!
				positionDiff.Normalize();
				mp_owner.GetComponent<CharacterController>().SimpleMove(positionDiff * mp_owner.speed);
			}
		}
		
		public override void EnterState ()
		{
			Debug.Log("Target sighted, moving in!");
		}
		
		public override void ExitState ()
		{
			
		}
	}
	
	protected class RunState : State<FSMChaseBot> {
		
		public RunState(FSMChaseBot owner) : base(owner)
		{
			
		}
		
		public override void Tick ()
		{
			Vector3 positionDiff = mp_owner.target.transform.position - mp_owner.transform.position;
			if (positionDiff.sqrMagnitude > mp_owner.runRadius * mp_owner.runRadius) {
				// We're safe, for now
			}
			else {
				// We need to move!
				positionDiff.Normalize();
				mp_owner.GetComponent<CharacterController>().SimpleMove(positionDiff * -mp_owner.speed);
			}
		}
		
		public override void EnterState ()
		{
			Debug.Log("Eeek! Run away!");
		}
		
		public override void ExitState ()
		{
			
		}
	}
}
