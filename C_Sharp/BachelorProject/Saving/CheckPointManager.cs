using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CheckPointManager : MonoBehaviour
{
    #region Inspector - variables

    [SerializeField]
    private bool useDebugging = true;

    [SerializeField]
    private bool shouldDeleteSaveFilesAtStart = true;

    [SerializeField]
    private Ability[] localAbilities;

    [SerializeField]
    private Puzzles[] localPuzzles;


    #endregion


    #region Update Functions
    private void Awake()
    {
        mXMLHandler = GetComponent<XMLSerialization>();
        //mGameManager = GameObject.Find("Managers").transform.GetChild(6).GetComponent<GameManager>();

        if (shouldDeleteSaveFilesAtStart)
        {
            if(File.Exists(mXMLHandler.GetPath(0)))
                File.Delete(mXMLHandler.GetPath(0));

            if (File.Exists(mXMLHandler.GetPath(1)))
                File.Delete(mXMLHandler.GetPath(1));
        }

            
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadingSave();
        }
    }

    #endregion


    #region MainFunctions - XML

    private XMLSerialization mXMLHandler = null;

    public void Save()
    {
        SavePickups();
        SaveSpawnPoint();
        //SavedGameData();
    }

    public void LoadingSave()
    {
        LoadSpawnPoint();
        LoadPickups();
        //LoadGameData();
               
    }

    #endregion

    //#region Game data

    //private GameManager mGameManager = null;

    //private void SavedGameData()
    //{
    //    Saving_GameData gameData = new Saving_GameData();

    //    gameData.collectablesCollected = mGameManager.Collected_pickups;

    //    gameData.playerHealth = mGameManager.Health;

    //    gameData.snehettaDead = mGameManager.SnehettaDead;

    //    gameData.snowmanDead = mGameManager.SnowmanDead;

    //    gameData.tutorialFinished = mGameManager.TutorialFinished;

    //    gameData.puzzles = new List<Puzzles>();
    //    gameData.puzzles.Clear();
    //    gameData.puzzles.AddRange(localPuzzles);

    //    gameData.abilities = new List<Ability>();
    //    gameData.abilities.Clear();
    //    gameData.abilities.AddRange(localAbilities);

    //    mXMLHandler.SaveData(gameData, mXMLHandler.GetPath(2));

    //}

    //private void LoadGameData()
    //{
    //    Saving_GameData gameData = mXMLHandler.LoadGameData();
    //    mGameManager.Collected_pickups = gameData.collectablesCollected;
    //    mGameManager.Health = gameData.playerHealth;
    //    mGameManager.SnehettaDead = gameData.snehettaDead;
    //    mGameManager.SnowmanDead = mGameManager.SnowmanDead;
    //    mGameManager.TutorialFinished = gameData.tutorialFinished;

    //    print(mGameManager.Collected_pickups + " pickups");
    //    print(mGameManager.Health + " health");
    //    print(mGameManager.SnehettaDead + " snehetta");
    //    print(mGameManager.SnowmanDead + " snowman");
    //    print(mGameManager.TutorialFinished + " tutorial");

    //    print("\n");
    //    print("---------------------------------------");
    //    print("\n");
    //    for (int i = 0; i < localPuzzles.Length; i++)
    //    {
    //        localPuzzles[i].puzzleName = gameData.puzzles[i].puzzleName;
    //        localPuzzles[i].state = gameData.puzzles[i].state;
    //        print(localPuzzles[i].puzzleName + " is " + localPuzzles[i].state);
    //    }
    //    print("\n");
    //    print("---------------------------------------");
    //    print("\n");
    //    for (int i = 0; i < localAbilities.Length; i++)
    //    {
    //        localAbilities[i].abilityName = gameData.abilities[i].abilityName;
    //        localAbilities[i].state = gameData.abilities[i].state;
    //        print(localAbilities[i].abilityName + " is " + localAbilities[i].state);
    //    }


    //}

    //#endregion

//List to add: Puzzle states.

    #region Pickups

    public static int amountPickedUp = 0;


    List<GameObject> mPickups = new List<GameObject>();

    private void SavePickups()
    {
        GameObject[] objectsInScene = GameObject.FindGameObjectsWithTag("Collectible");

        List<Saving_PickUps> mPickupsList = new List<Saving_PickUps>();

        if (!File.Exists(mXMLHandler.GetPath(0)))
        {
            if(mPickupsList.Count != 0)
                mPickupsList.Clear();

            if (useDebugging)
                print("File does not exists, creating new");

            mPickups.AddRange(objectsInScene);

            mPickups.Sort(delegate (GameObject x, GameObject y) { return x.name.CompareTo(y.name); });


            for (int i = 0; i < mPickups.Count; i++)
            {
                Saving_PickUps pickup = new Saving_PickUps();
                pickup.isPickedUp = !mPickups[i].transform.GetChild(0).gameObject.activeSelf;
                pickup.objectName = mPickups[i].name;
                pickup.spawnPosition = mPickups[i].transform.position;

                mPickupsList.Add(pickup);

                if (useDebugging)
                    print(pickup.objectName + " - Added to the list");
            }

            
        }
        else
        {
            mPickups.Clear();

            mPickupsList = mXMLHandler.LoadPickupListData();
            mPickups.AddRange(objectsInScene);
            mPickups.Sort(delegate (GameObject x, GameObject y) { return x.name.CompareTo(y.name); });

            for (int i = 0; i < mPickups.Count; i++)
            {
                mPickupsList[i].objectName = mPickups[i].name;
                mPickupsList[i].spawnPosition = mPickups[i].transform.position;
                mPickupsList[i].isPickedUp = !mPickups[i].transform.GetChild(0).gameObject.activeSelf;

                if (useDebugging)
                {
                    print("Number " + i + ": " + mPickupsList[i].objectName);
                    print("--||-- " + i + ": " + mPickupsList[i].spawnPosition);
                    print("--||-- " + i + ": " + mPickupsList[i].isPickedUp);
                }

            }
        }
        mXMLHandler.SaveData(mPickupsList, mXMLHandler.GetPath(0));
    }

    private void LoadPickups()
    {
        GameObject[] objectsInScene = GameObject.FindGameObjectsWithTag("Collectible");

        List<Saving_PickUps> mPickupsList = mXMLHandler.LoadPickupListData();

        mPickups.Clear();
        mPickups.AddRange(objectsInScene);
        mPickups.Sort(delegate (GameObject x, GameObject y) { return x.name.CompareTo(y.name); });

        for (int i = 0; i < mPickups.Count; i++)
        {
            mPickups[i].name = mPickupsList[i].objectName;
            mPickups[i].transform.position = mPickupsList[i].spawnPosition;
            mPickups[i].transform.GetChild(0).gameObject.SetActive(!mPickupsList[i].isPickedUp);

            if (useDebugging)
            {
                print("Loaded pickup number: " + i + " - with the name: " + mPickups[i].name);
            }

        }
    }

    #endregion


    #region Player SpawnPoints

    double spawnPointName = 1;

    private void SaveSpawnPoint()
    {

        Saving_SpawnPoint spawnPoint = new Saving_SpawnPoint();
        spawnPoint.name = "SpawnPoint " + spawnPointName.ToString();
        spawnPoint.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        spawnPoint.rotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;
      
        mXMLHandler.SaveData(spawnPoint, mXMLHandler.GetPath(1));

        //spawnPointName++;
    }
    private void LoadSpawnPoint()
    {
        Saving_SpawnPoint loadedSpawnPoint = mXMLHandler.LoadSpawnPointData();

        GameObject.FindGameObjectWithTag("Player").transform.position = loadedSpawnPoint.position;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = loadedSpawnPoint.rotation;

        if (useDebugging)
        {
            print("Player: " + GameObject.FindGameObjectWithTag("Player").transform.position + " | Loaded pos: " + loadedSpawnPoint.position);
            print("Player: " + GameObject.FindGameObjectWithTag("Player").transform.rotation + " | Loaded rot: " + loadedSpawnPoint.rotation);

            print("Loaded: " + loadedSpawnPoint.name);
        }
    }

#endregion
}   