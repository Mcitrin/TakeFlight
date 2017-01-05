#pragma strict

var anim: Animation;

function Start() {
	anim = GetComponent.<Animation>();
}

// Make the character fade between an idle and a run animation 
// when the player starts to move.
function Update () {
	if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1)
		anim.CrossFade("run");
	else
		anim.CrossFade("idle");
}