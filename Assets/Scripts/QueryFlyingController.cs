using UnityEngine;
using System.Collections;

namespace QueryChan {

	/**
	 * クエリちゃん用 飛行コントローラ
	 * 2015/3/12 Fantom
	 */
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(QueryMechanimController))]
	public class QueryFlyingController : MonoBehaviour {

		/** 前進速度 */
		private float forwardSpeed = 0f;

		/** キーによる前進速度加速量 */
		public float accelerarion = 0.05f;

		/** 最小前進速度(下限速度 > 0) */
		public float speedMin = 0f;

		/** 最大前進速度(上限速度) */
		public float speedMax = 10f;

		/** 上下[左右]速度(Y軸[X軸:rotationMode=false]方向移動速度) */
		public float moveSpeed = 3f;

		/** 左右旋回モード(false = 左右移動) */
		public bool rotationMode = true;

		/** 左右旋回速度 */
		public float rotationSpeed = 90f;

		//------------------------------------------------//
		//キー使用 ON/OFF

		/** (前進)加速キーの使用 ON/OFF */
		public bool useAccelKey = true;

		/** (前進)減速キーの使用 ON/OFF */
		public bool useBrakeKey = true;

		//------------------------------------------------//
		//キー設定

		/** (前進)加速キー */
		public KeyCode accelKey = KeyCode.LeftShift;

		/** (前進)減速キー */
		public KeyCode brakeKey = KeyCode.LeftControl;

		/** 上昇キー */
		[SerializeField]
		private KeyCode upKey = KeyCode.UpArrow;

		/** 下降キー */
		[SerializeField]
		private KeyCode downKey = KeyCode.DownArrow;

		/** 左移動キー */
		[SerializeField]
		private KeyCode leftKey = KeyCode.LeftArrow;

		/** 右移動キー */
		[SerializeField]
		private KeyCode rightKey = KeyCode.RightArrow;

		//------------------------------------------------//
		//仮想軸キー管理用

		/** 左右仮想軸キー */
		private AxisKey horizontalKey;

		/** 上下仮想軸キー */
		private AxisKey verticalKey;

		//------------------------------------------------//
		//実行用

		/** 移動方向のローカル空間→ワールド空間変換用 */
		private Vector3 moveDirection = Vector3.zero;

		/** CharacterController キャッシュ */
		private CharacterController controller;

		/** QueryMechanimController(Script) キャッシュ */
		private QueryMechanimController queryMechanim;

		/** Animator キャッシュ */
		private Animator animator;

		//------------------------------------------------//
		//ステート管理用

		/** ステートハッシュと QueryChanAnimationType の変換テーブル */
		private static Hashtable StateTable = new Hashtable() {
			{Animator.StringToHash("Base Layer.Idle"), QueryMechanimController.QueryChanAnimationType.FLY_IDLE},
			{Animator.StringToHash("Base Layer.Forward"), QueryMechanimController.QueryChanAnimationType.FLY_STRAIGHT},
			{Animator.StringToHash("Base Layer.Left"), QueryMechanimController.QueryChanAnimationType.FLY_TOLEFT},
			{Animator.StringToHash("Base Layer.Right"), QueryMechanimController.QueryChanAnimationType.FLY_TORIGHT},
			{Animator.StringToHash("Base Layer.Up"), QueryMechanimController.QueryChanAnimationType.FLY_UP},
			{Animator.StringToHash("Base Layer.Down"), QueryMechanimController.QueryChanAnimationType.FLY_DOWN},
		};

		/** 変換テーブルにないときのデフォルトハッシュ */
		private int defaultState = Animator.StringToHash("Base Layer.Idle");

		/** 現在のステートハッシュ */
		private int currentState;

		/** １つ前のステートハッシュ */
		private int oldState;

		/** ステートによって表情や手も変更する ON/OFF */
		public bool useEmotionAndHandChange = true;

		//------------------------------------------------//


		// Use this for initialization
		void Start () {
			//オブジェクトキャッシュ
			controller = this.GetComponent<CharacterController>();
			animator = this.GetComponentInChildren<Animator>();

			queryMechanim = this.GetComponent<QueryMechanimController>();
			queryMechanim.ChangeAnimation(QueryMechanimController.QueryChanAnimationType.FLY_IDLE, false);

			AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);	//BaseLayer
			currentState = animatorInfo.nameHash;
			oldState = currentState;

			//キーを設定
			horizontalKey = new AxisKey(rightKey, leftKey);
			verticalKey = new AxisKey(upKey, downKey);

			//初期化
			forwardSpeed = Mathf.Clamp(forwardSpeed, speedMin, speedMax);
		}


		// Update is called once per frame
		void Update () {

			//------------------------------------------------//
			//入力による移動処理

			bool accel = useAccelKey && Input.GetKey(accelKey);
			bool brake = useBrakeKey && Input.GetKey(brakeKey);

			forwardSpeed += (accel ? accelerarion : 0f) + (brake ? -accelerarion : 0f);
			forwardSpeed = Mathf.Clamp(forwardSpeed, speedMin, speedMax);

			float h = horizontalKey.GetAxis();
			float v = verticalKey.GetAxis();

			if (rotationMode) {
				transform.Rotate(0f, h * rotationSpeed * Time.deltaTime, 0f);
				moveDirection.Set(0f, v * moveSpeed, forwardSpeed);
			} else {
				moveDirection.Set(h * moveSpeed, v * moveSpeed, forwardSpeed);
			}

			moveDirection = transform.TransformDirection(moveDirection);
			controller.Move(moveDirection * Time.deltaTime);

			//------------------------------------------------//
			//Animator

			animator.SetFloat("Speed", forwardSpeed);
			animator.SetFloat("Horizontal", h);
			animator.SetFloat("Vertical", v);

			//------------------------------------------------//
			//ステートによって表情や手を変更する

			if (useEmotionAndHandChange) {
				AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);	//BaseLayer
				currentState = animatorInfo.nameHash;
				if (oldState != currentState) {
					oldState = currentState;

					if (StateTable.ContainsKey(currentState)) {
						queryMechanim.ChangeAnimation((QueryMechanimController.QueryChanAnimationType)StateTable[currentState], false);
					} else {
						queryMechanim.ChangeAnimation((QueryMechanimController.QueryChanAnimationType)StateTable[defaultState], false);
					}
				}
			}
		}
	}

}
