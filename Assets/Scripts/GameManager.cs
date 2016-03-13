using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
	public GameObject HeroFolder;
	public GameObject PlayerFolder;
	public HashSet<Hero> HeroesSet;
	public HashSet<Quest> QuestsSet;
	public HashSet<Quest> AvailableQuestsSet;
	public HashSet<Hero> AvailableHeroesSet;
	public HashSet<Quest> QuestsInProgress;
	public HashSet<TownStructure> TownStructureSet;
	public Player THEPLAYER;
	public GameObject QuestFolder;
	public GameObject TownStructureFolder;
	public List<int> InitialGoldReqForTownStructures;
	public Hero HeroToDisplay = null;
	public int NumberOfExactHeroes = 0;
	public LinkedList<Hero> haftq = null;
	bool selectionHeroes = false;
	bool updateSucc = false;
	bool updateFail =  false;
	public HashSet<GameObject> QuestScrollContainer = new HashSet<GameObject>();
	public HashSet<GameObject> HeroScrollContainer = new HashSet<GameObject>();
	public bool gameStart = true;
	public int gameTimer = 60000;
	public bool updateSuccBuyCheck = false;
	public bool updateFailBuyCheck = false;
	public bool NotEnoughForTannerybool = false;
	public bool NotEnoughXPForQuestbool = false;
	public bool criticalTime = false;
	public Town THETOWN = null;
	public string QuestJustEnded = "";


	public bool HeroDisplay = false;
	public List<Hero> UserSelectedHeroes = null;
	//[SerializeField] private Scrollbar Scrollbarvertical=null;
	//[SerializeField] private Scrollbar Scrollbarhorizontal=null;



	/*Buttons for Standard (init) Panel*/
	[SerializeField] private Button BuyButton = null;
	[SerializeField] private Button UpgradeButton = null;
	[SerializeField] private Button HeroesButton = null;
	[SerializeField] private Button QuestsButton = null;

	/*Buttons for Buy Panel*/
	[SerializeField] private Button BuyBuildingsButton = null;


	/*Buttons for Buy Buildings Panel*/
	[SerializeField] private Button BuyWorkshop = null;
	[SerializeField] private Button BuyBlacksmith = null;
	[SerializeField] private Button BuyApothecary = null;
	[SerializeField] private Button BuyTannery = null;
	[SerializeField] private Button BuyChurch = null;
	[SerializeField] private Button BuyMiscStructures = null;
	[SerializeField] private Button DestroyBuilding = null;

	//TODO
	//1. Create a function that all of the buy buildings button can feed into-- DONE
	//2. Create a dynamically sizing list of buttons with a scroll element --DONE
	//3. Fill the AvailableHeroesSet, add a 'name' component, 'class' component, and 'level component'
				/**********/


	public bool isBuyingBuilding = false;

	/*initializing the various panels*/
	public GameObject initPanel = null;
	public GameObject BuyPanel = null;
	public GameObject BuyBuildingsPanel = null;
	public GameObject HeroPanel = null;
	public GameObject QuestPanel = null;
	public GameObject NewQuestAvailablePanel = null;

	public GameObject prefabButton;
	public RectTransform ScrollbarHeroes;
	public RectTransform ScrollbarQuests;


	/*Initializing the various Texts*/
	public Text UpgradeButtonActiveText=null;
	public Text BuildOnPlotText = null;

	public GameObject plotCollider1Obj;
	public GameObject plotCollider2Obj;
	public GameObject plotCollider3Obj;
	public GameObject plotCollider4Obj;
	public GameObject plotCollider5Obj;
	public GameObject plotCollider6Obj;

	public BoxCollider2D plotCollider1;
	public BoxCollider2D plotCollider2;
	public BoxCollider2D plotCollider3;
	public BoxCollider2D plotCollider4;
	public BoxCollider2D plotCollider5;
	public BoxCollider2D plotCollider6;

	public List<BoxCollider2D> plotCols;

	public List<GameObject> plotObjs;

	public int curStructType;


    public Sprite mySprite;


	// Use this for initialization


	Player PlayerCharacter;

	void Start () {
		HeroFolder = new GameObject();  
		HeroFolder.name = "Heroes";		// The name of a game object is visible in the hHerarchy pane.
		QuestFolder = new GameObject();  
		QuestFolder.name = "Quests";		// The name of a game object is visible in the hHerarchy pane.
		PlayerFolder = new GameObject();  
		PlayerFolder.name = "Player";		// The name of a game object is visible in the hHerarchy pane.
		TownStructureFolder = new GameObject();  
		TownStructureFolder.name = "TownStructures";		// The name of a game object is visible in the hHerarchy pane.
		HeroesSet = new HashSet<Hero>();
		QuestsSet = new HashSet<Quest>();
		plotCols = new List<BoxCollider2D> ();
		AvailableQuestsSet = new HashSet<Quest>();
		AvailableHeroesSet = new HashSet<Hero> ();
		QuestsInProgress = new HashSet<Quest> ();
		TownStructureSet = new HashSet<TownStructure> ();
		THEPLAYER = null;
		initialisePlayer ();
		initialiseHeroes ();
		initialiseQuests ();
		gameStart = true;
		gameTimer = 30000;
		initialiseTown ();


		initialisePlotColliders ();


		//print (plotCollider1);



		/*Hiding the appropriate text windows*/
		UpgradeButtonActiveText.GetComponent <Text> (); //When the Upgrade button is clicked
		UpgradeButtonActiveText.gameObject.SetActive (false);
		BuildOnPlotText.GetComponent<Text> (); //When goldCheckandBuy is successful
		BuildOnPlotText.gameObject.SetActive(false);

		//-150 x, -30 y
		//UpgradeButtonActiveText.GetComponent<Text>().enabled = false;
		//UpgradeButtonActiveText.gameObject.SetActive (false);


		//Initial requirements for buying a TownStructure
		initializeInitialGoldReqForTownStructures ();






		initPanel = GameObject.Find("Standard Panel"); /*Initial panel*/
		BuyPanel = GameObject.Find ("Buy Panel"); /*Buy Window*/

		BuyBuildingsPanel = GameObject.Find ("List of Buildings Panel"); /*List of buildings to buy*/
		initPanel.SetActive (true); //initpanel sets to true

		//NewQuestAvailabe
		//NewQuestAvailablePanel = GameObject.Find("NewQuestAvailable");



		/*All of the other panels set to false*/
		BuyPanel.SetActive (false);
		BuyBuildingsPanel.SetActive (false);
		//NewQuestAvailablePanel.SetActive (false);

		/*Setting up the buttons*/

		/*Standard Panel Button Activation*/
		BuyButton.GetComponent<Button> ();
		BuyButton.onClick.AddListener(() => BuyButtonActive());
		UpgradeButton.GetComponent<Button> ();
		UpgradeButton.onClick.AddListener (() => UpgradeButtonActive ());
		HeroesButton.GetComponent<Button> ();
		HeroesButton.onClick.AddListener (() => HeroesButtonActive ());
		QuestsButton.GetComponent<Button> ();
		QuestsButton.onClick.AddListener (() => QuestsButtonActive ());
				/************/


		/*Buy Panel Button Activation*/
		BuyBuildingsButton.GetComponent<Button> ();
		BuyBuildingsButton.onClick.AddListener (() => BuyBuildingsActive ());


		/*Buy Buildings Button Activation*/
		BuyWorkshop.GetComponent<Button> ();
		BuyWorkshop.onClick.AddListener (() => BuyBuildings (0));
		BuyBlacksmith.GetComponent<Button> ();
		BuyBlacksmith.onClick.AddListener (() => BuyBuildings (1));
		BuyApothecary.GetComponent<Button> ();
		BuyApothecary.onClick.AddListener (() => BuyBuildings (2));
		BuyTannery.GetComponent<Button> ();;
		BuyTannery.onClick.AddListener (() => BuyBuildings (3));
		BuyChurch.GetComponent<Button> ();
		BuyChurch.onClick.AddListener (() => BuyBuildings (4));
		BuyMiscStructures.GetComponent<Button> ();
		BuyMiscStructures.onClick.AddListener (() => BuyBuildings (5));

		//DestroyBuilding.GetComponent<Button> ();



		//BuyButton.GetComponent<Button> ();
		//BuyButton.onClick.AddListener(() => BuyButtonActive());

//		plotCollider1 = GameObject.FindGameObjectWithTag ("plotCollider1") as BoxCollider2D;


	}
	
	// Update is called once per frame
	void Update () {
		if (!gameStart) {
			initPanel.SetActive (false);
		}
		if (gameTimer <= 0) {
			gameStart = false;
		}
	}

	// This is ot figure out how much gold for the initial buy of a townStucuture
	public void initializeInitialGoldReqForTownStructures(){
		InitialGoldReqForTownStructures = new List<int>();
		InitialGoldReqForTownStructures.Add (300);
		InitialGoldReqForTownStructures.Add (500);
		InitialGoldReqForTownStructures.Add (200);
		InitialGoldReqForTownStructures.Add (450);
		InitialGoldReqForTownStructures.Add (1000);


		

	}

	void initialisePlotColliders(){
		plotCollider1Obj = GameObject.Find ("Collider Plot 1");
		plotObjs.Add (plotCollider1Obj);
		plotCollider2Obj = GameObject.Find ("Collider Plot 2");
		plotObjs.Add (plotCollider2Obj);
		plotCollider3Obj = GameObject.Find ("Collider Plot 3");
		plotObjs.Add (plotCollider3Obj);
		plotCollider4Obj = GameObject.Find ("Collider Plot 4");
		plotObjs.Add (plotCollider4Obj);
		plotCollider5Obj = GameObject.Find ("Collider Plot 5");
		plotObjs.Add (plotCollider5Obj);
		plotCollider6Obj = GameObject.Find ("Collider Plot 6");
		plotObjs.Add (plotCollider6Obj);

		foreach (GameObject go in plotObjs) {
			Rigidbody2D tempRig = go.AddComponent<Rigidbody2D> ();
			LandPlots tempPlots = go.AddComponent<LandPlots> ();
			tempPlots.init (this, go);
			tempRig.isKinematic = false;
			tempRig.gravityScale = 0f;
		}

		plotCollider1 = plotCollider1Obj.GetComponent<BoxCollider2D> ();
		plotCols.Add (plotCollider1);
		plotCollider2 = plotCollider2Obj.GetComponent<BoxCollider2D> ();
		plotCols.Add (plotCollider2);
		plotCollider3 = plotCollider3Obj.GetComponent<BoxCollider2D> ();
		plotCols.Add (plotCollider3);
		plotCollider4 = plotCollider4Obj.GetComponent<BoxCollider2D> ();
		plotCols.Add (plotCollider4);
		plotCollider5 = plotCollider5Obj.GetComponent<BoxCollider2D> ();
		plotCols.Add (plotCollider5);
		plotCollider6 = plotCollider6Obj.GetComponent<BoxCollider2D> ();
		plotCols.Add (plotCollider6);

		foreach (BoxCollider2D boxy in plotCols) {
			boxy.isTrigger = true;
		}


	}





	void initialiseHeroes(){
		
		initialiseHero (0);
		initialiseHero (0);
		initialiseHero (1);
		initialiseHero (1);

	}

	void initialiseTown(){
		GameObject townObject = new GameObject ();			// Create a new empty game object that will hold a hero.
		Town theTown = townObject.AddComponent<Town> ();			// Add the hero.cs script to the object.
		// We can now refer to the object via this script.
		//theTown.transform.parent = TownStructureFolder.transform;
		//HeroFolder.Add (curHero);
		theTown.init(this );							// Initialize the hero script.
		theTown.name = "Town 1" ;						// Give the gem object a name in the Hierarchy pane.
		THETOWN = theTown;
		//print("initialised" + curHero.name);
	}





	void initialiseHero(int heroClass){
		
		GameObject heroObject = new GameObject ();			// Create a new empty game object that will hold a hero.
		Hero curHero = heroObject.AddComponent<Hero> ();			// Add the hero.cs script to the object.
		// We can now refer to the object via this script.
		curHero.transform.parent = HeroFolder.transform;
		//HeroFolder.Add (curHero);



		//heroRig;


		curHero.init(heroClass, this);							// Initialize the hero script.
		curHero.name = "Hero "+ HeroesSet.Count;						// Give the gem object a name in the Hierarchy pane.
		HeroesSet.Add(curHero);
		AvailableHeroesSet.Add (curHero);
		//print("initialised" + curHero.name);
	}

	void initialiseQuests(){
		int TimeNeeded = 80;
		int reqLevel = 1;
		int questCategory = 0;
		int reqXP = 1;
		List<int> reqClasses = null;
		List<Quest> previousAndNextList = null;
		int questType = 0;

		initialiseQuest (this, 0, 0, 80, 1, 40, new List<int> {-1}, null, 80);
		initialiseQuest (this, 0, 0, 100, 2, 50, new List<int> {-1}, null, 100);
		initialiseQuest(this, 0, 0 , 120, 3, 60, new List<int> {-1}, null, 140); 
		initialiseQuest(this, 0, 0 , 140, 4, 70, new List<int> {-1}, null, 165);
		initialiseQuest(this, 0, 1 , 40, 1, 40, new List<int> {-1}, null, 130);
		initialiseQuest(this, 0, 1 , 50, 2, 50, new List<int> {-1}, null, 145);
		initialiseQuest(this, 0, 1 , 60, 3, 60, new List<int> {-1}, null, 160);
		initialiseQuest(this, 0, 1 , 70, 4, 70, new List<int> {-1}, null, 175);
		initialiseQuest(this, 0, 2 , 80, 1, 40, new List<int> {-1}, null, 180);
		/*initialiseQuest(this, 0, 2 , 100, 2, 50, new List<int> {-1}, null, 210);
		initialiseQuest(this, 0, 2 , 120, 3, 60, new List<int> {-1}, null, 240);
		initialiseQuest(this, 1, 2 , 140, 4, 70, new List<int> {-1}, null, 270);
		initialiseQuest(this, 1, 4 , 240, 3, 160, new List<int> {0, 1}, null, 100);
		initialiseQuest(this, 1, 4 , 240, 3, 160, new List<int> {0, -1}, null, 100);
		initialiseQuest(this, 1, 4 , 560, 4, 800, new List<int> {0, 1, -1}, null, 1000);*/



	}



	void initialiseQuest(GameManager man, int questType, int questCategory, int TimeNeeded, int reqLevel, int reqXP, List<int> reqClasses, List<Quest> previousAndNextList, int gReward
	) {

			GameObject questObject = new GameObject ();			// Create a new empty game object that will hold a hero.
			Quest curQuest = questObject.AddComponent<Quest> ();			// Add the hero.cs script to the object.
			// We can now refer to the object via this script.
			curQuest.transform.parent = QuestFolder.transform;

		curQuest.init(this, questType, questCategory, TimeNeeded, reqLevel, reqXP, reqClasses, previousAndNextList, gReward);							// Initialize the hero script.

			curQuest.name = "Quest "+ QuestsSet.Count;						// Give the gem object a name in the Hierarchy pane.
			QuestsSet.Add(curQuest);
			
			
		}


	//***********
	//***********
	// This function will check if the user has enough gold to buy the townstructure that was selected through GUI
	//***********
	//***********
	public void goldCheckAndBuy(int TownStructureType){
		// Since we havent initialised this townStrucuture - we need to be able to check for its required Gold
		// For this Im thinking of adding a list indexed by townStructureType and their initial required gold
		int goldRequired = InitialGoldReqForTownStructures[TownStructureType];
		if (THEPLAYER.Gold > goldRequired + 10) {
			if (TownStructureType == 3) {
				if (TownStructureSet.Count >= 1) {
					//buyTownStructure (TownStructureType);
					curStructType = TownStructureType;
					this.isBuyingBuilding = true;



					THEPLAYER.Gold -= goldRequired;
					BuyBuildingsPanel.SetActive (false);
					//print("Successful");
					//BuildOnPlotText.gameObject.SetActive (true);
					initPanel.SetActive (true);
					//buyTownStructure (TownStructureType);
					StartCoroutine (updateSucccBuyCheck ());
				} else {
					StartCoroutine (NotEnoughForTannery ());
				}
			} else {


				//buyTownStructure (TownStructureType);
				THEPLAYER.Gold -= goldRequired;
				BuyBuildingsPanel.SetActive (false);
				//print("Successful");
				//BuildOnPlotText.gameObject.SetActive (true);
				initPanel.SetActive (true);
				curStructType = TownStructureType;

				this.isBuyingBuilding = true;

				//buyTownStructure (TownStructureType);
				StartCoroutine (updateSucccBuyCheck ());
			}

		} else {
			// Not enough gold to but the required townStrucuture
			//***********
			// Open a dialog box and tell it to the user. 
			//***********
			StartCoroutine (updateFailureBuyCheck());
			//print("Not enough gold");
		}
	}

	public void QuestJustEndedFunc(string x){
		this.QuestJustEnded = x;		
		StartCoroutine (QuestJustEndedRoutine());
	}

//	IEnumerator pickAPlot(){
//		bool done = false;
//		while (done == false) {
//			
//		}
//	}
//
	IEnumerator QuestJustEndedRoutine(){
		yield return new WaitForSeconds (3);
		QuestJustEnded = "";
	}


	IEnumerator NotEnoughForTannery(){
		NotEnoughForTannerybool = true;
		yield return new WaitForSeconds (3);
		NotEnoughForTannerybool = false;

	}

	IEnumerator updateSucccBuyCheck(){
		updateSuccBuyCheck = true;
		yield return new WaitForSeconds (3);
		updateSuccBuyCheck = false;
		this.curStructType = -1;

	}

	IEnumerator updateFailureBuyCheck(){
		updateFailBuyCheck =  true;
		yield return new WaitForSeconds(3);
		updateFailBuyCheck = false;
	}




	public void buyTownStructure(int TownStructureType, Vector3 pos){
		GameObject townStructureObject = new GameObject ();
		TownStructure newTownStructure = townStructureObject.AddComponent<TownStructure> ();
		newTownStructure.transform.parent = TownStructureFolder.transform;
		Vector3 positionOnMap = pos;
//		pos.z = -1;
		newTownStructure.transform.position  = positionOnMap;

		BoxCollider2D townBox = townStructureObject.AddComponent<BoxCollider2D>();
		Rigidbody2D townRig = townStructureObject.AddComponent<Rigidbody2D> ();
		print ("made it this far");
		townBox.isTrigger = true;
		townRig.gravityScale = 0f;
		townRig.isKinematic = true;
		newTownStructure.init (TownStructureType, this, THEPLAYER);

		newTownStructure.name = "TownStructure " + TownStructureSet.Count;
		TownStructureSet.Add (newTownStructure);

	}


	void initialisePlayer(){
		GameObject playerObject = new GameObject ();			// Create a new empty game object that will hold a hero.
		Player thePlayer = playerObject.AddComponent<Player> ();			// Add the hero.cs script to the object.
		Animator anim = playerObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load("Animations/playerAC") as RuntimeAnimatorController;
        SpriteRenderer renderer = playerObject.AddComponent<SpriteRenderer>();
        Object[] sprites;
        sprites = Resources.LoadAll("TextureFold/361_character_sheet");
        renderer.sprite = (Sprite)sprites[0];
		// We can now refer to the object via this script.
		thePlayer.transform.parent = PlayerFolder.transform;



		thePlayer.transform.position = new Vector3 (0f, 0f, 0f);

		BoxCollider2D playerBox = playerObject.AddComponent<BoxCollider2D>();
		Rigidbody2D playerRig = playerObject.AddComponent<Rigidbody2D> ();
		print ("made it this far");


		playerBox.isTrigger = true;
		playerRig.gravityScale = 0f;
		playerRig.isKinematic = true;
		playerRig.constraints = RigidbodyConstraints2D.FreezeRotation;
		//HeroFolder.Add (curHero);
		thePlayer.init(this);							// Initialize the hero script.
		thePlayer.name = "Player 1" ;						// Give the gem object a name in the Hierarchy pane.
		THEPLAYER = thePlayer;
		//print("initialised" + curHero.name);
	}

	// We get the TownStructure from the GUI and Colliders

	public void goldCheckAndUpdate(TownStructure toCheck){
		int goldReq = toCheck.goldReq;
		if (THEPLAYER.Gold > goldReq + 15) {
			updateTownStructure (toCheck);
			StartCoroutine (updateSuccc());
		} else {
			// Gold is not enough to update - print error
			// We would probably need to open up a dialog box here as well
			StartCoroutine (updateFailure());
			print("Not enough gold to upgrade");
		}
	}

	IEnumerator updateSuccc(){
		updateSucc = true;
		yield return new WaitForSeconds (3);
		updateSucc = false;
	}

	IEnumerator updateFailure(){
		updateFail =  true;
		yield return new WaitForSeconds(3);
		updateFail = false;
	}


	void updateTownStructure(TownStructure toUpdate){
		toUpdate.updatelevel ();
	}



	// THIS SECTION (STARTING HERE) WORKS WITH CHECKING AND ASSIGNING QUESTS


	IEnumerator NotEnoughXPForQuest(){
		NotEnoughXPForQuestbool =  true;
		yield return new WaitForSeconds(3);
		NotEnoughXPForQuestbool = false;
	}
	//Will let GUI Function get the Quest that we have to work with
	public void questAssignCheck(Quest x){
		
		if (THEPLAYER.XP < x.reqXP) {
			StartCoroutine (NotEnoughXPForQuest ());
			// At this point, we cannot carry out this quest
			//print("this quest is not available due to lack of user XP);
			//Opena  GUI button and show this to the user.
		} else {
		// At this point we can check other things for the Quest:
			LinkedList<Hero> HeroesAvailableForThisQuest = new LinkedList<Hero>();
			// We find all the heroes available for teh Quet
			foreach (Hero nextAvailable in AvailableHeroesSet) {
				
				if (x.reqClasses == null) {
					HeroesAvailableForThisQuest.AddLast (nextAvailable);
				} else if(x.reqClasses.Count == 1 && x.reqClasses[0] == -1){
					HeroesAvailableForThisQuest.AddLast (nextAvailable);

				}else {


					foreach (int classNeeded in x.reqClasses) {
						if (classNeeded == nextAvailable.heroClass) {
							HeroesAvailableForThisQuest.AddLast (nextAvailable);
						}
					}
				}
			}

			// Create Buttons for all of these Heroes

			// Now we have the heroes available for this Quest: 
			// At this point, we give the user the ability to select heroes for this quest
			// Here a GUI Button will be needed
			// I have a function getUserSelectedHeroesForThisQuest(List<Hero> AvailableHeroesForThisQuest, int NumberOfExactHeroes)
			// this function returns a list of heroes the user has seleceted for this quest;



			//************* This is the function //*************
			NumberOfExactHeroes = x.reqClasses.Count;
			createButtons(HeroesAvailableForThisQuest);
			//UserSelectedHeroes = getUserSelectedHeroesForThisQuest(List<Hero> AvailableHeroesForThisQuest, int NumberOfExactHeroes);

			//*************
			assignQuest(x, UserSelectedHeroes);
		

		
		}
	}
	void createButtons(LinkedList<Hero> haftq ){
		this.haftq = haftq;
		selectionHeroes = true;


	}



	public void assignQuest(Quest x, List<Hero> UserSelectedHeroes){
		//Remove this quest from available quests
		AvailableQuestsSet.Remove (x);

		//Add this to ongoing quests
		QuestsInProgress.Add(x);

		//Remove the heroes selected from this quest from available heroes
		foreach (Hero thisHero in UserSelectedHeroes){
			AvailableHeroesSet.Remove(thisHero);
		}

		//Signal to Quest x to begin itself;
		x.beginQuest(UserSelectedHeroes);


	}


	void BuyButtonActive(){

		/*Set the functionality for all the buy buttons*/
		initPanel.SetActive (false);
		BuyPanel.SetActive (true);

	}
	void UpgradeButtonActive(){
		initPanel.SetActive(false);
		UpgradeButtonActiveText.gameObject.SetActive (true);
		//print ("Yeah");
		//UpgradeButtonActiveText.fontSize = 22;
	}
	void HeroesButtonActive(){
		initPanel.SetActive (false);
		HeroPanel.SetActive (true);
		//Scrollbarhorizontal.gameObject.SetActive (false);
		//	for(int i = 1; i <= AvailableHeroesSet.Count; i++)
		foreach (Hero curHero in AvailableHeroesSet)
		{

			//print ("Reached");

			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.transform.SetParent (ScrollbarHeroes, false);
			goButton.transform.localScale = new Vector3 (1, 1, 1);

			Button tempButton = goButton.GetComponent<Button> ();
			tempButton.GetComponentInChildren<Text> ().text = curHero.xname;
			//heroes.name+"   " + heroes.heroClass + "    Lv:" heroes.experienceLevel; 
			/**Note: We need a format script so that the name and level and class are all aligned in the list **/
			//	int tempInt = i;
			HeroScrollContainer.Add(goButton);
			Hero heroToAdd = curHero;
				tempButton.onClick.AddListener (() => HeroButtonClicked (heroToAdd));


		}

		GameObject HeroBackButton = (GameObject)Instantiate (prefabButton);
		HeroBackButton.transform.SetParent (ScrollbarHeroes, false);
		HeroBackButton.transform.localScale = new Vector3 (1, 1, 1);
		Button tempButtonx = HeroBackButton.GetComponent<Button> ();
		tempButtonx.GetComponentInChildren<Text> ().text = "Back";
		tempButtonx.onClick.AddListener (() => HeroBackButtonClicked ());
		HeroScrollContainer.Add(HeroBackButton);

	}

	void HeroBackButtonClicked(){
		//Need to delete all buttons from the scroll bar
		initPanel.SetActive (true);
		HeroPanel.SetActive (false);
		foreach (GameObject x in HeroScrollContainer) {
			Destroy (x);
		}


		//Deleting all previous buttons

	}

	void HeroButtonClicked(Hero curHero){
		// We have a hero here. 
		//Display its Properties?
		HeroDisplay = true;
		HeroToDisplay = curHero;


	}

	void ButtonClicked(int buttonNo)
	{
			Debug.Log ("Button clicked = " + buttonNo);
	}
		
	
	void QuestsButtonActive(){
		initPanel.SetActive (false);
		QuestPanel.SetActive (true);

		foreach (Quest curQuest in QuestsSet)
			
		{



			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.transform.SetParent (ScrollbarQuests, false);
			goButton.transform.localScale = new Vector3 (1, 1, 1);

			Button tempButton = goButton.GetComponent<Button> ();
			tempButton.GetComponentInChildren<Text> ().text = curQuest.xname;
			//heroes.name+"   " + heroes.heroClass + "    Lv:" heroes.experienceLevel; 
			/**Note: We need a format script so that the name and level and class are all aligned in the list **/

			QuestScrollContainer.Add (goButton);
			Quest questToClick = curQuest;
			tempButton.onClick.AddListener (() => QuestButtonClicked (questToClick));


		}
		GameObject QuestBackButton = (GameObject)Instantiate (prefabButton);
		QuestBackButton.transform.SetParent (ScrollbarQuests, false);
		QuestBackButton.transform.localScale = new Vector3 (1, 1, 1);
		Button tempButtonx = QuestBackButton.GetComponent<Button> ();
		tempButtonx.GetComponentInChildren<Text> ().text = "Back";
		tempButtonx.onClick.AddListener (() => QuestBackButtonClicked ());

		QuestScrollContainer.Add (QuestBackButton);

	}

	void QuestBackButtonClicked(){
		//Need to delete all buttons from the scroll bar
		initPanel.SetActive (true);
		QuestPanel.SetActive (false);
		foreach (GameObject x in QuestScrollContainer) {
			Destroy (x);
		}
	}

	void QuestButtonClicked(Quest quest){
		
		print ("Quest Button Clicked" + quest.xname);
		questAssignCheck (quest);
	}




	void BuyBuildingsActive()
	{
		BuyPanel.SetActive (false);
		BuyBuildingsPanel.SetActive (true);
	}

	void BuyBuildings(int BuildingType)

	{
		//print ("Reached Buy Buildings Function");
		goldCheckAndBuy (BuildingType);

	}



	void OnGUI () {
		GUI.Button (new Rect (10,Screen.height - 80,150,40), "Player Gold: " + THEPLAYER.Gold + " \n Player XP: "+THEPLAYER.XP ) ; 
		GUI.Button (new Rect (Screen.width - 160,Screen.height - 80,150,40), "Infrastructure Level: " + THETOWN.infrastructureLevel ) ; 
		// Printing goes to the Console pane.  
		// If an object doesn't extend monobehavior, calling print won't do anything.  
		// Make sure "Collapse" isn't selected in the Console pane if you want to see duplicate messages.
		if (HeroDisplay == true) {
			if (GUI.Button (new Rect (10, 100, 250, 80), 
				"Hero Name: "  + HeroToDisplay.xname + "\n" +
				"Hero Type: "  + HeroToDisplay.heroClass + "\n" +
				"Hero Fetch Time: "  + HeroToDisplay.fetchTime + "\n" +
				"Hero Grind Time: " + HeroToDisplay.grindTime + "\n" +
				"Hero Delivery Bonus: " + HeroToDisplay.deliverBonus + " "
			)) {
				HeroDisplay = false;
				initPanel.SetActive (true);
			}
		}

		if (QuestsInProgress.Count > 0) {
			GUI.Button (new Rect (10,80,150,20), "Active Quests: " ) ; 
			int y = 105;
			foreach (Quest p in QuestsInProgress) {
				string z = "\n" + p.xname + "\n";
			
			
				int i = 1;	
				foreach (Hero h in p.HeroesAssignedToThisQuest){
					z= z+ "Hero " + i + ": " + h.xname + "\n";
					i++;
					}
				GUI.Button (new Rect (10,y,150,20*i), z ) ; 

				y += 20*i + 30;
			}



		}


		if (selectionHeroes == true && NumberOfExactHeroes != 0) {
			int y = 110;
			int x = Screen.width / 2 - 175;
			GUI.Box (new Rect (x, 80, 350, 20), "Choose remaining " + NumberOfExactHeroes + " heroes for this quest");
			foreach (Hero HeroToDisplay in haftq) {
				if (GUI.Button (new Rect (x + 50, y, 250, 100), 
					"Hero Name: "  + HeroToDisplay.xname + "\n" +
					"Hero XP Level: "  + HeroToDisplay.heroClass + "\n" +
					"Hero Fetch Time: "  + HeroToDisplay.fetchTime + "\n" +
					"Hero Grind Time: " + HeroToDisplay.grindTime + "\n" +
					"Hero Delivery Bonus: " + HeroToDisplay.deliverBonus + " "
				)) {
					//HeroDisplay = false;
					NumberOfExactHeroes--;
					UserSelectedHeroes.Add (HeroToDisplay);
					haftq.Remove (HeroToDisplay);
					if (NumberOfExactHeroes == 0) {
						selectionHeroes = false;
						QuestPanel.SetActive (false);
						initPanel.SetActive (true);

					}
				}
			
				y += 105;
			}
				
		}

		if (updateSucc) {
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height/2 - 20, 300, 40), "Update Successful");
		}


		if (updateFail) {
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height/2 - 20, 300, 40), "Not enough gold to update");
		}

		if (updateSuccBuyCheck) {
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height/2 - 20, 300, 40), "You just bought a building");
		}


		if (updateFailBuyCheck) {
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height/2 - 20, 300, 40), "Not enough gold to buy a building");
		}

		if(NotEnoughForTannerybool){
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height/2 - 20, 300, 40), "Tannery requires at least one functional building.");
		}

		if(NotEnoughXPForQuestbool){
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height/2 - 20, 300, 40), "You need more XP to initiate this Quest");
		}



		if (gameStart) {
			if (gameTimer <= 0) {
				gameStart = false;
			}

			GUI.Box(new Rect ( 10, Screen.height - 35, 150, 20), "Time Left; "+gameTimer);
			gameTimer--;

		
		}

		if (!gameStart) {
		
			GUI.Box(new Rect ( Screen.width/2 - 200, Screen.height/2 -200 , 400, 40), "GAME OVER!");
		}

		if (criticalTime) {
			GUI.Box (new Rect ( Screen.width/2 - 150, Screen.height - 60, 300, 40), "Please focus on maintaining your town \n It is reaching Critical Levels");
		}

		if (!QuestJustEnded.Equals ("")) {
			GUI.Box(new Rect ( Screen.width/2 - 200, Screen.height/2 -200 , 400, 40), this.QuestJustEnded);
		}






			

	
	
	}






}




