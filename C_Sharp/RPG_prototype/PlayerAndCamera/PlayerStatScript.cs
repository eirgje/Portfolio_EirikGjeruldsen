using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatScript : GameManager {

    [Header("---------------------")]
    [Header("Health and Energy")]
    [Header("---------------------")]
    [SerializeField]
    private Image healthbar;
    [SerializeField]
    private Image energybar;
    [SerializeField]
    private int StartHealth;
    [Range(1,10)]
    [SerializeField]
    private int AttackDamage_PerClaw;

    private float EnergyLevel = 1f;
    [Range(1f, 60f)]
    [SerializeField]
    private float WallRegainTimer = 15f;

    [Header("---------------------")]
    [Header("Canvas ref.")]
    [Header("---------------------")]
    [SerializeField]
    private GameObject UI_Canvas;
    [SerializeField]
    private GameObject GameOver_Canvas;


    [Header("---------------------")]
    [Header("Other")]
    [Header("---------------------")]
    [SerializeField]
    private AudioClip ImpactSound;
    private Animator ThisAnimator;
    [SerializeField]
    private Transform StartSpawnLocation = null;
    private Transform CurrentCheckPoint = null;


    private bool isDead = false;
    private bool hasTakenDamage = false;
 

    private AudioSource ThisAudioSource;
	// Use this for initialization
	void Awake () {
        Health = StartHealth;
        AttackDamage = AttackDamage_PerClaw;
        ThisAudioSource = GetComponent<AudioSource>();
        ThisAnimator = GetComponent<Animator>();
        CurrentCheckPoint = StartSpawnLocation;
	}
	
	// Update is called once per frame
	void Update () {
        
        SetPlayerHealth();
        SetPlayerEnergy();
		
	}
    public bool GameOver() {
        if (Health <= 0)
        {
            if (!GameOver_Canvas.activeSelf && UI_Canvas.activeSelf)
            { 
               GameOver_Canvas.SetActive(true);
               UI_Canvas.SetActive(false); 
            }
            return true;
        }
        else
        {
            if (GameOver_Canvas.activeSelf && !UI_Canvas.activeSelf)
            {
                GameOver_Canvas.SetActive(false);
                UI_Canvas.SetActive(true);
            }

            return false;
        }
    }

    public int GetAttackDamage() { return AttackDamage; }

    public void DealDamageToPlayer(int damage, Transform hitFrom)
    {
        if (Health > 0 && !GetComponent<PlayerControllerTwo>().GetCastingStance())
        {
            Health -= damage;
            Vector3 distance = (hitFrom.localPosition - transform.localPosition).normalized;
            print(distance);
            ThisAnimator.SetTrigger("HitFromLeft");

        }

        if (Health <= 0)
            isDead = true;
    }

    private void SetPlayerEnergy() {
        energybar.fillAmount = EnergyLevel;
    }

    private void SetPlayerHealth() {
        float healthInFloat = (float)Health / 100;
        healthbar.fillAmount = healthInFloat;
    }


    public float CurrentEnergy() {
        return EnergyLevel;
    }

    public void DepleteEnergy() {
        if (EnergyLevel >= 0)
        EnergyLevel -= 0.1f * Time.deltaTime;
    }

    public void RegainEnergy(){
        if (EnergyLevel <= 1) {
            EnergyLevel += 0.05f * Time.deltaTime;
        }
    }
    public void RespawnPlayer() {
        Health = 100;
        transform.position = CurrentCheckPoint.position;
    }


}
