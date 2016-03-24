using UnityEngine;
using System;
using System.Collections;
using System.Threading;

public class ProductManager : EmployeeBase {
	private Thread thread;
	private bool threadIsWorking = true;
	private bool shouldhandlingTask = false;

	//return the task if too busy to handle this
	public Action<TaskBase, string> returnTask;

	void Start(){
		superior = null;
		thread = new Thread(doJob);
		thread.Start();
	}

	public override bool receiveTask (TaskBase task)
	{
		if(base.receiveTask (task)){

			shouldhandlingTask = true;
			print("PM is dealing with"+ task.type +"in the PM way");	
			return true;
		}
		print("PM is too busy to start work");
		return false;
	}

	//if Product manager is too busy to handle the task, then return this task to the task queue
	protected override void tooBusy_TransferTaskToSuperior (TaskBase newtask)
	{
		returnTask(newtask, FeedBacks.BUSYLINE);
	}

	private void doJob(){

		while(threadIsWorking){
			if(shouldhandlingTask){
				print("PM is working");

				//stimulate working process
				Thread.Sleep(Utils.GetRand(2000,4000));	

				//PM 100% solved the problem
				finishTask();
				shouldhandlingTask = false;				
			}

			Thread.Sleep(500);
		}
	}

	void OnDisable(){
		shouldhandlingTask = false;
		if(thread.IsAlive){
			threadIsWorking = false;
		}
	}
}
