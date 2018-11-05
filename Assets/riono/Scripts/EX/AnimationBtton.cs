using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBtton : MonoBehaviour {
    public Animator animator;
    AnimatorStateInfo stateInfo;

    // Use this for initialization
    void Start () {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    }
	
	// Update is called once per frame
	void Update () {
        // アニメーションの情報
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        CheckFlag(stateInfo.normalizedTime);
        //Debug.Log(stateInfo.normalizedTime);
        //Debug.Log(Animator.StringToHash("Jump"));
        //Debug.Log(stateInfo1.length);

    }

    public void JumpAnim(){
        animator.SetBool("Jumping", true);
    }

    public void RunAnim(){
        animator.SetBool("Running", true);
    }

    public void RestAnim(){
        animator.SetBool("IsRest", true);
    }

    public  void CheckFlag(float normalizedTime) {
        int nameHash = stateInfo.shortNameHash;

        int jump = Animator.StringToHash("Jump");
        int run = Animator.StringToHash("Run");
        int rest = Animator.StringToHash("Rest");

        if(nameHash == jump ){
            if(normalizedTime >= 1){
                animator.SetBool("Jumping", false);
            }
        }
        else if(nameHash == run){
            if(normalizedTime >= 3){
                animator.SetBool("Running",false);
            }
        }
        else if( nameHash == rest){
            if(normalizedTime >= 1){
                animator.SetBool("IsRest", false);
            }
        }
    }
}
