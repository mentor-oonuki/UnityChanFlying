using UnityEngine;
using System.Collections;

/**
 * キャラクター飛行コントローラ
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterFlickFlyingController : MonoBehaviour
{

    /** 前進速度 */
    private float forwardSpeed = 0f;

    /** キーによる前進速度加速量 */
    public float accelerarion = 0.1f;

    /** 最小前進速度(下限速度 > 0) */
    public float speedMin = 0f;

    /** 最大前進速度(上限速度) */
    public float speedMax = 10f;

    /** 上下[左右]速度(Y軸[X軸:rotationMode=false]方向移動速度) */
    public float moveSpeed = 3f;

    /** 左右旋回モード(false = 左右移動) */
    public bool rotationMode = true;

    /** 左右旋回速度 */
    public float rotationSpeed = 1f;

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

    /** Animator キャッシュ */
    private Animator animator;

    //------------------------------------------------//

    // Use this for initialization
    void Start()
    {
        //オブジェクトキャッシュ
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();

        //キーを設定
        horizontalKey = new AxisKey(rightKey, leftKey);
        verticalKey = new AxisKey(upKey, downKey);

        //初期化
        forwardSpeed = Mathf.Clamp(forwardSpeed, speedMin, speedMax);
    }

    // Update is called once per frame
    void Update()
    {
        //入力による移動処理

        bool accel = useAccelKey && (FlickManager.Instance.Accel || Input.GetKey(accelKey));
        bool brake = useBrakeKey && (FlickManager.Instance.Brake || Input.GetKey(brakeKey));


        forwardSpeed += (accel ? accelerarion : 0f) + (brake ? -accelerarion * 2 : 0f);
        forwardSpeed = Mathf.Clamp(forwardSpeed, speedMin, speedMax);

        //        float h = horizontalKey.GetAxis();
        //        float v = verticalKey.GetAxis();

        float h = FlickManager.Instance.DeltaX;
        h = h > 45 ? 45 : h;
        h = h < -45 ? -45 : h;
        float v = FlickManager.Instance.DeltaY;
        v = v > 45 ? 45 : v;
        v = v < -45 ? -45 : v;


        if (rotationMode)
        {
            transform.Rotate(0f, h * rotationSpeed * Time.deltaTime, 0f);
            moveDirection.Set(0f, v * moveSpeed, forwardSpeed);
            // transform.Rotate(0f, h * Time.deltaTime, 0f);
            // moveDirection.Set(0f, v, forwardSpeed);
        }
        else
        {
            moveDirection.Set(h * moveSpeed, v * moveSpeed, forwardSpeed);
        }

        moveDirection = transform.TransformDirection(moveDirection);
        controller.Move(moveDirection * Time.deltaTime);

        //------------------------------------------------//
        //Animator
        animator.SetFloat("Speed", forwardSpeed);
        animator.SetFloat("Horizontal", h);
        animator.SetFloat("Vertical", v);
    }
}