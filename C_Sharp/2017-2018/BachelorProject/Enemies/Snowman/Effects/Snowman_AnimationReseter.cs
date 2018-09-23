using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_AnimationReseter : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        EnemySnowman snowmanRefrence = animator.transform.parent.GetComponent<EnemySnowman>();

        if (snowmanRefrence.GetPhase() != EnemySnowman.Phase.three)
        {
            snowmanRefrence.SetPhase();
            snowmanRefrence.CheckPhase();
        }
        animator.GetComponent<Snowman_Animations>().PlayerCanContinue(false);
    }

}
