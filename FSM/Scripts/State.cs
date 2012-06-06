using UnityEngine;
using System.Collections;

public abstract class State<T> {
	
	protected T mp_owner;
	public T Owner { get { return mp_owner; } }
	
	public State(T owner)
	{
		mp_owner = owner;
	}
	
	public abstract void Tick();
	public abstract void EnterState();
	public abstract void ExitState();
	
}
