using UnityEngine;
using System.Collections;

public class PlayerModel : MonoBehaviour {


	Rigidbody2D rbody;
	Animator anim;
	Player owner;


	public void init(Player owner){
		rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update(){
		Vector2 moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (moveVec != Vector2.zero) {
			anim.SetBool ("isWalking", true);
			anim.SetFloat ("input_x", moveVec.x);
			anim.SetFloat ("input_y", moveVec.y);
		} else {
			anim.SetBool ("isWalking", false);
		}
		rbody.MovePosition (rbody.position + moveVec * Time.deltaTime);
	}


}
