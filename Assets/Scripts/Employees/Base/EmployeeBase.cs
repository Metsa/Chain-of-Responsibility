using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ---------------------------------------------------------------------------------------
/// The process of handling tasks.
/// receiveTask - if(isbusy)  -  transferToSuperior.receiveTask 
/// 			- if(notbusy) -  if(could sole it)       - solve the task
/// 							 if(coult not solve it)  - transferToSuperior.receiveTask
/// ---------------------------------------------------------------------------------------
/// </summary>
public abstract class EmployeeBase : MonoBehaviour, IEmployeeActions {
	
	/// <summary>
	/// Get the dealt tasks count.
	/// </summary>
	/// <value>The dealt tasks count.</value>
	public int DealtTasksCount{
		get{
			return solvedTasksList.Count;
		}
	}

	/// <summary>
	/// skill level of employee
	/// </summary>
	public Levels skillLevel;

	/// <summary>
	/// If current employee is busy
	/// </summary>
	public volatile bool isBusy = false;

	/// <summary>
	/// Employee's superior.
	/// </summary>
	protected EmployeeBase superior = null;

	/// <summary>
	/// the task which is currenylt being handling.
	/// </summary>
	protected TaskBase dealingTask = null;

	//the list of all solved tasks
	private List<TaskBase> solvedTasksList = new List<TaskBase>();


	////////////For Unity/////////////
	private Vector3 pos;
	private Renderer render;
	void Awake(){
		pos = gameObject.transform.position;
		render = GetComponent<Renderer>();
	}
	////////////For Unity/////////////

	/// <summary>
	/// Receives the task.
	/// To freshers, tasks come from TasksController. To TL, PM, tasks come from their lower level.
	/// </summary>
	/// <returns><c>true</c>, if task was received, <c>false</c> otherwise.</returns>
	/// <param name="task">Task.</param>
	public virtual bool receiveTask (TaskBase task)
	{
		////////////For Unity/////////////
		task.couldMove = true;
		task.targetPos = pos;
		////////////For Unity///////////// 

		//if it's busy transfer the task to its superior.
		if(isBusy){
			tooBusy_TransferTaskToSuperior(task);
			return false;
		}

		//if not busy, then start handling this task.
		isBusy = true;
		dealingTask = task;
		return true;
	}

	/// <summary>
	/// Finish the task.
	/// </summary>
	protected virtual void finishTask(){
		print("a task was finished");
		solvedTasksList.Add(dealingTask);
		isBusy = false;
		dealingTask.solved = true;
		dealingTask = null;
	} 

	/// <summary>
	/// too busy and Transfers to superior.
	/// </summary>
	protected virtual void tooBusy_TransferTaskToSuperior(TaskBase newtask){
		//if task is a new task
		if(newtask != dealingTask && isBusy){
			if(superior != null){
				superior.receiveTask(newtask);
			}	 
		}
	}

	/// <summary>
	/// unable to handle the task and transfer to superior.
	/// </summary>
	/// <param name="task">Task.</param>
	protected virtual void unable_TransToSuperior(){
		if(superior != null && dealingTask != null){
			superior.receiveTask(dealingTask);
			isBusy = false;
			dealingTask = null;
		}
	}

	public override string ToString ()
	{
		return string.Format ("skillLevel{0}, dealt {1} tasks", skillLevel, solvedTasksList.Count);
	}

	////////////For Unity///////////// 
	void Update(){
		if(isBusy){
			render.material.color = Color.red;
		}else{
			render.material.color = Color.green;
		}
	}
	////////////For Unity///////////// 
}
