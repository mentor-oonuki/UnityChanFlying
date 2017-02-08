using UnityEngine;
using System.Collections;

/**
 * SmoothFollow.js の C#版 に軸回転アングルと高さと距離の遠近機能を追加したもの
 * 2015/2/6 Fantom
 */
[AddComponentMenu("Camera-Control/SmoothFollow2")]
public class SmoothFollow2 : MonoBehaviour {

	/** 追従するオブジェクト */
	public Transform target;

	/** Z方向の距離 */
	public float distance = 10.0f;

	/** Y方向の高さ */
	public float height = 5.0f;

	/** カメラアングル初期値 */
	public float preAngle = 0f;

	/** 上下高さのスムーズ移動速度 */
	public float heightDamping = 2.0f;

	/** 左右回転のスムーズ移動速度 */
	public float rotationDamping = 3.0f;

	/** 距離のスムーズ移動速度 */
	public float distanceDamping = 1.0f;


	//回転の操作用

	/** 回転のキー操作の ON/OFF */
	public bool angleKeyOperation = true;

	/** 左右回転速度 */
	public float angleKeySpeed = 45f;

	/** 左旋回キー */
	public KeyCode angleKeyLeft = KeyCode.Z;

	/** 右旋回キー */
	public KeyCode angleKeyRight = KeyCode.X;

	/** カメラアングル相対値 */
	private float angle;

	/** 回転のドラッグ操作の ON/OFF */
	public bool angleDragOperation = true;

	/** ドラッグ操作での回転速度 */
	public float angleDragSpeed = 10f;

	/** マウス移動始点 */
	private Vector3 startPos;


	//高さの操作用

	/** 高さのキー操作の ON/OFF */
	public bool heightKeyOperation = true;

	/** キー操作での移動速度 */
	public float heightKeySpeed = 1.5f;

	/** 高さ上へキー */
	public KeyCode heightKeyUp = KeyCode.C;

	/** 高さ下へキー */
	public KeyCode heightKeyDown = KeyCode.V;

	/** 高さのドラッグ操作での ON/OFF */
	public bool heightDragOperation = true;

	/** ドラッグ操作での高さ移動速度 */
	public float heightDragSpeed = 0.5f;


	//距離の操作用

	/** 距離のキー操作の ON/OFF */
	public bool distanceKeyOperation = true;

	/** Z方向の最小距離 */
	public float distanceMin = 1.0f;

	/** 距離の移動速度 */
	public float distanceKeySpeed = 1.0f;

	/** 近くへキー */
	public KeyCode distanceKeyNear = KeyCode.LeftControl;

	/** 遠くへキー */
	public KeyCode distanceKeyFar = KeyCode.LeftAlt;

	/** 距離のホイール操作の ON/OFF */
	public bool distanceWheelOperation = true;

	/** ホイール１目盛りの速度倍率（ホイール量 x N倍） */
	public float distanceWheelSpeed = 10f;

	/** 変化先距離 */
	private float wantedDistance;



	// Use this for initialization
	void Start () {
		angle = preAngle;
		wantedDistance = distance;
	}

	// Update is called once per frame
	void Update () {

		//回転のキー操作
		if (angleKeyOperation) {
			if (Input.GetKey(angleKeyLeft)) {
				angle += angleKeySpeed * Time.deltaTime;
				if (angle >= 360f) {
					angle -= 360f;
				}
			}
			if (Input.GetKey(angleKeyRight)) {
				angle -= angleKeySpeed * Time.deltaTime;
				if (angle < 0f) {
					angle += 360f;
				}
			}
		}

		//高さのキー操作
		if (heightKeyOperation) {
			if (Input.GetKey(heightKeyUp)) {
				height += heightKeySpeed * Time.deltaTime;
			}
			if (Input.GetKey(heightKeyDown)) {
				height -= heightKeySpeed * Time.deltaTime;
			}
		}

		//ドラッグ操作
		if (angleDragOperation || heightDragOperation) {
			Vector3 movePos = Vector3.zero;
			if (Input.GetMouseButtonDown(0)) {
				startPos = Input.mousePosition;
			}
			else if (Input.GetMouseButton(0)) {
				movePos = Input.mousePosition - startPos;
				startPos = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0)) {
				startPos = Vector3.zero;
			}
			if (movePos != Vector3.zero) {
				//回転のドラッグ操作
				if (angleDragOperation) {
					angle += movePos.x * angleDragSpeed * Time.deltaTime;
					if (angle < 0f) {
						angle += 360f;
					} else if (angle >= 360f) {
						angle -= 360f;
					}
				}
				//高さのドラッグ操作
				if (heightDragOperation) {
					height -= movePos.y * heightDragSpeed * Time.deltaTime;
				}
			}
		}

		//距離のキー操作
		if (distanceKeyOperation) {
			if (Input.GetKey(distanceKeyNear)) {
				wantedDistance = distance - distanceKeySpeed;
				if (wantedDistance <= distanceMin) {
					wantedDistance = distanceMin;
				}
			}
			if (Input.GetKey(distanceKeyFar)) {
				wantedDistance = distance + distanceKeySpeed;
			}
		}

		//距離のホイール遠近
		if (distanceWheelOperation) {
			float mw = Input.GetAxis("Mouse ScrollWheel");
			if (mw != 0) {
				wantedDistance = distance - mw * distanceWheelSpeed; //0.1 x N倍
				if (wantedDistance <= distanceMin) {
					wantedDistance = distanceMin;
				}
			}
		}
	}

	void LateUpdate () {
		if (target == null) {
			return;
		}

		//追従先位置
		float wantedRotationAngle = target.eulerAngles.y + angle;
		float wantedHeight = target.position.y + height;

		//現在位置
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		//追従先へのスムーズ移動距離(方向)
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		distance = Mathf.Lerp(distance, wantedDistance, distanceDamping * Time.deltaTime);

		//カメラの移動
		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
		Vector3 pos = target.position - currentRotation * Vector3.forward * distance;
		pos.y = currentHeight;
		transform.position = pos;

		transform.LookAt(target);
	}
}
