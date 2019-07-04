using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleLogic {
    /// <summary>
    /// Do a regular attack on an enemy
    /// </summary>
    /// <param name="charName">Name of the character attacking</param>
    /// <param name="enemy">Enemy that is being attacked</param>
    /// <returns></returns>
    public static int RegularAttack(string charName, Enemy enemy) {
        int damage = GameManager.Instance.Party.GetCharAttk(charName) - enemy.Defense;

        if (enemy.IsDefending) {
            // if enemy is defending, do half damage
            damage = Mathf.RoundToInt(damage / 2);
        }

        damage = Math.Max(damage, 1); // always do at least 1 damage

        enemy.CurrentHP -= damage;
        enemy.CurrentHP = Math.Max(enemy.CurrentHP, 0); // lowest amount is 0
        
        return damage;
    }

    /// <summary>
    /// Use a healing item
    /// </summary>
    /// <param name="charName">Character being healed</param>
    /// <param name="item">Item being used</param>
    /// <returns></returns>
    public static int UseHealingItem(string charName, Items item) {
        int amountHealed = 0;

        int curr = 0;
        if (item.AffectHP) {
            curr = GameManager.Instance.Party.GetCharCurrHP(charName);
        } else if (item.AffectSP) {
            curr = GameManager.Instance.Party.GetCharCurrSP(charName);
        }

        item.Use(charName);

        if (item.AffectHP) {
            amountHealed = GameManager.Instance.Party.GetCharCurrHP(charName) - curr;
        } else if (item.AffectSP) {
            amountHealed = GameManager.Instance.Party.GetCharCurrSP(charName) - curr;
        }

        return amountHealed;
    }

    /// <summary>
    /// Use attack skill on an enemy
    /// </summary>
    /// <param name="charName">Character using attack skill</param>
    /// <param name="enemy">Enemy being attacked</param>
    /// <param name="skill">Skill being used</param>
    /// <returns></returns>
    public static int UseAttackSkill(string charName, Enemy enemy, Skill skill) {
        int damage = Mathf.RoundToInt(skill.UseSkill(charName));

        if (skill.IsPhyAttk) {
            // reduce by defense
            damage -= enemy.Defense;
        } else {
            // reduce by magic defense
            damage -= enemy.MagicDefense;
        }

        // reduce enemy's hp
        enemy.CurrentHP -= damage;
        enemy.CurrentHP = Math.Max(enemy.CurrentHP, 0); // set so that 0 is the lowest amount it can go

        return damage;
    }

    /// <summary>
    /// Use a healing skill on a party memter
    /// </summary>
    /// <param name="charUsing">Character using the skill</param>
    /// <param name="charReceiving">Character receiving the skill effects</param>
    /// <param name="skill">Skill being used</param>
    /// <returns></returns>
    public static int UseHealingSkill(string charUsing, string charReceiving, Skill skill) {
        int amountHealed = 0;

        int currHP = GameManager.Instance.Party.GetCharCurrHP(charReceiving);

        skill.UseSkill(charUsing, charReceiving);

        amountHealed = GameManager.Instance.Party.GetCharCurrHP(charReceiving) - currHP;

        return amountHealed;
    }

    /// <summary>
    /// Use a healing skill on self
    /// </summary>
    /// <param name="charName">Character using skill</param>
    /// <param name="skill">Skill being used</param>
    /// <returns></returns>
    public static int UseHealingSkill(string charName, Skill skill) {
        int amountHealed = 0;

        int currHP = GameManager.Instance.Party.GetCharCurrHP(charName);

        skill.UseSkill(charName);

        amountHealed = GameManager.Instance.Party.GetCharCurrHP(charName) - currHP;

        return amountHealed;
    }

    /// <summary>
    /// Determine the turn order for the round
    /// </summary>
    /// <param name="enemies">List of the enemies</param>
    /// <returns>TurnQueue with whose turn it is</returns>
    public static TurnQueue DetermineTurnOrder(List<Enemy> enemies) {
        TurnQueue turnOrder = new TurnQueue();

        string[] turn = new string[200];
        List<string> party = GameManager.Instance.Party.GetCurrentParty();

        foreach (string name in party) {
            float speed = GameManager.Instance.Party.GetCharSpd(name);
            int idx = (int)speed;
            if (turn[idx] != null) {
                // put character in the next available index if speed match
                for (int i = idx - 1; i >= 0; i--) {
                    if (turn[i] == null) {
                        turn[i] = name;
                        break;
                    }
                }
            } else {
                turn[idx] = name;
            }
        }

        foreach (Enemy enemy in enemies) {
            string name = enemy.EnemyName;

            int idx = enemy.Speed;

            if (turn[idx] != null) {
                // put character in the next available index if speed match
                for (int i = idx - 1; i >= 0; i--) {
                    if (turn[i] == null) {
                        turn[i] = name;
                        break;
                    }
                }
            } else {
                turn[idx] = name;
            }
        }

        // put the one with the higher speed in the queue first
        for (int i = turn.Length - 1; i >= 0; i--) {
            if (turn[i] != null) {
                turnOrder.Enqueue(turn[i]);
            }
        }

        return turnOrder;
    }

    /// <summary>
    /// Enemy's turn
    /// </summary>
    /// <param name="enemy">Enemy whose turn it is</param>
    /// <param name="hostilityMeter">Hostility meter</param>
    /// <returns></returns>
    public static EnemyTurn EnemyTurn(Enemy enemy, List<int> hostilityMeter) {
        enemy.IsDefending = false;

        EnemyTurn enemyTurn = new EnemyTurn();

        int move = UnityEngine.Random.Range(0, 5);
        switch (move) {
            case 0:
                // enemy defend
                enemy.IsDefending = true;

                enemyTurn.Defend = true;
                enemyTurn.Attack = false;
                enemyTurn.Heal = false;
                break;
            default:
                // enemy attack
                List<string> party = GameManager.Instance.Party.GetCurrentParty();

                int target = UnityEngine.Random.Range(0, hostilityMeter.Count);
                string partyMember = party[hostilityMeter[target]];

                int damage = enemy.Attack - GameManager.Instance.Party.GetCharDef(partyMember);
                if (GameManager.Instance.Party.IsDefending(partyMember)) {
                    damage /= 2;
                }
                damage = Math.Max(damage, 1); // at least do 1 damage
                GameManager.Instance.Party.DealtDamage(partyMember, damage);

                enemyTurn.Attack = true;
                enemyTurn.Defend = false;
                enemyTurn.Heal = false;
                enemyTurn.Target = target;
                enemyTurn.Amount = damage;

                break;
        }
        return enemyTurn;
    }
}
