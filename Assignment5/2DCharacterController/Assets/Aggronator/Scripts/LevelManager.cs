using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    List<Enemy> listOfPassiveEnemies = new List<Enemy>(); //List of all passive enemies on the scene
    List<Enemy> listOfAlteredEnemies = new List<Enemy>(); //List of all altered enemies on the scene
    bool arePassiveEnemiesOnGame = false; //Boolean that checks if there are any passive enemies on the scene
    bool areAlteredEnemiesOnGame = false; //Boolean that checks if there are any altered enemies on the scene

    public enum State { passive, altered }; //enum used by the enemies that will indicate their state

    void Start()
	{
        //Creation of the lists of enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int e = 0; e < enemies.Length; e++)
        {
            if (enemies[e].GetComponent<Enemy>())
            {
                if (enemies[e].GetComponent<Enemy>().getState() == State.passive)
                {
                    listOfPassiveEnemies.Add(enemies[e].GetComponent<Enemy>());
                    arePassiveEnemiesOnGame = true;
                }
                else
                {
                    listOfAlteredEnemies.Add(enemies[e].GetComponent<Enemy>());
                    areAlteredEnemiesOnGame = true;
                }
            }
        }
    }

    //This function is called when an enemy dies. It is removed by the list
    //of enemies 
    public void notifyDeath(Enemy enemy)
    {
        if (enemy.getState() == State.passive)
        {
            listOfPassiveEnemies.Remove(enemy);
            verifyPassiveEnemies();
        }
        else
        {
            listOfAlteredEnemies.Remove(enemy);
            verifyAlteredEnemies();
        }
    }

    //This function changes the state of the enemies from passive to altered
    //It removes them from the list of Passive and adds them to the list of Altered
    //Also changes areAlteredEnemiesOnGame to true
    public void changeStateToAltered(Enemy enemy)
    {
        if (enemy.getState() == State.altered)
            return;

        listOfPassiveEnemies.Remove(enemy);
        listOfAlteredEnemies.Add(enemy);
        
        areAlteredEnemiesOnGame = true;

        verifyPassiveEnemies();
    }

    //If there are no passive enemies on the list, change arePassiveEnemiesOnGame
    //to false
    private void verifyPassiveEnemies()
    {
        if (listOfPassiveEnemies.Count == 0)
        {
            arePassiveEnemiesOnGame = false;
        }
    }

    //If there are no altered enemies on the list, change areAlteredEnemiesOnGame
    //to false
    private void verifyAlteredEnemies()
    {
        if (listOfAlteredEnemies.Count == 0)
        {
            areAlteredEnemiesOnGame = false;
        }
    }

    public List<Enemy> getListOfAlteredEnemies() { return listOfAlteredEnemies; }
    public List<Enemy> getListOfPassiveEnemies() { return listOfPassiveEnemies; }
    public bool getArePassiveEnemiesOnGame() { return arePassiveEnemiesOnGame; }
    public bool getAreAlteredEnemiesOnGame() { return areAlteredEnemiesOnGame; }
}
