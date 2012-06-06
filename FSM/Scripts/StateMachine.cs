using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine<T> {
	
	protected State<T> mp_currentState = null;
	protected State<T> mp_previousState = null;
	
	protected Dictionary<string, State<T>> mp_stateList = new Dictionary<string, State<T>>();
	
	public void Tick()
	{
		// If there is no state, return
		if (mp_currentState == null) return;
		
		// If there is a current state, execute it's Tick
		mp_currentState.Tick();
	}
	
	public void ChangeState(string nextState)
	{
		// Make sure we're asking for a sane switch!
		if (!mp_stateList.ContainsKey(nextState)) {
			Debug.LogError("Requested state '" + nextState + "' is not on the state list.");
			return;
		}
		
		// Exit
		if (mp_currentState != null) mp_currentState.ExitState();
		
		// Change
		mp_previousState = mp_currentState;
		mp_currentState = mp_stateList[nextState];
		
		// Enter
		mp_currentState.EnterState();
	}
	
	public bool IsInState(string whichState)
	{
		if (mp_currentState == null) {
			if (whichState == null) return true;
			else return false;
		}
		
		if (!mp_stateList.ContainsKey(whichState)) return false;
		
		if (mp_currentState == mp_stateList[whichState]) return true;
		else return false;
	}
	
}
