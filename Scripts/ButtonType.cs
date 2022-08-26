using System.Collections;
using UnityEngine;
using SummerInAustralia;

namespace SummerInAustralia.Scripts
{
    public class ButtonType : MonoBehaviour
    {
        public int typeOfButton;
        public Plugin plugin;
        bool hasPraised;

        //Only for grill placement, i dont need to assign this fo other stuff
        public SausageSequence sausageSequence;
        //0 = Start, 1 = plates,2= bread, 3 = Coal, 4 = sausage, 5 = icy poles, 6 = beer, 7 = empty bottle for tomato sauce, 8 = weird remote, 9 = banana, 10 = table placement, 11 = grill placement, 12 = kangaroo ending, 13 = shrine

        void OnTriggerEnter()
        {
            switch (typeOfButton)
            {
                case 0:
                    plugin.StartGame();
                    gameObject.SetActive(false);
                    break;
                case 1:
                    //plates
                    if (!plugin.boughtItems[0])
                    {
                        if (plugin.gamePhase == 1)
                        {
                            if (plugin.money >= 2)
                            {
                                plugin.money -= 2;
                                plugin.handheldsParent.GetChild(0).gameObject.SetActive(true);
                                plugin.boughtItems[0] = true;
                                plugin.hasItem = true;
                                plugin.heldItem = 1;
                                GetComponent<Renderer>().material.color = Color.blue;
                                transform.parent.GetChild(3).gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case 2:
                    //bread
                    if (!plugin.boughtItems[1])
                    {
                        if (plugin.gamePhase == 2)
                        {
                            if (plugin.money >= 3)
                            {
                                plugin.money -= 3;
                                plugin.handheldsParent.GetChild(1).gameObject.SetActive(true);
                                plugin.boughtItems[1] = true;
                                plugin.hasItem = true;
                                plugin.heldItem = 2;
                                GetComponent<Renderer>().material.color = Color.blue;
                                transform.parent.GetChild(3).gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case 3:
                    //coal

                    if (!plugin.boughtItems[2])
                    {
                        if (plugin.gamePhase == 3)
                        {
                            if (plugin.money >= 4)
                            {
                                plugin.money -= 4;
                                plugin.handheldsParent.GetChild(2).gameObject.SetActive(true);
                                plugin.boughtItems[2] = true;
                                plugin.hasItem = true;
                                plugin.heldItem = 3;
                                GetComponent<Renderer>().material.color = Color.blue;
                                transform.parent.GetChild(3).gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case 4:
                    //sausage
                    if (!plugin.boughtItems[3])
                    {
                        if (plugin.gamePhase == 4)
                        {
                            if (plugin.money >= 5)
                            {
                                plugin.money -= 5;
                                plugin.handheldsParent.GetChild(3).gameObject.SetActive(true);
                                plugin.boughtItems[3] = true;
                                plugin.hasItem = true;
                                plugin.heldItem = 4;
                                GetComponent<Renderer>().material.color = Color.blue;
                                transform.parent.GetChild(3).gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case 5:
                    //icy pole
                    if (!plugin.boughtItems[4])
                    {
                        if (plugin.gamePhase == 6)
                        {
                            if (plugin.money >= 5)
                            {
                                plugin.money -= 5;
                                plugin.handheldsParent.GetChild(5).gameObject.SetActive(true);
                                plugin.boughtItems[4] = true;
                                plugin.hasItem = true;
                                plugin.heldItem = 6;
                                GetComponent<Renderer>().material.color = Color.blue;
                                transform.parent.GetChild(3).gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case 6:
                    //beer
                    if (!plugin.boughtItems[5])
                    {
                        if (plugin.gamePhase == 7)
                        {
                            if (plugin.hasID)
                            {
                                if (plugin.money >= 10)
                                {
                                    plugin.money -= 10;
                                    plugin.handheldsParent.GetChild(6).gameObject.SetActive(true);
                                    plugin.handheldsParent.GetChild(8).gameObject.SetActive(false);
                                    plugin.boughtItems[5] = true;
                                    plugin.hasItem = true;
                                    plugin.heldItem = 7;
                                    GetComponent<Renderer>().material.color = Color.blue;
                                    transform.parent.GetChild(3).gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                transform.parent.GetChild(4).gameObject.SetActive(true);
                                plugin.toDoText.text = "BRUH, YOU FORGOT YOUR ID AND NOW YOUR FRIENDS ARE MAD\n\nTHE END";
                                plugin.table.transform.GetChild(7).GetComponent<AudioSource>().Play();
                                plugin.trophyParent.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
                                Invoke("destroyBeerText", 10);
                                plugin.ResetGame(true);
                                plugin.trophiesOwned[5] = true;
                                SaveManager.SaveUserData(plugin);
                            }
                        }
                    }
                    break;
                case 7:
                    //sauce
                    if (!plugin.boughtItems[6])
                    {
                        if (plugin.gamePhase == 8)
                        {
                            if (plugin.money >= 10)
                            {
                                plugin.money -= 10;
                                plugin.handheldsParent.GetChild(7).gameObject.SetActive(true);
                                plugin.boughtItems[6] = true;
                                plugin.hasItem = true;
                                plugin.heldItem = 8;
                                GetComponent<Renderer>().material.color = Color.blue;
                                transform.parent.GetChild(3).gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case 10:
                    if (plugin.hasItem)
                    {
                        switch (plugin.heldItem)
                        {
                            case 1:
                                //plates
                                plugin.unlockParent.GetChild(0).GetChild(0).gameObject.SetActive(false);
                                plugin.unlockParent.GetChild(0).GetChild(1).gameObject.SetActive(true);
                                plugin.unlockParent.GetChild(1).GetChild(0).gameObject.SetActive(true);
                                plugin.heldItem = 0;
                                plugin.hasItem = false;
                                plugin.handheldsParent.GetChild(0).gameObject.SetActive(false);
                                plugin.toDoText.text = "BUY THE BREAD FROM CITY";
                                plugin.gamePhase = 2;
                                break;
                            case 2:
                                //bread
                                plugin.unlockParent.GetChild(1).GetChild(0).gameObject.SetActive(false);
                                plugin.unlockParent.GetChild(1).GetChild(1).gameObject.SetActive(true);
                                plugin.grill.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
                                plugin.heldItem = 0;
                                plugin.hasItem = false;
                                plugin.handheldsParent.GetChild(1).gameObject.SetActive(false);
                                plugin.toDoText.text = "BUY THE COAL FROM CITY";
                                plugin.gamePhase = 3;
                                break;
                            case 5:
                                //cooked sausage
                                plugin.unlockParent.GetChild(2).GetChild(0).gameObject.SetActive(false);
                                plugin.unlockParent.GetChild(2).GetChild(1).gameObject.SetActive(true);
                                plugin.unlockParent.GetChild(3).GetChild(0).gameObject.SetActive(true);
                                plugin.heldItem = 0;
                                plugin.hasItem = false;
                                plugin.handheldsParent.GetChild(4).gameObject.SetActive(false);
                                plugin.toDoText.text = "VISIT THE IRWIN SHRINE IN CAVES TO EARN $25";
                                plugin.gamePhase = 5;
                                break;
                            case 6:
                                //icypole
                                plugin.unlockParent.GetChild(3).GetChild(0).gameObject.SetActive(false);
                                plugin.unlockParent.GetChild(3).GetChild(1).gameObject.SetActive(true);
                                plugin.unlockParent.GetChild(4).GetChild(0).gameObject.SetActive(true);
                                plugin.heldItem = 0;
                                plugin.hasItem = false;
                                plugin.handheldsParent.GetChild(5).gameObject.SetActive(false);
                                plugin.gamePhase = 7;
                                plugin.toDoText.text = "GRAB YOUR ID AND GO BUY SOME BEER";
                                plugin.grill.transform.GetChild(10).gameObject.SetActive(true);
                                break;
                            case 7:
                                //beer
                                plugin.unlockParent.GetChild(4).GetChild(0).gameObject.SetActive(false);
                                plugin.unlockParent.GetChild(4).GetChild(1).gameObject.SetActive(true);
                                plugin.unlockParent.GetChild(5).GetChild(0).gameObject.SetActive(true);
                                plugin.heldItem = 0;
                                plugin.hasItem = false;
                                plugin.handheldsParent.GetChild(6).gameObject.SetActive(false);
                                plugin.gamePhase = 8;
                                plugin.toDoText.text = "BUY THE SAUCE FROM CITY";
                                break;
                            case 8:
                                //sauce
                                plugin.unlockParent.GetChild(5).GetChild(0).gameObject.SetActive(false);
                                plugin.unlockParent.GetChild(5).GetChild(1).gameObject.SetActive(true);
                                plugin.heldItem = 0;
                                plugin.hasItem = false;
                                plugin.handheldsParent.GetChild(7).gameObject.SetActive(false);
                                plugin.gamePhase = 9;
                                plugin.hasStarted = false;
                                if (plugin.trophiesOwned[0] && !plugin.trophiesOwned[1])
                                {
                                    plugin.trophiesOwned[2] = true;
                                    plugin.trophyParent.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                                    plugin.toDoText.text = "Oh no, your friends didnt show up...\n\nTHE END";
                                    plugin.table.transform.GetChild(7).GetComponent<AudioSource>().Play();
                                }
                                else
                                {
                                    plugin.trophiesOwned[0] = true;
                                    plugin.trophyParent.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                                    plugin.toDoText.text = "CONGRATULATIONS, YOU WON.\n\nTHE END";
                                    plugin.table.transform.GetChild(8).GetComponent<AudioSource>().Play();
                                    SaveManager.SaveUserData(plugin);
                                    plugin.ResetGame(true);
                                }
                                break;
                            default:
                                Debug.Log("not a table item");
                                break;
                        }
                    }
                    break;
                case 11:
                    if (plugin.heldItem == 3)
                    {
                        //placing the coal
                        plugin.heldItem = 0;
                        plugin.hasItem = false;
                        plugin.handheldsParent.GetChild(2).gameObject.SetActive(false);
                        plugin.unlockParent.GetChild(2).GetChild(0).gameObject.SetActive(true);
                        sausageSequence.grill.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
                        sausageSequence.grill.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
                        plugin.toDoText.text = "BUY THE RAW SAUSAGE FROM CITY";
                        plugin.gamePhase = 4;
                    }
                    if (plugin.heldItem == 4)
                    {
                        //placin the sausage
                        plugin.heldItem = 0;
                        plugin.hasItem = false;
                        plugin.toDoText.text = "COOK THE SAUSAGES";
                        plugin.handheldsParent.GetChild(3).gameObject.SetActive(false);
                        sausageSequence.StartSequence();
                    }
                    if (sausageSequence.progress > 120 && sausageSequence.progress < 140)
                    {
                        //no more sosig
                        plugin.hasItem = true;
                        plugin.heldItem = 5;
                        plugin.handheldsParent.GetChild(4).gameObject.SetActive(true);
                        sausageSequence.EndSequence();
                        sausageSequence.progress = 0;
                    }
                    else
                    {
                        if( sausageSequence.progress > 150)
                        {
                            plugin.trophyParent.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                            sausageSequence.EndSequence();
                            plugin.table.transform.GetChild(7).GetComponent<AudioSource>().Play();
                            plugin.ResetGame(true);
                            plugin.toDoText.text = "Bruh, you burnt the sausage\n\nTHE END";
                            plugin.trophiesOwned[4] = true;
                            SaveManager.SaveUserData(plugin);
                        }
                    }
                    break;
                case 12:
                    if (plugin.hasStarted)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(destroyKangarooText());
                    }
                    break;
                case 13:
                    if (plugin.gamePhase == 5)
                    {
                        if (!hasPraised)
                        {
                            hasPraised = true;
                            plugin.money += 25;
                            plugin.gamePhase = 6;
                            GetComponent<AudioSource>().Play();
                            plugin.toDoText.text = "BUY THE ICY POLES FROM CITY";
                        }
                    }
                    break;
                case 14:
                    //ID
                    plugin.handheldsParent.GetChild(8).gameObject.SetActive(true);
                    plugin.hasID = true;
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void fard()
        {
            transform.parent.GetChild(4).gameObject.SetActive(true);
            plugin.toDoText.text = "BRUH, YOU FORGOT YOUR ID AND NOW YOUR FRIENDS ARE MAD\n\nTHE END";
            plugin.table.transform.GetChild(7).GetComponent<AudioSource>().Play();
            plugin.trophyParent.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
            plugin.ResetGame(true);
            plugin.trophiesOwned[5] = true;
            SaveManager.SaveUserData(plugin);
        }
        IEnumerator destroyKangarooText()
        {
            yield return new WaitForSeconds(8);
            transform.GetChild(0).gameObject.SetActive(false);
            plugin.trophyParent.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            plugin.trophiesOwned[3] = true;
            SaveManager.SaveUserData(plugin);
        }
        void destroyBeerText()
        {
            transform.parent.GetChild(4).gameObject.SetActive(false);
        }
        IEnumerator resetGame(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}