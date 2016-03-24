using UnityEngine;
using System.Threading;
using System.Collections;

public class TechLead : EmployeeBase {

	private Thread thread;
	private bool threadIsWorking = true;
	private bool shouldhandlingTask = false;

	void Start(){
		superior = GameObject.FindGameObjectWithTag(Tags.ProductManager).GetComponent<ProductManager>();

		thread = new Thread(doJob);
		thread.Start();
	}

	public override bool receiveTask (TaskBase _task)
	{
		if(base.receiveTask (_task)){
			shouldhandlingTask = true;
			print("techlead is dealing with"+ _task.type +"in the lead way");	
		}
		return true;
	}

	private void doJob(){
		while(threadIsWorking){
			if(shouldhandlingTask){
				print("tech lead is working");

				//stimulate working process
				Thread.Sleep(Utils.GetRand(2000,7000));	

				if(Utils.RollDice()){
					finishTask();
				}else{
					unable_TransToSuperior();
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
