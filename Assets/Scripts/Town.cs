using UnityEngine;
using System.Collections;

public class Town : MonoBehaviour {

	// Use this for initialization
	public GameManager manager;
	public int infrastructureLevel = 0;
	public int buildingTimer = 1;
	public int numBuildings = 0;
	public int moder = 1000;




	public void init(GameManager m){
		this.manager = m;
		this.infrastructureLevel = 50;

	}


		


	void infrastructureCheck(){
		

		if (infrastructureLevel < 20) {
			StartCoroutine(criticalRoutine ());
		}



		int numBuidings = this.manager.TownStructureSet.Count;
		if (numBuidings == 0) {
			infrastructureLevel-= 4;
			return;
		}



		int x = ((buildingTimer / 1000) * numBuidings) % 20;
		infrastructureLevel += (x * 10) / infrastructureLevel;


	}
	
	// Update is called once per frame
	void Update () {
		if (infrastructureLevel <= 0) {
			manager.gameStart = false;
		}
		buildingTimer++;
		if (buildingTimer % moder == 0) {
			//At this point, effect the infrastructure
			infrastructureCheck();

		}






	}

	IEnumerator criticalRoutine(){
		manager.criticalTime = true;
		yield return new WaitForSeconds (5);
		manager.criticalTime = false;
	}







}
