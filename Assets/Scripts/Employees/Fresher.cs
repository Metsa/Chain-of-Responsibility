using UnityEngine;
using System.Collections;
using System.Threading;

public class Fresher : EmployeeBase {

	private Thread thread;
	private bool threadIsWorking = true;
	private bool shouldhandlingTask = false;

	void Start(){
		superior = GameObject.FindGameObjectWithTag(Tags.TechLead).GetComponent<TechLead>();

		thread = new Thread(doJob);
		thread.Start();
	}

	public override bool receiveTask (TaskBase task)
	{
		if(base.receiveTask (task)){

			shouldhandlingTask = true;
			print("fresher is dealing with"+ task.type +"in the fresher way");	
			return true;
		}
		print("fresher is too busy to start work");
		return false;
	}

	private void doJob(){
		while(threadIsWorking){
			if(shouldhandlingTask){
				print("a fresher is working");
				//stimulate working process
				Thread.Sleep(Utils.GetRand(2000,10000));	
				if(Utils.RollDice()){ 
					//if solved the problem
					finishTask();
				}else{
					unable_TransToSuperior();
					print("freser transfer taks to superior");
				}
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
