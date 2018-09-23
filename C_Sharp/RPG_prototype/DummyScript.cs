using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : GameManager
{
    [SerializeField]
    private GameObject DeathPrefab;
    [SerializeField]
    private QuestInteraction Qinteraction;
    [SerializeField]
    private QuestGiverPositionChangingScript questGiverPosScript;
    private Animator ThisAnimator;
    private bool hasBeenStruck = false;
    [SerializeField]
    private AudioClip ImpactSound; 
    private AudioSource ThisAudioSource;
    // Use this for initialization
    void Start () {
        Health = 200;
        ThisAnimator = GetComponent<Animator>();
        ThisAudioSource = GetComponent <AudioSource> ();
    }

    public void DealDamageToDummy(int damage)
    {
        ThisAudioSource.PlayOneShot(ImpactSound, 0.5f);
        if (!hasBeenStruck)
            StartCoroutine(GetHitAnimation(0.5f));
        if (Qinteraction.ReadCurrentQuest() == 3)
        Health -= damage;
        IsDummydead();
    }

    private IEnumerator GetHitAnimation(float time) {
        hasBeenStruck = true;
        ThisAnimator.SetTrigger("GotHit");
        yield return new WaitForSeconds(time);
        hasBeenStruck = false;
    }
    private void IsDummydead() {
        if (Health <= 0 ) {
            GameObject DeadPrefab = Instantiate(DeathPrefab, transform.position + new Vector3 (0,2,0), Quaternion.identity, null);
            Qinteraction.IsDummyDead(true);
            questGiverPosScript.ChangePositionOfQuestGiver();
            Destroy(this.gameObject);
       }
    }
}
