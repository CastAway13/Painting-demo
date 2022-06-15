/*///////////////////////////////////////////////////////
/														/
/					BVG TEAM - Vik						/
/														/
/				 Scissor Lift Control					/
/														/
/														/
///////////////////////////////////////////////////////*/

/* 
The script raises the lift up when the up arrow is pressed. It stops when it reaches the top or if the up arrow is pressed again.
It also lowers the lift, if the down arrow is pressed. It stops when it reaches the bottom, or when the down arrow is pressed again.
*/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class liftAnim : MonoBehaviour // liftAnim and C# file name must be the same
{
	public InputHelpers.Button GoUpButton;
	public InputHelpers.Button GoDownButton;
	public XRController HandController;
	
	public float Speed = 0.5f;
	private Animator anim;
	private int goUp;
	private int goDown;
	private int topped;
	private int bottomed;
	private float animFrame;
	private UnityEvent upPressed , downPressed  , upReleased , downReleased;
    public [SerializeField]ScissorButton RightButton ;
    public [SerializeField]ScissorButton LeftButton;
    public [SerializeField]ScissorButton UpButton ;
    public [SerializeField]ScissorButton DownButton ;
    public float speed = 0.3f;
    public float rorationSpeed = 1f;

	private bool _upIsPressed = false;
	private bool _downIsPressed = false;
	void Start() // Sets up Starting conditions
	{
		anim = GetComponent<Animator>();
		goDown = 2;
		goUp = 0;
		topped = 0;
		bottomed = 1;
	}
	
	void Update() // Runs once every frame
	{	
		animFrame = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
	
		if(InputHelpers.IsPressed(HandController.inputDevice , GoUpButton, out bool aisActivated) && aisActivated){
			Up();
		}
		else if(InputHelpers.IsPressed(HandController.inputDevice , GoDownButton, out bool isActivated) && isActivated){
			Down();
		}
		else{
			Pause();
		}
		
		if(animFrame > 1.0f) // Check if we reched the top
		{
			topMax();
		}
		if(animFrame < 0.0f) // Ceck if we reched the bottom
		{
			bottomMax();
		}
        if(UpButton.Status()) {
            this.transform.Translate(Vector3.forward * Time.deltaTime *speed );  
        }
        if(DownButton.Status()) {
            this.transform.Translate(Vector3.back  * Time.deltaTime*speed  );  
        }
        if(RightButton.Status()){
            this.transform.Rotate(Vector3.up,  2* Time.deltaTime*rorationSpeed);  
        }
        if(LeftButton.Status()){
            this.transform.Rotate(Vector3.up, -2* Time.deltaTime*rorationSpeed);  
        }
	}
	float GetLevelValue(){
		
		return 0;
	}
	void onUpPressed(){
		_upIsPressed = true;
		upPressed.Invoke();
		Debug.Log("Up");
	}
	void onDownPressed(){
		_downIsPressed = true;
		downPressed.Invoke();
		Debug.Log("Down");
	}
	private void onUpReleased(){
        _upIsPressed = false;
        upReleased.Invoke();
        Debug.Log("Released");
    }
	private void onDownReleased(){
        _downIsPressed = false;
        downReleased.Invoke();
        Debug.Log("Released");
    }
	void Up() // Start raising the lift
	{
		anim.SetFloat ("Direction", 1);
		anim.speed = Speed;
		anim.Play("move", -1, float.NegativeInfinity);
		goUp = 1;
		goDown = 2;
	}

	public void Down() // Start lowering the lift
	{
		anim.SetFloat ("Direction", -1);
		anim.speed = Speed;
		anim.Play("move", -1, float.NegativeInfinity);
		goDown = 1;
		goUp = 2;
	}
	
	void Pause() // Stop the lift where it is
	{
		goUp = 0;
		goDown = 0;
		topped = 0;
		bottomed = 0;
		anim.speed = 0.0f;
	}
	
	void topMax() // Stop if we reached the top
	{
		switch(topped)
		{
			case 0:
			goUp = 2;
			anim.speed = 0.0f;
			goDown = 0;
			topped = 1;
			bottomed = 0;
			break;
			
			default:
			topped = 1;
			break;
		}
	}
	
	void bottomMax() // Stop if we reached the bottom
	{
		switch(bottomed)
		{
			case 0:
			goDown = 2;
			anim.speed = 0.0f;
			goUp = 0;
			bottomed = 1;
			topped = 0;
			break;
			
			default:
			bottomed = 1;
			break;
		}
	}
}