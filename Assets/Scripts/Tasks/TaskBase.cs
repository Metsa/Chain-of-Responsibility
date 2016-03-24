using UnityEngine;
using System.Collections;

/// <summary>
/// The Base of all tasks. New Tasks should extend this. In current specific case, we only have Call task;
/// </summary>
public abstract class TaskBase : MonoBehaviour {
	//what the task is about
	public string details;
	//which type it is. For extention in the future, we may have client call tasks, client visit tasks, etc.
	public string type;

	/////////////for Unity///////////////
	[HideInInspector]
	public Vector3 targetPos;
	[HideInInspector]
	public bool couldMove = false;
	[HideInInspector]
	public bool solved = false;
	/////////////for Unity///////////////

	//////for unity visualizing//////
	void Update(){
		if(solved) gameObject.SetActive(false);

		if(!couldMove) return;
		if(Vector3.Distance(transform.position, targetPos) > 0.1){
			transform.position =  Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 7);
		}
	}
	//////for unity visualizing//////
}
