using UnityEngine;
using System.Collections;

public class Utils {

	public static int GetRand(int min, int max){
		System.Random random = new System.Random();
		return random.Next(min, max);
	}

	public static bool RollDice(){
		if(GetRand(0, 2) == 1){
			return true;
		}
		return false;
	}
}
