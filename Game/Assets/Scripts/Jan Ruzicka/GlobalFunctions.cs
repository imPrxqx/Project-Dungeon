using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalFunctions : MonoBehaviour
{
       //Function returns the random time between min and max value
    public static float WaitTime(float minTime, float maxTime)
    {
        float waitTime = Random.Range(minTime, maxTime);

        return waitTime;
    }
    //Function returns distance from the player and the gameobject in float value
    public static float DistanceRange(Transform player, Transform transform)
    {
        float distanceLength = GetPositionDistance(player, transform).magnitude;

        return distanceLength;
    }
    //Function returns distance from the player and the gameobject in vector value
    public static Vector3 GetPositionDistance(Transform player, Transform transform)
    {  
        Vector3 distance = player.position - transform.position;

        return distance;
    }
    //Function returns rotation from the gameobject in vector value
    public static Vector3 GetRotation(Transform transform)
    {
        float x;
        float y;
        float z;
        
        if (transform.localEulerAngles.x <= 180f)
        {
            x = transform.eulerAngles.x;
        }
        else
        {
            x = transform.eulerAngles.x - 360f;
        }

        if (transform.localEulerAngles.y <= 180f)
        {
            y = transform.eulerAngles.y;
        }
        else
        {
            y = transform.eulerAngles.y - 360f;
        }

        if (transform.localEulerAngles.z <= 180f)
        {
            z = transform.eulerAngles.z;
        }
        else
        {
            z = transform.eulerAngles.z - 360f;
        }

        Vector3 rotation = new Vector3(x, y, z);

        return rotation;
    }
    //Function returns calculated the boss health in percentile value 
    public static float GetBossHealth(float health, float maxHealth)
    {
        float healthper = health / maxHealth;

        return healthper;
    }    
    //Function returns random value between 0 and max value
    public static int RandomValue(int maxValue)
    {
        int value = Random.Range(0, maxValue);

        return value;
    } 
    //Function returns bool value true of false on based min and max chance
    public static bool GambleBool(float minChance, float maxChance)
    {
        float chance = minChance/maxChance;
        float value = Random.Range(0.0f, 1.0f);

        if(chance >= value)
        {
            return true;
        } else
        {
            return false;
        }
    }
    //Todo: matematicke vzorce pro prubeh hry
    public static (float, float, int, int) CalcEnemy(int tierEnemy, float defaultHealth, float defaultDamage, int defaultXp, int defaultMoney)
    {
        float health = (Mathf.Pow(1.2f, GameData.level) * tierEnemy + defaultHealth);
        float damage = (Mathf.Pow(1.2f, GameData.level) * tierEnemy + defaultDamage);
        int xp = (int)(Mathf.Pow(1.2f, GameData.level) * (tierEnemy + defaultXp));
        int money = (int)(GameData.level * (tierEnemy + defaultXp));

        return (health, damage, xp, money);
    }

    public static (float, float) CalcPlayer(float defaultHealth, float defaultMana)
    {
        float health = (GameData.level * defaultHealth * 0.75f);
        float mana = (GameData.level * defaultMana);

        return (health, mana);
    }

    public static (int, int) CalcLoot(int tierLoot, int defaultXp, int defaultMoney)
    {
        int xp = (int)(Mathf.Pow(1.2f, GameData.level) * tierLoot * 1.1f + defaultXp);
        int money = (int)(Mathf.Pow(1.2f, GameData.level) + tierLoot * 1.5f + defaultMoney);

        return (xp, money);
    }

    public static int CalcTrap(int tierTrap, float defaultDamage)
    {
        int damage = (int)(Mathf.Pow(1.2f, GameData.level) * tierTrap * 1.1f + defaultDamage);

        return damage;
    }
}
