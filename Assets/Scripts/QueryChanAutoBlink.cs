using UnityEngine;
using System.Collections;

namespace QueryChan {

	/**
	 * オート目パチスクリプト
	 * Close→Open 2段階変化
	 * 2015/2/27 Fantom
	 */
	[RequireComponent(typeof(QueryEmotionalController))]
	public class QueryChanAutoBlink : MonoBehaviour {

		/** オート目パチ有効フラグ */
		public bool isActive = true;


		//表情のマテリアル（目と口の組み合わせになってるので注意）
		/** 開いている目 */
		public QueryEmotionalController.QueryChanEmotionalType eyeOpen = 
			QueryEmotionalController.QueryChanEmotionalType.NORMAL_EYEOPEN_MOUTHCLOSE;
		/** 閉じている目 */
		public QueryEmotionalController.QueryChanEmotionalType eyeClose = 
			QueryEmotionalController.QueryChanEmotionalType.NORMAL_EYECLOSE_MOUTHCLOSE;

		/** コントローラ(Script)のキャッシュ */
		private QueryEmotionalController emotionalController;


		/** 目パチ実行中フラグ	 */
		private bool isBlink = false;

		/** 目パチ全体の時間（Close→Open までの時間[秒]） */
		public float timeBlink = 0.15f;

		/** 目パチ全体の時間のゆらぎ時間（0.05 = timeBlink±0.05） */
		public float fluctuation = 0.05f;

		/** 目パチ全体の時間のゆらぎ時間算出値 */
		private float fluctTime;

		/** 目パチ発生ランダム判定の閾値（乱数0～1.0 で 0.3より上のとき発生[=70%]） */
		public float threshold = 0.3f;

		/** 目パチ発生ランダム判定実行のインターバル */
		public float interval = 3.0f;


		/** 目の状態を表すステータス */
		enum Status {
			/** 目が閉じている状態 */
			Close,
			/** 目が開いている状態 */
			Open
		}

		/** 現在の目パチステータス */
		private Status eyeStatus = Status.Open;



		// Use this for initialization
		void Start () {
			emotionalController = this.GetComponent<QueryEmotionalController>();
			
			//初期化（Status.Open 前提）
			Reset();
			
			//目パチランダム発生と変化をスタートする
			StartCoroutine("RandomChange");
		}

		/** 状態初期化（目が開いていて[=Status.Open]、瞬きしてない[isBlink=false]状態にする） */
		public void Reset () {
			SetOpenEyes();
			isBlink = false;
		}

		void SetCloseEyes () {
			eyeStatus = Status.Close;
			emotionalController.ChangeEmotion(eyeClose);
		}

		void SetOpenEyes () {
			eyeStatus = Status.Open;
			emotionalController.ChangeEmotion(eyeOpen);
		}

		/** 目パチランダム発生と変化[(Open→)Close→Open] */
		IEnumerator RandomChange () {
			while (true) {
				if (isActive) {
					if (isBlink) {
						switch (eyeStatus) {
						case Status.Open:	//Open→Close へ
							SetCloseEyes();
							yield return new WaitForSeconds(timeBlink + fluctTime);
							break;
							
						case Status.Close:	//Close→Open へ(ランダム発生待ち)
							SetOpenEyes();
							isBlink = false;
							yield return null;
							break;
						}
					} else {
						//ランダム発生
						if (Random.Range(0f, 1f) > threshold) {
							isBlink = true;
							fluctTime = Random.Range(-fluctuation, fluctuation);
						}
						yield return new WaitForSeconds(interval);
					}
				} else {
					yield return null;
				}
			}
		}
	}

}
