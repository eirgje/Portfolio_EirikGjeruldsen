using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saving_GameData 
{
    public int collectablesCollected = 0;
    public int playerHealth = 0;
    public bool snehettaDead;
    public bool snowmanDead;
    public bool tutorialFinished;

    public List<Puzzles> puzzles = new List<Puzzles>();
    public List<Ability> abilities = new List<Ability>();

}

