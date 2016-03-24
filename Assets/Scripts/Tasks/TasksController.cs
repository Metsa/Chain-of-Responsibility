using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// TasksController is responsible for:
/// 1. Managing different types of tasks.
/// 2. assign task to EmployeeController
/// </summary>
public class TasksController : SingletonT<TasksController> {

	public Transform callPrefab;

	public int initialCallsCount = 10;

	//the queue for client calls 
	protected Queue<TaskBase> callQueue;

	//callback from EmployeeController to handle ALL types of tasks.
	public Action<TaskBase> assignTask;

	private Thread tasksThread;
	private bool threadIsWorking = true;
	private bool shouldGenerate = true;

	void Start(){
		callQueue = new Queue<TaskBase>();

		for(int i = 0; i < initialCallsCount; ++i){
			generateTask(TaskTypes.Client_Call);
		}
		tasksThread = new Thread(assignCallsToEmployees);
		tasksThread.Start();
	}

	public void generateTask(string _tasktype){
		if (_tasktype == TaskTypes.Client_Call) {
			
			//vector3 here is for visualizing the task
			var tmpObj = Instantiate(callPrefab, new Vector3(-7.5f + 1 * callQueue.Count, -2.0f,0f), Quaternion.identity) as Transform;
			//add the new task into queue
			callQueue.Enqueue(tmpObj.GetComponent<ClientCall>());
		}
	}

	//assign call task only. In the future, we may have client visit task. Maybe.
	public void assignCallsToEmployees(){
		while(threadIsWorking){
			if(shouldGenerate){
				lock(callQueue){
					if(callQueue.Count > 0){
						if(assignTask != null)
						{
							Thread.Sleep(1000);
							assignTask(callQueue.Dequeue());
							print("assigned a call");
						}
					}else{
						print("no more call tasks to go");
					}	
				}
			}	
			//give it a random second to assign next task
			Thread.Sleep(Utils.GetRand(1000,2000));	
		}
	}

	/// <summary>
	/// Handles the returned task. Add them back to the task queue and give feedback to clients. 
	/// Returned tasks will assign them again later.
	/// </summary>
	/// <param name="returnedTask">Returned task.</param>
	/// <param name="feedback">Feedback.</param>
	public void handleReturnedTask(TaskBase returnedTask, string feedback){
		restoreTask(returnedTask);
		print(feedback);
	}

	//move task to the end of queue
	//When all employees are busy, restore the task at the end of queue.
	private void restoreTask(TaskBase task){
		if(task.type == TaskTypes.Client_Call){
			callQueue.Enqueue(task);

			///////////for Unity///////////
			task.couldMove = true;
			task.targetPos = new Vector3(-7.5f + 1 * callQueue.Count, -4.0f,0f);
			///////////for Unity///////////
		}
	}

	void OnDisable(){
		if(tasksThread.IsAlive){
			shouldGenerate = false;
		}
	}
}
