using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Utilla;
using SummerInAustralia.Scripts;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;

namespace SummerInAustralia
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        bool inRoom;
        bool ticking;
        public bool hasStarted;
        public bool[] trophiesOwned;
        public int money = 5;
        ButtonType shrineTrigger;

        int timeSinceCountdown = 650;

        //0 = nothing, 1 = plate, 2 = bread, 3 = coal, 4 = sausage, 5 = cooked sausage,6 = Icy poles, 7 = Beer, , 8 = tomato sauce
        public int heldItem;
        public bool hasItem;

        //0 = not started, 1 = waiting to place plates, 2 = waiting for bread, 3 = waiting for coal, 4 = sausage cookin, 5 = waiting for shrine, 6 = waiting for icypoles, 7 = waiting for beer, 8 = waiting for sauce, 9 = done
        public int gamePhase;

        public bool hasID = false;

        bool hasCalledEnd;

        public bool[] boughtItems;

        public GameObject objectsParent;
        public Transform handheldsParent;
        public Text toDoText;
        GameObject positions;
        GameObject moneyPrefab;
        public GameObject table;
        public GameObject grill;
        public GameObject trophyParent;
        GameObject purchaseBoxes;

        Text MoneyText1;    
        Text TimeText;    

        public Transform unlockParent;
        public Transform grillUnlocks;
        //the ghost parent holds the objects that you place on the table, it also holds the ghost variants of those objects

        Text timerText;

        AssetBundle ObjectsBundle;

        void Start()
        {
            Instance = this;
            Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            if (inRoom)
            {
                ShowAll();
            }
        }

        void OnDisable()
        {
            RemoveAll();
            ResetGame(false);
            EnableNetworkTriggers();
            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            boughtItems = new bool[7];
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("AustralianSummerMod.Assets.ausobjects");
            ObjectsBundle = AssetBundle.LoadFromStream(str);

            objectsParent = new GameObject();
            objectsParent.name = "AustraliaModParent";

            table = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(0).gameObject);
            table.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            table.transform.position = new Vector3(-48, 2.35f, -58);
            table.transform.SetParent(objectsParent.transform, false);
            unlockParent = table.transform.GetChild(5);

            GameObject toDoBox = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(7).gameObject);
            toDoBox.transform.position = new Vector3(-49, 2.4f, -56);
            toDoBox.transform.eulerAngles = new Vector3(270, 270, 0);
            toDoBox.transform.SetParent(objectsParent.transform, false);
            toDoText = toDoBox.transform.GetChild(0).GetChild(1).GetComponent<Text>();

            timerText = table.transform.GetChild(4).GetChild(0).GetComponent<Text>();
            timerText.gameObject.AddComponent<FacePlayer>();

            grill = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(4).gameObject);
            grill.transform.position = new Vector3(-51, 2.38f, -57);
            grill.transform.eulerAngles = new Vector3(0, -80, 0);
            grill.transform.SetParent(objectsParent.transform, false);

            grillUnlocks = grill.transform.GetChild(5);

            ButtonType idItem =  grill.transform.GetChild(10).gameObject.AddComponent<ButtonType>();
            idItem.typeOfButton = 14;
            idItem.plugin = this;

            GameObject MoneyAndTimer = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(9).gameObject);
            TimeText = MoneyAndTimer.transform.GetChild(0).GetChild(2).GetComponent<Text>();
            MoneyText1 = MoneyAndTimer.transform.GetChild(0).GetChild(1).GetComponent<Text>();
            MoneyAndTimer.transform.SetParent(objectsParent.transform, false);

            GameObject shrine = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(8).gameObject);
            shrine.transform.position = new Vector3(-61.5f, -7.6f, -61.3f);
            shrine.transform.eulerAngles = new Vector3(-90, 0, -70);
            shrine.transform.SetParent(objectsParent.transform, false);
            shrineTrigger = shrine.AddComponent<ButtonType>();
            shrineTrigger.typeOfButton = 13;
            shrineTrigger.plugin = this;

            handheldsParent = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(5).gameObject).transform;
            handheldsParent.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent.Find("palm.01.R"), false);
            handheldsParent.transform.localPosition = Vector3.zero;

            purchaseBoxes = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(6).gameObject);
            ButtonType plateButton = purchaseBoxes.transform.GetChild(0).GetChild(0).gameObject.AddComponent<ButtonType>();
            plateButton.plugin = this;
            plateButton.typeOfButton = 1;
            ButtonType breadButton = purchaseBoxes.transform.GetChild(1).GetChild(0).gameObject.AddComponent<ButtonType>();
            breadButton.plugin = this;
            breadButton.typeOfButton = 2;
            ButtonType coalButton = purchaseBoxes.transform.GetChild(2).GetChild(0).gameObject.AddComponent<ButtonType>();
            coalButton.plugin = this;
            coalButton.typeOfButton = 3;
            ButtonType sausageButton = purchaseBoxes.transform.GetChild(3).GetChild(0).gameObject.AddComponent<ButtonType>();
            sausageButton.plugin = this;
            sausageButton.typeOfButton = 4;
            ButtonType icypoleButton = purchaseBoxes.transform.GetChild(4).GetChild(0).gameObject.AddComponent<ButtonType>();
            icypoleButton.plugin = this;
            icypoleButton.typeOfButton = 5;
            ButtonType beerButton = purchaseBoxes.transform.GetChild(5).GetChild(0).gameObject.AddComponent<ButtonType>();
            beerButton.plugin = this;
            beerButton.typeOfButton = 6;
            ButtonType sauceButton = purchaseBoxes.transform.GetChild(6).GetChild(0).gameObject.AddComponent<ButtonType>();
            sauceButton.plugin = this;
            sauceButton.typeOfButton = 7;

            trophyParent = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(10).gameObject);
            trophyParent.transform.position = new Vector3(-66,11.9f,-85);
            trophyParent.transform.SetParent(objectsParent.transform, false);

            GameObject kangaroo = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(1).gameObject);
            kangaroo.transform.eulerAngles = new Vector3(270, 140, 0);
            kangaroo.transform.position = new Vector3(-110.5f, 12.4f, -127);
            kangaroo.transform.SetParent(objectsParent.transform, false);
            positions = Instantiate(ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(2).gameObject);
            positions.transform.SetParent(objectsParent.transform, false);
            moneyPrefab = ObjectsBundle.LoadAsset<GameObject>("Objects").transform.GetChild(3).gameObject;

            ButtonType startGame = table.transform.GetChild(3).gameObject.AddComponent<ButtonType>();
            startGame.typeOfButton = 0;
            startGame.plugin = this;

            ButtonType tableInteraction = table.transform.GetChild(6).gameObject.AddComponent<ButtonType>();
            tableInteraction.typeOfButton = 10;
            tableInteraction.plugin = this;

            ButtonType grillInteraction = grill.transform.GetChild(8).gameObject.AddComponent<ButtonType>();
            grillInteraction.typeOfButton = 11;
            grillInteraction.plugin = this;
            grillInteraction.sausageSequence = grill.transform.GetChild(6).gameObject.AddComponent<SausageSequence>();
            grillInteraction.sausageSequence.grill = grill;

            ButtonType kangarooButton = kangaroo.AddComponent<ButtonType>();
            kangarooButton.typeOfButton = 12;
            kangarooButton.plugin = this;

            purchaseBoxes.gameObject.SetActive(false);
            objectsParent.gameObject.SetActive(false);

            SaveData data = SaveManager.LoadUserData();
            if (data.trophies != null)
            {
                trophiesOwned = data.trophies;

                for(int i = 0; i < trophiesOwned.Length; i++)
                {
                    if (trophyParent.transform.GetChild(0).GetChild(i).gameObject != null)
                    {
                        trophyParent.transform.GetChild(0).GetChild(i).gameObject.SetActive(trophiesOwned[i]);
                    }
                }
            }
            else
            {
                trophiesOwned = new bool[6];
            }
        }

        void Update()
        {
            if (hasStarted)
            {
                if (ticking == false && timeSinceCountdown > 0)
                {
                    StartCoroutine(SecondTick());
                }
            }
            if(timeSinceCountdown == 0)
            {
                toDoText.text = "YOU TOOK TOO LONG TO PREPARE THE BARBECUE \n\n THE END";
                 table.transform.GetChild(7).GetComponent<AudioSource>().Play();
                if (hasStarted)
                {
                    hasStarted = false;
                    trophyParent.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    ResetGame(true);
                    trophiesOwned[1] = true;
                    SaveManager.SaveUserData(this);
                    timeSinceCountdown = 650;
                }

            }

            if (MoneyText1 == null || TimeText == null)
                return;

            MoneyText1.text = "MONEY: " + money.ToString();
            TimeText.text = "TIME: " + timeSinceCountdown.ToString();
        }

        IEnumerator SecondTick()
        {
            ticking = true;
            yield return new WaitForSeconds(1);
            timeSinceCountdown -= 1;
            Debug.Log(timeSinceCountdown);
            timerText.text = timeSinceCountdown.ToString();
            timerText.color = Color.white;
            if (timeSinceCountdown <= 10)
                timerText.color = Color.red;
            ticking = false;
        }

        public void StartGame()
        {
            if (inRoom)
            {
                foreach (Transform t in positions.transform)
                {
                    GameObject moneyObj = Instantiate(moneyPrefab);
                    moneyObj.transform.SetParent(t, false);
                    moneyObj.transform.localEulerAngles = new Vector3(90, UnityEngine.Random.Range(0, 360), 0);
                    MoneyItem moneyItem = moneyObj.AddComponent<MoneyItem>();
                    moneyItem.plugin = this;
                }
                DisableNetworkTriggers();
                toDoText.text = "COLLECT MONEY AROUND THE MAP, THEN FIND AND BUY THE PLATES FROM THE CITY MAP";
                table.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                table.transform.GetChild(2).GetComponent<AudioSource>().Play();
                unlockParent.GetChild(0).GetChild(0).gameObject.SetActive(true);
                purchaseBoxes.SetActive(true);
                timeSinceCountdown = 650;
                hasStarted = true;
                hasID = false;
                gamePhase = 1;
            }
        }
        public void ResetGame(bool isSlowReset)
        {
            boughtItems = new bool[7];
            
            timeSinceCountdown = 650;
            money = 0;
            hasStarted = false;
            gamePhase = 0;
            grill.transform.GetChild(10).gameObject.SetActive(false);
            #region Box And Ghost Cleanup
            if (isSlowReset)
            {
                Invoke("resetGame", 6);
                grill.transform.GetChild(6).gameObject.GetComponent<SausageSequence>().knob.transform.localEulerAngles = new Vector3(-90, 0, 0);
            }
            else
            {
                foreach (Transform t in positions.transform)
                {
                    if (t.GetChild(0).gameObject != null)
                    {
                        Destroy(t.GetChild(0).gameObject);
                    }
                }
                foreach (Transform child in purchaseBoxes.transform)
                {
                    if (!child.GetChild(3).gameObject.activeSelf)
                    {
                        child.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                        child.GetChild(3).gameObject.SetActive(true);
                    }
                }
                foreach (Transform child in unlockParent.transform)
                {
                    child.GetChild(0).gameObject.SetActive(false);
                    child.GetChild(1).gameObject.SetActive(false);
                }
                foreach (Transform child in handheldsParent.transform)
                {
                    if (child.gameObject != null & child.gameObject.activeSelf)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                grill.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
                grill.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
                grill.transform.GetChild(6).gameObject.GetComponent<SausageSequence>().EndSequence();
                grill.transform.GetChild(6).gameObject.GetComponent<SausageSequence>().knob.transform.eulerAngles = new Vector3(-90, 0, 0);
                grill.transform.GetChild(6).gameObject.GetComponent<SausageSequence>().progress = 0;
                table.transform.GetChild(3).gameObject.SetActive(true);
                shrineTrigger.hasPraised = false;
                toDoText.text = "PRESS THE RED BUTTON TO START \n\nPLAY IN A PRIVATE ROOM";
                timerText.text = "650";
                timerText.color = Color.white;
                hasID = false;
            }
            hasCalledEnd = false;
            #endregion
        }
        public void RemoveAll()
        {
            if (purchaseBoxes != null && objectsParent != null)
            {
                purchaseBoxes.gameObject.SetActive(false);
                objectsParent.gameObject.SetActive(false);
                ResetGame(false);
            }
        }
        public void ShowAll()
        {
            purchaseBoxes.gameObject.SetActive(true);
            objectsParent.gameObject.SetActive(true);
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            inRoom = true;
            if (!PhotonNetwork.CurrentRoom.IsVisible)
                ShowAll();
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            EnableNetworkTriggers();
            RemoveAll();
            ResetGame(false);

            inRoom = false;
        }

        void DisableNetworkTriggers()
        {
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - Forest, Tree Exit").SetActive(false);
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - Cave").SetActive(false);
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - Canyon").SetActive(false);
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - City Front").SetActive(false);
        }
        void EnableNetworkTriggers()
        {
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - Forest, Tree Exit").SetActive(true);
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - Cave").SetActive(true);
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - Canyon").SetActive(true);
            GameObject.Find("NetworkTriggers/Networking Trigger/JoinPublicRoom - City Front").SetActive(true);
        }

        void resetGame()
        {
            Debug.Log("startin and fartin");
            foreach (Transform t in positions.transform)
            {
                if (t.childCount != 0)
                {
                    Debug.Log("Found Child");
                    Destroy(t.GetChild(0).gameObject);
                    Debug.Log("Did destroy");
                }
            }
            Debug.Log("Made it past money destroy");
            
            foreach (Transform child in purchaseBoxes.transform)
            {
                if (!child.GetChild(3).gameObject.activeSelf && child != null)
                {
                    child.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                    child.GetChild(3).gameObject.SetActive(true);
                }
            }
            Debug.Log("Made it past purchase box destroy");
            foreach (Transform child in unlockParent.transform)
            {
                if(child.GetChild(0) != null && child.GetChild(1) != null)
                child.GetChild(0).gameObject.SetActive(false);
                child.GetChild(1).gameObject.SetActive(false);
            }
            Debug.Log("Made it past unlock parent");
            foreach (Transform child in handheldsParent.transform)
            {
                if (child.gameObject != null & child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
            }
            Debug.Log("Made it past handheld fixes");
            table.transform.GetChild(3).gameObject.SetActive(true);
            Debug.Log("Made it past button");
            grill.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
            grill.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
            Debug.Log("Made it past sosig destroy");
            grill.transform.GetChild(6).gameObject.GetComponent<SausageSequence>().progress = 0;
            Debug.Log("Made it past reset sequence");
            toDoText.text = "PRESS THE RED BUTTON TO START \n\nPLAY IN A PRIVATE ROOM";
            timerText.text = "650";
            timerText.color = Color.white;
            shrineTrigger.hasPraised = false;
            hasID = false;
        }
    }
}
