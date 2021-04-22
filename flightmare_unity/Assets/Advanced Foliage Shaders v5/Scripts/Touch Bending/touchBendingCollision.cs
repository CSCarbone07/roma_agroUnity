using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("AFS/Touch Bending/Collision")]
public class touchBendingCollision: MonoBehaviour {

	public float bendability = 10.0f;
	public float disturbance = 0.3f;
	public float duration = 5.0f;
	
	// Use component caching
	private Renderer m_Renderer;
	private MaterialPropertyBlock TouchMaterialBlock;
	
	private int registeredTouches = 0;	

	private Matrix4x4 finalMatrix = Matrix4x4.identity;
	private Quaternion finalRotation = Quaternion.identity;
	private Vector4 finalTouchForce = Vector4.zero;

	private int touchToRemove = -1;
	private bool resetted = true;
	private int RotMatrixPID;
	private int TouchBendingForcePID;

    private Matrix4x4 ZeroMatrix = Matrix4x4.identity;
    private Quaternion ZeroQuaternion = Quaternion.identity;
    private Vector4 ZeroVec = Vector4.zero;

	private struct touch {
		public GameObject go;
		public int playerId;
        public touchBendingPlayerListener playerListener;
        public Vector3 playerDirection;
		public float intialTouchForce;
        //  As proposed by @PutridPleasure in the forum
        public float finalTouchForce;
		public Vector3 axis;
		public float easingControl;
		public float timer;
		public float timerLeft;
		public bool left;
	}

	private List<touch> touches = new List<touch>();
	
	// Init component caching
	// void Awake () {
	void OnEnable() {
		m_Renderer = GetComponent<Renderer>();
		RotMatrixPID = Shader.PropertyToID("_RotMatrix");
		TouchBendingForcePID = Shader.PropertyToID("_TouchBendingForce");
		// Init MaterialPropertyBlock
		TouchMaterialBlock = new MaterialPropertyBlock();
	}

	void OnTriggerEnter(Collider other) {
		touchBendingPlayerListener tempPlayerVars = other.GetComponent<touchBendingPlayerListener>();
		// register touch only if collider has the touchBendingPlayerListener script attached and enabled
		if( tempPlayerVars != null && tempPlayerVars.enabled) {
            touches.Add(new touch() {
                go = other.transform.gameObject,
                playerId = other.GetInstanceID(),
                playerListener = tempPlayerVars,
                playerDirection = tempPlayerVars.Player_Direction,
				intialTouchForce = tempPlayerVars.Player_Speed,
				// rotate axis by 90
				axis = Quaternion.Euler(0,90,0) * tempPlayerVars.Player_Direction,
				timer = 0.0f,
				timerLeft = 0.0f,
				left = false,
			});
		}
	}

	void OnTriggerExit(Collider other) {

		int pID = other.GetInstanceID();
		
		for(int i = 0; i < touches.Count; i++) {
            //if (touches[i].playerId == pID) {
        //  As proposed by @PutridPleasure on the forum
            if (touches[i].playerId == pID && touches[i].left == false) {
                touch temp = touches[i];
				temp.left = true;
                temp.finalTouchForce = temp.playerListener.Player_Speed;
                break;
			}
		} 
	}

	void Update () {
		registeredTouches = touches.Count;
	//	Handles touches
		if (registeredTouches > 0 ) {
        //	Reset
            finalMatrix = ZeroMatrix;
            finalRotation = ZeroQuaternion;
            finalTouchForce = ZeroVec;
            resetted = false;
			touch temp;
        //  Cache Time.deltaTime
            var deltaTime = Time.deltaTime;

            for (int i = 0; i < registeredTouches; i++) {
				temp = touches[i];

			//	Reset Animation if it has ended
				if ( temp.timer >= duration ) {
					if (temp.left == true) {
						touchToRemove = i;
					}
				}

			//	Bounce Animation
				else {
					temp.easingControl = Mathf.Lerp( Mathf.Sin(temp.timer * 10.0f / duration) / (temp.timer + 1.25f) * 8.0f, 0.0f, Mathf.Clamp01( temp.timer / duration ) );
				}

            //  As proposed by @PutridPleasure on the forum
                float speedForce;

				if (temp.left == true) {
                    speedForce = temp.finalTouchForce;
					// timerLeft = 0 --> player is outside the trigger
					temp.timerLeft = Mathf.Clamp01(temp.timerLeft - deltaTime * duration);
				}
				else {
                    speedForce = temp.playerListener.Player_Speed;
                    // timerLeft = 1 --> player is inside the trigger
                    temp.timerLeft = Mathf.Clamp01(temp.timerLeft + deltaTime * duration);
				}

			//	Set timer according to Player_Speed if player is inside the trigger
				temp.timer += deltaTime * Mathf.Lerp(1.0f, temp.playerListener.Player_Speed, temp.timerLeft);

				touches[i] = temp;
				Quaternion rotation = Quaternion.Euler (temp.axis * (temp.intialTouchForce * bendability * temp.easingControl) );
				finalRotation = finalRotation * rotation;

			//	Calculate extra force // faster to do it componentwise instead of new Vector4()
				finalTouchForce.x += temp.playerDirection.x * temp.easingControl;
				finalTouchForce.y += temp.playerDirection.y * temp.easingControl;
				finalTouchForce.z += temp.playerDirection.z * temp.easingControl;
				finalTouchForce.w += speedForce * temp.easingControl * disturbance; 
			}

		//	Set rotation and extra force
			finalMatrix.SetTRS(Vector3.zero, finalRotation, Vector3.one );
			TouchMaterialBlock.SetMatrix (RotMatrixPID, finalMatrix);
			TouchMaterialBlock.SetVector(TouchBendingForcePID, finalTouchForce);
			m_Renderer.SetPropertyBlock(TouchMaterialBlock);

		//	Remove touch from list
			if (touchToRemove != -1) {
				touches.RemoveAt(touchToRemove);
				touchToRemove= -1;
			}
		}
		else if (!resetted) {
		//	Remove PropertyBlock
			resetted = true;
			TouchMaterialBlock.Clear();
			m_Renderer.SetPropertyBlock(TouchMaterialBlock);
		}
	}
}