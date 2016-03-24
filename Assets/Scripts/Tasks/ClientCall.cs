using UnityEngine;
using System.Collections;

public class ClientCall : TaskBase {
	void Start(){
		details = "a client call";
		type = TaskTypes.Client_Call;
	}
}
