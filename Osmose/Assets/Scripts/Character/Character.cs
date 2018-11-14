using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
    private string name;

    private CharacterStat hp;
    private CharacterStat sp;
    private CharacterStat attack;
    private CharacterStat defense;
    private CharacterStat speed;
    private CharacterStat luck;

    private Object weapon;
    private Object armor;

    public Character(string name, float hp, float sp, float attack, float defense, float speed, float luck) {
        this.name = name;

        this.hp = new CharacterStat(hp);
        this.sp = new CharacterStat(sp);
        this.attack = new CharacterStat(attack);
        this.defense = new CharacterStat(defense);
        this.speed = new CharacterStat(speed);
        this.luck = new CharacterStat(luck);
    }
}
