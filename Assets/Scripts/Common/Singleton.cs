using UnityEngine;
using System.Collections;

public class SingletonT<T> : MonoBehaviour 
	where T : Component 
{
	private static T instance;

	public static T Instance{
		get{
			if(instance == null){
				instance = (T) GameObject.FindObjectOfType(typeof(T)); 
				if(instance == null){
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.HideAndDontSave;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;	
		}
	}

	public virtual void Awake(){
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this as T;
		} else {
			if(instance != this){
				Destroy(this.gameObject);
			}
		}
	}
}