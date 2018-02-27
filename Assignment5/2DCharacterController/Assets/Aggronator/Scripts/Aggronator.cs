using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyValue
{
    //An object of this class will be returned whenever an enemy calls for
    //a target to the Aggronator. It has two informations:
    //1 - The target itself
    //2 - The action to be chosen by the enemy (attack? run? regroup?)
    public Enemy enemy;
    public float action = 0f;
}

public class Aggronator : MonoBehaviour
{
    public EnemyValue calculatePassiveEnemy(Enemy enemy, List<Enemy> listOfAlteredEnemies, List<Enemy> listOfPassiveEnemies, float life, float strength, float nearAllies)
    {
        Enemy targetedEnemy = null; //This will be the target of the current enemy
        float targetedEnemyValue = Mathf.NegativeInfinity; //This float will be used to check what enemy will be targeted; the enemy with bigger value will be targeted
        for (int e = 0; e < listOfAlteredEnemies.Count; e++) //Iterates over the list of altered enemies to check the target
        {
            float distance = Vector2.Distance(listOfAlteredEnemies[e].transform.position, enemy.transform.position); //The distance from the enemy to the possible target

            //The lines below will call the function calculatePassiveTarget, which is
            //responsible for getting a numeric value to decide the target. You can see
            //more info about what is happening on the function itself
            float currentEnemyValue = calculatePassiveTarget(distance, enemy.getAimedBy().Count,
                listOfPassiveEnemies.Count, listOfAlteredEnemies.Count, nearAllies, life,
                listOfAlteredEnemies[e].getLife(), strength, listOfAlteredEnemies[e].getStrength());

            if (currentEnemyValue > targetedEnemyValue)
            {
                //If the enemy that is being analyzed now has a bigger value than the current
                //target, it will become the target
                targetedEnemy = listOfAlteredEnemies[e];
                targetedEnemyValue = currentEnemyValue;
            }
        }
        
        EnemyValue newEnemyValue = new EnemyValue();
        newEnemyValue.enemy = targetedEnemy; //After checking all of the enemies, we can decide our target

        //The lines below will call the function calculatePassiveAction, which is
        //responsible for getting a numeric value to decide what will be the action of
        //this enemy. You can see more info about what is happening on the function itself
        newEnemyValue.action = calculatePassiveAction(enemy.getAimedBy().Count, listOfPassiveEnemies.Count,
                listOfAlteredEnemies.Count, nearAllies, life, targetedEnemy.getLife(),
                strength, targetedEnemy.getStrength());

        return newEnemyValue; //Returns the target and the action to the enemy
    }

    //This is the function responsible for returning the value of the target.
    //The enemy with the higher value will be selected as the target.
    //You can change the parameters of this function according to your needs.
    //You can also change the weights of the variables on the return line to
    //modify how much each of the parameters is going to influence the value.
    private float calculatePassiveTarget(float distance, float aimedBy, int numberOfPassive, int numberOfActive,
        float nearAllies, float ownLife, float enemyLife, float ownStrength, float enemyStrength)
    {
        float life = ownLife - enemyLife; //Life is the difference between the enemy life and the target life; the more life, the more an enemy is going to want to attack another enemy (as he will have the life advantage over the other enemy)
        float strength = ownStrength - enemyStrength; //Same thing as the life, but with the strength
        
        return (distance / -5f) + (aimedBy * 1f) + (life / 100f) + (strength / 100f); //Change the numbers to modify how much each parameter is going to influence the value
    }

    //This is the function responsible for returning the value of the passive.
    //If the returned value is above or equal to zero, it is going to attack;
    //otherwise, it's going to run.
    //As well as the above function, you can change the parameters and the weights
    //according to your needs
    private float calculatePassiveAction(float aimedBy, int numberOfPassive, int numberOfActive,
        float nearAllies, float ownLife, float enemyLife, float ownStrength, float enemyStrength)
    {
        float life = ownLife - enemyLife;
        float strength = ownStrength - enemyStrength;
        
        return (-aimedBy * 2f) + (life / 100f) + (strength / 100f) + (nearAllies * 2f);
    }

    //Almost the same thing as 'calculatePassiveEnemy', with some differences in the parameters
    public EnemyValue calculateAlteredEnemy(Enemy enemy, List<Enemy> listOfAlteredEnemies, List<Enemy> listOfPassiveEnemies, float life, float strength)
    {
        Enemy targetedEnemy = null;
        float targetedEnemyValue = Mathf.NegativeInfinity;
        for (int e = 0; e < listOfPassiveEnemies.Count; e++)
        {
            float distance = Vector2.Distance(listOfPassiveEnemies[e].transform.position, enemy.transform.position);
            float currentEnemyValue = calculateAlteredTarget(enemy, distance, listOfPassiveEnemies[e].getAimedBy(),
                listOfAlteredEnemies.Count, life, listOfPassiveEnemies[e].getLife(), strength, listOfPassiveEnemies[e].getStrength());

            if (currentEnemyValue > targetedEnemyValue)
            {
                targetedEnemy = listOfPassiveEnemies[e];
                targetedEnemyValue = currentEnemyValue;
            }
        }

        EnemyValue newEnemyValue = new EnemyValue();
        newEnemyValue.enemy = targetedEnemy;
        newEnemyValue.action = calculateAlteredAction(enemy, targetedEnemy.getAimedBy(), life,
                targetedEnemy.getLife(), strength, targetedEnemy.getStrength());

        return newEnemyValue;
    }

    //Almost the same thing as 'calculatePassiveTarget', with some differences in the parameters
    private float calculateAlteredTarget(Enemy thisEnemy, float distance, List<Enemy> passiveAimedBy,
        int numberOfActive, float ownLife, float enemyLife, float ownStrength, float enemyStrength)
    {
        float life = ownLife - enemyLife;
        float strength = ownStrength - enemyStrength;

        float numberPassiveAimedBy = passiveAimedBy.Count;
        if (passiveAimedBy.Contains(thisEnemy))
            numberPassiveAimedBy--;

        return (distance / -5f) + (numberPassiveAimedBy / 10f) + (life / 100f) + (strength / 100f);
    }

    //Almost the same thing as 'calculatePassiveAction', with some differences in the parameters
    private float calculateAlteredAction(Enemy thisEnemy, List<Enemy> passiveAimedBy,
        float ownLife, float enemyLife, float ownStrength, float enemyStrength)
    {
        float life = ownLife - enemyLife;
        float strength = ownStrength - enemyStrength;

        float numberPassiveAimedBy = passiveAimedBy.Count;
        if (passiveAimedBy.Contains(thisEnemy))
            numberPassiveAimedBy--;

        return numberPassiveAimedBy + (life / 100f) + (strength / 100f);
    }
}
