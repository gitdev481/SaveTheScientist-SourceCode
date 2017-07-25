using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarControlScript : MonoBehaviour {
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;

    public Transform WheelFLTrans;
    public Transform WheelFRTrans;
    public Transform WheelRLTrans;
    public Transform WheelRRTrans;

    public GameObject truckBody;
    public Material normalMat;
    public Material brakeMat;
    public Material reverseMat;
    bool braked = false;
    float maxBrakeTorque = 150;

    private float maxTorque = 100;

    float lowestSteerAtSpeed = 50;
    float lowSpeedSteerAngle = 10;
    float highSpeedSteerAngle = 1;
    float decelerationSpeed = 40;
    public float currentSpeed;
    float topSpeed = 125;

    private float mySidewayFriction;
    private float myForwardFriction;
    private float slipSidewayFriction;
    private float slipForwardFriction;
    private int[] gearRatio;

    public float mobileForce;
    public Vector2 axisValues;
    public float centreAngleOffset = 0;

    public GameObject winPanel;
    public float gameTimeTaken;
    bool isGameWon = false;
    public Text gameTimeTakenText;
    private GameObject missilePickup;
    private GameObject missileDummy;
    private GameObject launcherLocation;
    public GameObject fireButton;
    public bool isBigRockDestroyed = false;
    public GameObject smallRocks;
    public GameObject jumpCrevice;
    public GameObject rocketPickupSound;
    public GameObject gameWinApplause;
    public GameObject miniMapButton;
    public GameObject backgroundSound;
    public GameObject bumpIntoRockButton;
    private bool missileEquipped = false;
    public GameObject landingPosition;
    public GameObject parachute;
  

    public float rockHealth = 3f;
    private float descentSpeed = 1.5f;
    private bool hasLanded = false;

    // Use this for initialization
    void Start() {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0);
        setValues();
        axisValues = new Vector2(0, 0);
        //winPanel.SetActive(false);
        missilePickup = GameObject.FindGameObjectWithTag("MISSILEPICKUP");
        missileDummy = GameObject.FindGameObjectWithTag("MISSILEDUMMY");
        launcherLocation = GameObject.FindGameObjectWithTag("LAUNCHERLOCATION");
        smallRocks = GameObject.FindGameObjectWithTag("SMALLROCKS");
        jumpCrevice = GameObject.FindGameObjectWithTag("HOLE");
        rocketPickupSound = GameObject.FindGameObjectWithTag("ROCKETPICKUPSOUND");
        gameWinApplause = GameObject.FindGameObjectWithTag("GAMEWINAPPLAUSE");
        miniMapButton = GameObject.FindGameObjectWithTag("MINIMAPBUTTON");
        backgroundSound = GameObject.FindGameObjectWithTag("BACKGROUNDSOUND");
        landingPosition = GameObject.FindGameObjectWithTag("LANDINGPOSITION");
        parachute = GameObject.FindGameObjectWithTag("PARACHUTE");



    }

    void setValues()
    {
        myForwardFriction = WheelRR.forwardFriction.stiffness;
        mySidewayFriction = WheelRR.sidewaysFriction.stiffness;
        slipForwardFriction = 0.01f;
        slipSidewayFriction = 0.005f;
        gearRatio = new int[5];
        gearRatio[0] = 20; gearRatio[1] = 50; gearRatio[2] = 80;
        gearRatio[3] = 110; gearRatio[4] = (int)(topSpeed + 5); //>topspeed
    }

    void FixedUpdate() {

        if (!hasLanded)
        {
            float step = descentSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, landingPosition.transform.position, step);
        }else if(hasLanded)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            landingPosition.SetActive(false);
            parachute.SetActive(false);
        }
        

        if (gameObject.transform.position.y <= 100.5f)
        {
            Application.LoadLevel("GameOverScene");
        }
        Control();
        HandBrake();
        if (!isGameWon) {
            gameTimeTaken += Time.deltaTime;
        }

        if (isBigRockDestroyed || rockHealth <= 0) {
            for (int i = 0; i < smallRocks.transform.childCount; i++)
            {
                GameObject child = smallRocks.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "ENDCOLLIDER") {
            isGameWon = true;
            winPanel.SetActive(true);
            gameTimeTakenText.text = "YOU BEAT THE GAME IN " + (Mathf.Round(gameTimeTaken * 100f) / 100f).ToString() + " SECONDS!";
            backgroundSound.GetComponent<AudioSource>().enabled = false;
            gameWinApplause.GetComponent<AudioSource>().enabled = true;
            gameObject.GetComponent<AudioSource>().enabled = false;
            miniMapButton.SetActive(false);
        }
        if (collision.gameObject == missilePickup)
        {
            Debug.Log("COLLIDED");
            missileEquipped = true;
            //  missilePickup.SetActive(false);
            rocketPickupSound.GetComponent<AudioSource>().enabled = true;
            launcherLocation.SetActive(false);
            missileDummy.GetComponent<MeshRenderer>().enabled = true;
            fireButton.SetActive(true);

        }
        if (collision.gameObject == jumpCrevice)
        {
            // Debug.Log("FELL IN HOLE");
            Application.LoadLevel("GameOverScene");
        }
        if(collision.gameObject == landingPosition)
        {
            hasLanded = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BRIDGEROCK" && missileEquipped == false)
        {
            bumpIntoRockButton.SetActive(true);
        }
    }

    public void SelfDestruct()
    {
        Application.LoadLevel("GameOverScene");
    }
    public void DismissNotification()
    {
        bumpIntoRockButton.SetActive(false);
    }

    public void mobileAccelerate()
    {

        axisValues.y = 1;
        WheelRR.brakeTorque = 0;
        WheelRL.brakeTorque = 0;
    }
    public void mobileReset()
    {
        //if no gas, slow down the car
        axisValues.y = 0;
        WheelRR.brakeTorque = decelerationSpeed;
        WheelRL.brakeTorque = decelerationSpeed;
    }
    public void mobileDecelerate()
    {
        axisValues.y = -1;
        WheelRR.brakeTorque = 0;
        WheelRL.brakeTorque = 0;
    }

	void Control()
	{
       
     
		currentSpeed = (2*3.1415926f *WheelRL.radius*WheelRL.rpm*60/1000)/0.4f;
		
		currentSpeed = Mathf.Round(currentSpeed);
		if(currentSpeed < topSpeed)
		{
            
          
           WheelRR.motorTorque = maxTorque * axisValues.y;
           WheelRL.motorTorque = maxTorque * axisValues.y;

         
        }
		else
		{
			WheelRR.motorTorque = 0;
			WheelRL.motorTorque = 0;
		}
		
		float speedFactor = currentSpeed/lowestSteerAtSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);

#if UNITY_EDITOR
        currentSteerAngle = Input.GetAxis("Horizontal") * 10;
#endif

#if !UNITY_EDITOR
        currentSteerAngle = Input.acceleration.x * 10 ;
#endif

    

        WheelFL.steerAngle = currentSteerAngle;
		WheelFR.steerAngle = currentSteerAngle;

    }
	
	// Update is called once per frame
	void Update()
	{
        //axisValues.x = Input.acceleration.x;

		WheelFLTrans.Rotate(WheelFL.rpm/60*360*Time.deltaTime,0,0);
		WheelFRTrans.Rotate(WheelFR.rpm/60*360*Time.deltaTime,0,0);
		WheelRLTrans.Rotate(WheelRL.rpm/60*360*Time.deltaTime,0,0);
		WheelRRTrans.Rotate(WheelRR.rpm/60*360*Time.deltaTime,0,0);

		float ttFL = WheelFL.steerAngle - WheelFLTrans.localEulerAngles.z;
		float ttFR = WheelFR.steerAngle - WheelFRTrans.localEulerAngles.z;
		Vector3 AngleFL = new Vector3 (WheelFLTrans.localEulerAngles.x,ttFL,WheelFLTrans.localEulerAngles.z);
		WheelFLTrans.localEulerAngles = AngleFL;
		Vector3 AngleFR = new Vector3 (WheelFRTrans.localEulerAngles.x,ttFR,WheelFRTrans.localEulerAngles.z);
		WheelFRTrans.localEulerAngles = AngleFR;

		BrakeLight(); 
		EngineSound();
	}

	void BrakeLight()
	{
		if(currentSpeed > 0 && Input.GetAxis ("Vertical") < 0 )
			truckBody.GetComponent<Renderer>().material = brakeMat;
		else if(currentSpeed < 0 && Input.GetAxis ("Vertical") > 0 )
			truckBody.GetComponent<Renderer>().material = brakeMat;
		else if(currentSpeed < 0 && Input.GetAxis ("Vertical") < 0 )
			truckBody.GetComponent<Renderer>().material = reverseMat;
		else
			truckBody.GetComponent<Renderer>().material = normalMat;
	}

	void EngineSound()
	{
		/*int i;
		for(i=0;i<gearRatio.Length;i++)
		{
			if(gearRatio[i] > currentSpeed ) break;
		}
		float gearMinValue =0;
		float gearMaxValue =0;
		if(i==0)
		{
			gearMinValue =0; gearMaxValue = gearRatio[i];
		}
		else
		{
			gearMinValue = gearRatio[i-1]; 
			gearMaxValue = gearRatio[i];
		}
		float enginePitch = ((currentSpeed - gearMinValue)/(gearMaxValue-gearMinValue))+1;
		GetComponent<AudioSource>().pitch  = enginePitch;*/
		GetComponent<AudioSource>().pitch = currentSpeed/topSpeed+1;
	}

	void HandBrake()
	{
		if(Input.GetButton("Jump"))
		{
			braked = true;
		}
		else
		{
			braked = false;
		}
		if(braked)
		{
			WheelFR.brakeTorque = maxBrakeTorque;
			WheelFL.brakeTorque = maxBrakeTorque;
			WheelRR.motorTorque = 0;
			WheelRL.motorTorque = 0;
			if(GetComponent<Rigidbody>().velocity.magnitude>1)
				SetSlip(slipForwardFriction, slipSidewayFriction);
			else
				SetSlip (1, 1);
			if(currentSpeed <1 && currentSpeed >-1)
				truckBody.GetComponent<Renderer>().material = normalMat;
			else
				truckBody.GetComponent<Renderer>().material = brakeMat;
		}
		else
		{
			SetSlip(myForwardFriction,mySidewayFriction);
			WheelFR.brakeTorque = 0;
			WheelFL.brakeTorque = 0;
		}
	}
	
	void SetSlip(float currentForwardFriction, float currentSidewayFriction)
	{
		WheelFrictionCurve rr = WheelRR.forwardFriction;
		WheelFrictionCurve rl = WheelRL.forwardFriction;
		rr.stiffness = currentForwardFriction;
		rl.stiffness = currentForwardFriction;
		WheelRR.forwardFriction = rr;
		WheelRL.forwardFriction = rl;
		
		rr = WheelRR.sidewaysFriction;
		rl = WheelRL.sidewaysFriction;
		rr.stiffness = currentSidewayFriction;
		rl.stiffness = currentSidewayFriction;
		WheelRR.sidewaysFriction = rr;
		WheelRL.sidewaysFriction = rl;
	}
}
