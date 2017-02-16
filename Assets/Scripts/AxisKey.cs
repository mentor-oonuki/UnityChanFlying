using UnityEngine;
using System.Collections;

/**
 * Input.GetKey() で Input.GetAxisRaw() のような仮想軸の値を返す。
 * -1, 0, 1 のみで中間の値はない。
 * 2015/3/12 Fantom
 */
public class AxisKey {

	/** +1 を返すキー */
	public KeyCode positiveKey = KeyCode.UpArrow;

	/** -1 を返すキー */
	public KeyCode negativeKey = KeyCode.DownArrow;

	/** 現在押しっぱなしにされているキー */
	private KeyCode holdKey = KeyCode.None;

	//------------------------------------------------------//
	//コンストラクタ

	/**
	 * キー指定のコンストラクタ
	 * positiveKey = 1 / negativeKey = -1 となる。
	 */
	public AxisKey(KeyCode positiveKey, KeyCode negativeKey) {
		this.positiveKey = positiveKey;
		this.negativeKey = negativeKey;
	}

	//------------------------------------------------------//

	/**
	 * Input.GetKey() で Input.GetAxisRaw() のような仮想軸の値を返す。
	 * positiveKey = 1 / negativeKey = -1 / 離したとき or それ以外は 0 を返す。
	 * Input.GetAxisRaw() は急激に反対のキーを押しても -1→0→1 のように 0 を通過するが、
	 * この関数は、-1→1 のように 0 を通過しない値を返す。
	 * 同時押しの場合は、先に押されていた方の値を返す。
	 */
	public float GetAxis() {
//		bool positive = Input.GetKey(positiveKey);
//		bool negative = Input.GetKey(negativeKey);

        bool positive = Input.GetKey(positiveKey);
        bool negative = Input.GetKey(negativeKey);


        //同時押しの場合、先に押されていた方を優先する
        if (holdKey == positiveKey && positive) {
			return 1f;		//holdKey は変更なし
		}
		if (holdKey == negativeKey && negative) {
			return -1f;		//holdKey は変更なし
		}

		if (positive) {
			holdKey = positiveKey;
			return 1f;
		}
		if (negative) {
			holdKey = negativeKey;
			return -1f;
		}

		holdKey = KeyCode.None;
		return 0f;
	}
}
