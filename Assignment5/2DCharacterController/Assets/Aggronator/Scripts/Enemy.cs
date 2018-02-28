using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float attackTimer; //This timer is used to give a space of time between the attacks
    [SerializeField]
    float attackTimerLimit = 1.0f; //This is the minimum time between the attacks (1 sec)
    bool canAttack = true; //This variable will check the attackTimer and will be true when the timer goes to 0
    Quaternion rotation; //This is the rotation of the movement. Not to confuse with the rotation of the object, which is always the same in this example
    float speed = 0.5f; //This is the speed of the movement
    public Sprite alteredEnemySprite;

    LevelManager levelManager; //This is the Level Manager. It contains some important informations about the level and the enemies
    Aggronator aggronator; //This is the aggronator, which is actually what makes the enemies decide who they are going to target and what they will do

    float life = 100f; //Life of the enemy
    float strength = 50.0f; //Strength of the enemy
    List<Enemy> aimedBy = new List<Enemy>(); //This is a list of all of the enemies who are focusing on this enemy; it's used by the Aggronator
    LevelManager.State state = LevelManager.State.passive; //State of the enemy
    Enemy target; //The enemy that will be targeted by this

    float distanceToCheckAllies = 2.0f; //The distance to check for allies; it's used to define the variable below
    int nearAllies = 0; //This is the number of near allies; it's used by the Aggronator

    List<Enemy> listOfAlteredEnemies;
    List<Enemy> listOfPassiveEnemies;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); //Setting the Level Manager
        aggronator = GameObject.FindGameObjectWithTag("Aggronator").GetComponent<Aggronator>(); //Setting the Aggronator
        attackTimer = attackTimerLimit; //Starting the attack timer

        listOfAlteredEnemies = levelManager.getListOfAlteredEnemies();
        listOfPassiveEnemies = levelManager.getListOfPassiveEnemies();
    }

    void Update()
    {
        updateAttackTimer(); //Updating the variable attackTimer

        if (state == LevelManager.State.passive)
        {
            updatePassiveAction(); //If the enemy is passive, do the passive action
        }
        else
        {
            updateAlteredAction(); //If it is altered, do the altered action
        }
    }

    private void updateAttackTimer()
    {
        if (!canAttack) //If the enemy can't attack...
        {
            attackTimer -= Time.deltaTime; //The timer goes down
            if (attackTimer <= 0f) //If the timer zeroed out...
            {
                canAttack = true; //The enemy can attack
                attackTimer = attackTimerLimit; //The timer attack will be reseted
            }
        }
    }

    private void updatePassiveAction()
    {
        if (!levelManager.getAreAlteredEnemiesOnGame())
            return; //If there are no altered enemies in the game, don't do anything

        else //There are altered enemies in the level
        {
            bool enemyChanged = false;
            EnemyValue newEnemyValue; //Creates a new EnemyValue object, then gets all the info from the Aggronator
            newEnemyValue = aggronator.calculatePassiveEnemy(this, listOfAlteredEnemies,
                listOfPassiveEnemies, life, strength, getNearAllies());
            if (target != newEnemyValue.enemy)
            {
                //If the new target is different from the previous, it has to be changed
                if (target != null)
                    target.decreaseAimedBy(this);
                target = newEnemyValue.enemy;
                enemyChanged = true;
            }

            if (newEnemyValue.action < 0f)
            {
                runFromTarget(); //If the 'action' float is below 0, this enemy is going to run from its target
            }
            else
            {
                goToTarget(enemyChanged); //Else, this enemy is going to run to its target and try to attack it
            }
        }
    }

    private void runFromTarget()
    {
        //Gets the angle between this enemy and the target, and make the 'rotation' variable point to the other side
        Vector2 distanceVector = target.transform.position - transform.position;
        float ang = Vector2.Angle(distanceVector, Vector2.down);
        Vector3 cross = Vector3.Cross(distanceVector, Vector2.down);
        if (cross.z > 0)
            ang = 360 - ang;
        rotation = Quaternion.Euler(new Vector3(0f, 0f, ang));

        //Tries to run from the target
        bool checking = true;
        Vector2 checkingVector = Vector2.up;
        while (checking)
        {
            //Tries to run in the directions: up, right and left
            //If the enemy can't go to any of these directions, don't do anything
            //The enemy can't go more than 5 meters from the center of the level, to avoid
            //enemies going too far away from each other
            if (Vector2.Distance(transform.position + rotation * checkingVector * Time.deltaTime * speed, Vector2.zero) < (5.0f - transform.localScale.x))
            {
                transform.Translate(rotation * checkingVector * Time.deltaTime * speed);
                break;
            }
            if (checkingVector == Vector2.up)
                checkingVector = Vector2.right;
            else if (checkingVector == Vector2.right)
                checkingVector = Vector2.left;
            else
                checking = false;
        }
    }

    private void goToTarget(bool enemyChanged)
    {
        if (enemyChanged)
            target.increaseAimedBy(this);

        if (Vector2.Distance(target.transform.position, transform.position) < 1.05f && canAttack)
        {
            //If the target is at a small distance, and this can attack, then it attacks
            if (target.setDamage(Random.Range(0.4f * strength, 0.7f * strength)) <= 0f)
            {
                strength += 10f;
            }
            canAttack = false;
        }
        else
        {
            //Otherwise, move to the target
            Vector2 distanceVector = target.transform.position - transform.position;
            float ang = Vector2.Angle(distanceVector, Vector2.up);
            Vector3 cross = Vector3.Cross(distanceVector, Vector2.up);
            if (cross.z > 0)
                ang = 360 - ang;
            rotation = Quaternion.Euler(new Vector3(0f, 0f, ang));

            if (Vector2.Distance(transform.position + rotation * Vector2.up * Time.deltaTime * speed, Vector2.zero) < (5.0f - transform.localScale.x))
            { //If it's not going too far away from the center, moves
                transform.Translate(rotation * Vector2.up * Time.deltaTime * speed);
            }
        }
    }

    private void updateAlteredAction()
    {
        //This function is almost the same as updatePassiveAction,
        //with only some differences in the actions
        if (!levelManager.getArePassiveEnemiesOnGame())
            return;
        
        bool enemyChanged = false;
        EnemyValue newEnemyValue;
        newEnemyValue = aggronator.calculateAlteredEnemy(this, listOfAlteredEnemies, listOfPassiveEnemies, life, strength);
        if (target != newEnemyValue.enemy)
        {
            if (target != null)
                target.decreaseAimedBy(this);
            target = newEnemyValue.enemy;
            enemyChanged = true;
        }

        if (newEnemyValue.action < 0f && listOfAlteredEnemies.Count > 1)
        {
            //The main difference between an altered and a passive enemy is that the
            //altered regroups when 'action' is below zero, instead of running
            regroup();
        }
        else
        {
            goToTarget(enemyChanged);
        }
    }

    private void regroup()
    {
        //This function is called when an altered enemy has an 'action' below zero
        //Tries to regroup with another altered enemy instead of attacking
        clearTarget();
        Enemy ally = null;
        int nowAllies = -1;
        for (int a = 0; a < listOfAlteredEnemies.Count; a++)
        {
            if (listOfAlteredEnemies[a].getNearAllies() > nowAllies)
            {
                nowAllies = listOfAlteredEnemies[a].getNearAllies();
                ally = listOfAlteredEnemies[a];
            }
        }
    }

    public void changeStateToAltered()
    {
        //If the enemy is already altered, there's nothing to do...
        if (state == LevelManager.State.altered)
            return;

        levelManager.changeStateToAltered(this); //Sends the information about the change to the Aggronator
        state = LevelManager.State.altered; //Changes the local state
        clearTarget();
        gameObject.GetComponent<SpriteRenderer>().sprite = alteredEnemySprite; //Changes the sprite
        speed = 0.6f; //Increases the speed, so the altered enemies are a bit faster than the passive ones
        canAttack = false; //Avoids attacking immediatly after changing the state
        attackTimer = attackTimerLimit; //Restarts the attack timer
    }

    public float setDamage(float amount)
    {
        //Reduces the life by an amount and return the life
        life -= amount;
        if (life <= 0f)
            killSelf(); //If life is equal or below 0, die
        return life;
    }

    private void killSelf()
    {
        //If this enemy has a target, then remove this from the Aimed By of the target
        clearTarget();
        levelManager.notifyDeath(this); //Informs to the Aggronator that this enemy doesn't exist anymore
        Destroy(gameObject); //Destroy the gameobject
    }

    //Changes 'target' to null and also removes this from the list of enemies targeting
    //the target
    private void clearTarget()
    {
        if (target == null)
            return;

        target.decreaseAimedBy(this);
        target = null;
    }

    //Getting the near allies of this
    public int getNearAllies()
    {
        Collider2D[] allies =
            Physics2D.OverlapCircleAll(transform.position, distanceToCheckAllies);
        nearAllies = 0;
        for (int a = 0; a < allies.Length; a++)
        {
            if (allies[a].CompareTag("Enemy"))
            {
                if (allies[a].gameObject.GetComponent<Enemy>())
                {
                    if (allies[a].gameObject.GetComponent<Enemy>().getState() == LevelManager.State.passive)
                    {
                        nearAllies++;
                    }
                }
            }
        }
        return nearAllies;
    }

    private void OnMouseDown()
    {
        changeStateToAltered(); //Changes the state of the enemy when it's clicked
    }

    //Called whenever an enemy starts targeting this
    public void increaseAimedBy(Enemy followingEnemy)
    {
        if (!aimedBy.Contains(followingEnemy))
            aimedBy.Add(followingEnemy);
    }

    //Called whenever an enemy stops targeting this
    public void decreaseAimedBy(Enemy followingEnemy)
    {
        aimedBy.Remove(followingEnemy);
    }

    public LevelManager.State getState() { return state; }
    public float getLife() { return life; }
    public float getStrength() { return strength; }
    public List<Enemy> getAimedBy() { return aimedBy; }
    public Enemy getTarget() { return target; }
}
