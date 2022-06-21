using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStats
{
    public int constitution;
    public int wits;
    public int agility;
    public int strength;
    public int intelligence;

    public CharacterStats(int c, int w, int a, int s, int i)
    {
        this.constitution = c;
        this.wits = w;
        this.agility = a;
        this.strength = s;
        this.intelligence = i;
    }
}
public struct CharacterData
{
    public string name;
    public int level;
    public int health;
    public int healthMax;
    public int energy;
    public int energyMax;

    public CharacterStats stats;

    public CharacterData(string n, int l, int h, int hm, int e, int em, CharacterStats newStats)
    {
        name = n;
        level = l;
        health = h;
        healthMax = hm;
        energy = e;
        energyMax = em;
        stats = newStats;
    }
}
