using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour {

	public static BirdScript instance;

	[SerializeField]
	private Rigidbody2D myRigidBody;

	[SerializeField]
	private Animator anim;

	private float forwardSpeed = 3f; //3f;    5.5f(green)     3f(red and blue)

    private float bounceSpeed = 4f; //4f;       6f (red)        4f(green and blue)
    //if (PlayerPrefs.IsGreenBirdUnlocked())

        

	private bool didFlap;

	public bool isAlive;

	private Button flapButton;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip flapClick, pointClip, diedClip;

	public int score;

	void Awake() {
		if (instance == null) {
			instance = this;
		}

		isAlive = true;
		score = 0;

		flapButton = GameObject.FindGameObjectWithTag ("FlapButton").GetComponent<Button> ();
		flapButton.onClick.AddListener (() => FlapTheBird());

		SetCamerasX ();
	}

	// Use this for initialization
	void Start () {
        if (GameController.instance.GetSelectedBird() == 1)
        {
            forwardSpeed = 5.5f;
            bounceSpeed = 3.3f;
        }

        if (GameController.instance.GetSelectedBird() == 2)
        {
            forwardSpeed = 2.3f;
            bounceSpeed = 6f;

        }

        //if (MenuController.copyOfBirds[1].activeSelf)
        //    forwardSpeed = 6f;
        //if (MenuController.copyOfBirds[2].activeSelf)
        //    bounceSpeed = 8f;


      /*  if (GameController.instance.IsGreenBirdUnlocked() == 1)
            forwardSpeed = 6f;
        if (GameController.instance.IsRedBirdUnlocked() == 1)
            bounceSpeed = 8f;
	
       */ 
    }
	
	// Update is called once per frame
	void FixedUpdate () {

       // if (GameController.instance.//,IsGreenBirdUnlocked() )
		if (isAlive) {
            
			Vector3 temp = transform.position;
			temp.x += forwardSpeed * Time.deltaTime;
			transform.position = temp;

			if(didFlap) {
				didFlap = false;
				myRigidBody.velocity = new Vector2(0, bounceSpeed);
				audioSource.PlayOneShot(flapClick);
				anim.SetTrigger("Flap");
			}

			if(myRigidBody.velocity.y >= 0) {
				transform.rotation = Quaternion.Euler(0, 0, 0);
			} else {
				float angle = 0;
				angle = Mathf.Lerp(0, -90, -myRigidBody.velocity.y / 7);
				transform.rotation = Quaternion.Euler(0, 0, angle);
			}

		}

	}

	void SetCamerasX() {
		CameraScript.offsetX = (Camera.main.transform.position.x - transform.position.x) - 1f;
	}

	public float GetPositionX() {
		return transform.position.x;
	}

	public void FlapTheBird() {
		didFlap = true;
	}

	void OnCollisionEnter2D(Collision2D target) {
		if (target.gameObject.tag == "Ground" || target.gameObject.tag == "Pipe") {
			if(isAlive) {
				isAlive = false;
				anim.SetTrigger("Bird Died");
				audioSource.PlayOneShot(diedClip);
				GameplayController.instance.PlayerDiedShowScore(score);
                int random = Random.Range(0, 10);
                if (random > 5)
				AdsController.instance.ShowInterstitial();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == "PipeHolder") {
			score++;
			GameplayController.instance.SetScore(score);
			audioSource.PlayOneShot(pointClip);
		}
	}


}























































