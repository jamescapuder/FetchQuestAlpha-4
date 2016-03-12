using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest : MonoBehaviour {

    //Constant (immutable) variables representing time availability of this quest.



	// These are the possible QuestTypes:
    public const int LIMITEDAVAIL = 1;

    public const int ANYTIME = 0;
	//Possibilities for questtypes end here

    //Same as above but for questCategory

	//These are questcategories
    public const int FETCH = 0;

    public const int GATHER =3;

    public const int DELIVER = 2;

    public const int GRIND = 1;

    public const int SPECIAL = 4;

	public string xname = "";
	public int whenAvailable = 0;
	public int isLimitedStarted = 0;
	//public int checker = 0;
	//Quest categories end here.


    //Previous and next quest in chain, used for linked list style implementation of quest chains

	public Quest previous;

	public Quest next;

    //Boolean value for whether or not this quest is the final in a chain

    public bool isFinalInChain;

    //Ints for quest time,

	public int goldReward;
	public int XPReward;
	public int HeroXPReward;



	public int QuestType;
	public int QuestCategory; 
	public int TimeNeeded; 
	public int reqLevel;
	public int reqXP;

    //List of integer represeentations of the required classes

	public List<int> reqClasses;
	public List<Hero> HeroesAssignedToThisQuest;

    private GameManager manager;

	/*
    Init for Quest object, takes in the numerical values listed in the design doc aas parameters
    to instantiate the above fields. Additionally, we pass in a list of two quests, representing previous and next in the chain.
    */
	public void init(GameManager man, int questType, int questCategory,int TimeNeeded, int reqLevel, int reqXP, List<int> reqClasses, List<Quest> previousAndNextList, int gReward) {
		this.goldReward = gReward;
		this.TimeNeeded = TimeNeeded;
        this.reqLevel = reqLevel;
        this.manager = man;
        this.QuestCategory = questCategory;
        this.reqXP = reqXP;
        this.reqClasses = reqClasses;
        this.QuestType = questType;

        //If the quest is limited availability, we want to set its previous quest as specified by the parameter

		if (questCategory == 0) {
			this.xname = "FETCH: " + TimeNeeded + " secs";
		} else if (questCategory == 1) {
			this.xname = "GRIND: " + TimeNeeded + " secs";
		} else if (questCategory == 2) {
			this.xname = "DELIVER: " + TimeNeeded + " secs";
		} else if (questCategory == 3) {
			this.xname = "GATHER: " + TimeNeeded + " secs";
		} else if (questCategory == 4) {
			this.xname = "SPECIAL: " + TimeNeeded + " secs";
		}

		if (questType == LIMITEDAVAIL)
        {
            this.previous = previousAndNextList[0];

            //Now, we can infer the isFinalInChain value by checking if the second value of
            //previousAndNextList is null. If it is, the value is true, false otherwise.

            if (previousAndNextList[1] == null)
            {
                this.isFinalInChain = true;
            }
            else
            {
                this.isFinalInChain = false;
            }
        }

		if (this.QuestType == 0) {
			man.AvailableQuestsSet.Add (this);
		} else {
			System.Random rnd = new System.Random();
			int x = rnd.Next (10000, manager.gameTimer);
			this.whenAvailable = x;
			this.isLimitedStarted = 0;
		}
    }


	public void beginQuest(List<Hero> UserGivenHeroesForThisQuest){
		this.HeroesAssignedToThisQuest = UserGivenHeroesForThisQuest;
		foreach (Hero thisHero in this.HeroesAssignedToThisQuest) {
			this.TimeNeeded += thisHero.fetchTime;
			this.TimeNeeded += thisHero.grindTime;
		}
		//print ("quest Startedx:" +this.xname);
		StartCoroutine ("questRoutine");


	}

	void Update(){
		if (this.QuestType == 1) {
			if (this.whenAvailable <= this.manager.gameTimer && this.isLimitedStarted == 0) {
				StartCoroutine ("newQuestActivated");
				manager.AvailableQuestsSet.Add (this);
				this.isLimitedStarted = 1;
			}
		}



	}



	//Want to call endQuest TimeNeeded seconds after beginQuest is called 

	IEnumerator newQuestActivated(){
		this.manager.NewQuestAvailablePanel.SetActive (true);
		yield return new WaitForSeconds (3);
		this.manager.NewQuestAvailablePanel.SetActive (false);
	}

	IEnumerator questRoutine(){
		//print ("quest Started:" +this.xname);
		yield return new WaitForSeconds (this.TimeNeeded);
		this.endQuest();
	}


	public void endQuest(){
		//print ("quest Ended");

		manager.THEPLAYER.Gold += this.goldReward;
		manager.THEPLAYER.XP += this.XPReward;
		foreach (Hero thisHero in this.HeroesAssignedToThisQuest) {
			thisHero.addWin (this);
			manager.AvailableHeroesSet.Add (thisHero);

		}
		this.HeroesAssignedToThisQuest.Clear ();
		string z = "The Quest: " + this.xname + " just ended!";
		this.manager.QuestJustEndedFunc (z);

		manager.QuestsInProgress.Remove (this);
		if (this.QuestType == ANYTIME) {
			manager.AvailableQuestsSet.Add (this);
		}



	
	}


}
