using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// EmployeesController is responsible for:
/// 1. Managing employees.
/// 2. Receiving tasks from TaskController.
/// 3. Assigning tasks to available employees.
/// </summary>
public class EmployeesController : SingletonT<EmployeesController> {
	[SerializeField]
	private int fresherCount = 5;
	[SerializeField]
	private Transform fresherPrefab;


	public ProductManager pm;
	public TechLead tl;

	[HideInInspector]
	public List<Fresher> freshers;

	void OnEnable(){
		TasksController.Instance.assignTask = dealTasksFromTaskCtrl;
		pm.returnTask = TasksController.Instance.handleReturnedTask;
	}
	void OnDisable(){
		TasksController.Instance.assignTask = null;
		pm.returnTask = null;
	}

	void Start(){

		freshers = new List<Fresher>();
		fresherCount = fresherCount > 1? fresherCount:1;
		for(int i = 0; i < fresherCount; ++i){
			var tmpObj = Instantiate(fresherPrefab, new Vector3(-6 + 3 * i, 0, 0), Quaternion.identity) as Transform;
			freshers.Add(tmpObj.GetComponent<Fresher>());
		}
	}

	/// <summary>
	/// Deal the tasks from TaskController. Accroding to types of tasks, handle them in different ways.
	/// </summary>
	/// <param name="task">Task.</param>
	public void dealTasksFromTaskCtrl(TaskBase task){
		if(task.type == TaskTypes.Client_Call){
			assignTaskToFresherOrLeader(task);
		}
	}

	//find available employees and assign tasks to them (freshers or tech lead).
	private void assignTaskToFresherOrLeader(TaskBase task){

		List<int> tmp_available_List = new List<int>();

		{
			lock(freshers){
				for(int i = 0; i < fresherCount; ++i){
					if(!freshers[i].isBusy){
						tmp_available_List.Add(i);
					}
				}
			}
		}

		{

			lock(tmp_available_List){
				//choose a random free fresher
				//if there are no available freshers, then transfer the task directly to the tech lead.
				if(tmp_available_List.Count > 0){
					int rand = Utils.GetRand(0, tmp_available_List.Count);
					freshers[tmp_available_List[rand]].receiveTask(task);
				}else{
					tl.receiveTask(task);
				}	
			}
		}

		tmp_available_List.Clear();
		tmp_available_List = null;
	}
}
