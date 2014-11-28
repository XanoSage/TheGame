//#define FORCE_TAP

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
    public class InputController : MonoBehaviour
    {

        private bool isTouched;

#if (UIToolkit)
	public UIToolkit[] untappableToolkits;

	/// <summary>
	/// Check if untappable toolkits contain screen point.
	/// </summary>
	/// <param name="point">Screen point</param>
	private bool UntappableToolkitsContains (Vector2 point) {
		return untappableToolkits.Any(toolkit => toolkit.Contains(point));
	}

	
#endif

        private const int TouchMouseAlternativeButton = 0;

        private float GetEnlarge()
        {
            if (IsInputLocked)
                return 0;

#if ((UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR || FORCE_TAP)
		if (Input.touchCount < 2)
			return 0;

		Vector2 center = (Input.touches[0].position + Input.touches[1].position) / 2f;

		float angle1 = Vector2.Angle(Input.touches[0].position - center, Input.touches[0].deltaPosition);
		float angle2 = Vector2.Angle(Input.touches[1].position - center, Input.touches[1].deltaPosition);

		return (Input.touches[0].deltaPosition + Input.touches[1].deltaPosition).magnitude * (angle1 + angle2 < 180 ? 1 : -1);
#else
            return Input.GetAxis("Mouse ScrollWheel") * -100f;
#endif
        }

        private const float TapMaxDistance = 48f;
        private const int StandardScreenHeight = 480;

        private Vector3 startPos;
        private Vector3 lastPos;

        private int inputLocked;

        //lock input in this rects
        private List<Rect> lockedRects;

        public static void LockInput()
        {
            Instance.inputLocked++;
            Debug.Log("LOCK INPUT, inputLocked = " + Instance.inputLocked);
        }

        public void AddLockRect(Rect rect)
        {
            if (lockedRects == null)
            {
                lockedRects = new List<Rect>();
            }
			for (int i = 0; i < lockedRects.Count; i++)
			{
				if (lockedRects[i].x == rect.x && lockedRects[i].y == rect.y &&
				    lockedRects[i].width == rect.width && lockedRects[i].height == rect.height)
				{
					return;
				}
			}
            lockedRects.Add(rect);
        }

        public void RemoveLockRect(Rect rect)
        {
            if (lockedRects.Count < 1) return;

            for (int i = 0; i < lockedRects.Count; i++)
            {
                if (lockedRects[i].x == rect.x && lockedRects[i].y == rect.y &&
                    lockedRects[i].width == rect.width && lockedRects[i].height == rect.height)
                {
                    lockedRects.RemoveAt(i);
                }
            }
        }

        public static void UnlockInput()
        {
            Instance.inputLocked = 0;
            Debug.Log("INPUT UNLOCK, inputLocked = " + Instance.inputLocked);

            if (Instance.inputLocked < 0)
            {
                Debug.LogWarning(string.Format("Input locked < 0!"));
            }
        }

        public static bool IsInputLocked { get { return Instance.inputLocked != 0; } }

#if ((UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR || FORCE_TAP)
	private int fingerId;
#endif

        private bool isTappable;

        private Vector2 GetMove()
        {
            if (IsInputLocked)
                return Vector2.zero;

            #region Lock Input in some screen rect

            if (Input.GetMouseButton(TouchMouseAlternativeButton) && lockedRects != null)
            {
                foreach (Rect lockedRect in lockedRects)
                {
                    if (lockedRect.Contains(Input.mousePosition))
                    {
                        return Vector2.zero;
                    }
                }
            }

            #endregion

#if ((UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR || FORCE_TAP)
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
#if (UIToolkit)
			if (UntappableToolkitsContains(Input.mousePosition))
				return Vector2.zero;
#endif
			startPos = Input.touches[0].position;
			fingerId = Input.touches[0].fingerId;
			isTappable = true;
			isTouched = true;
		}
			else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended &&
			     Input.touches[0].fingerId == fingerId &&
			     isTappable) {
			Tap(Input.touches[0].position);
			isTouched = false;
		}
		else if (isTappable) {
				isTappable = IsTappableDistance(Input.touchCount > 0 ? Input.touches[0].position : Vector2.zero);
		}

		return Input.touchCount != 1 || !isTouched ? Vector2.zero : Input.touches[0].deltaPosition;
#else
            if (Input.GetMouseButtonUp(TouchMouseAlternativeButton))
            {
                if (isTappable && isTouched)
                {
					Tap(Input.mousePosition);
                    isTouched = false;
                    return Vector2.zero;
                }
            }

            if (!Input.GetMouseButton(TouchMouseAlternativeButton))
                return Vector2.zero;

            if (Input.GetMouseButtonDown(TouchMouseAlternativeButton))
            {
#if (UIToolkit)
			if (UntappableToolkitsContains(Input.mousePosition)) {
				isTouched = false;
				return Vector2.zero;
			}
#endif
                startPos = Input.mousePosition;
                lastPos = Input.mousePosition;
                isTappable = true;
                isTouched = true;
            }

            if (isTappable)
                isTappable = IsTappableDistance(Input.mousePosition);

            if (isTappable)
                return Vector2.zero;

            Vector2 result = Input.mousePosition - lastPos;

            lastPos = Input.mousePosition;

            return isTouched ? result : Vector2.zero;
#endif
        }

        public float Enlarge { get; private set; }
        public Vector2 Move { get; private set; }

        private bool IsTappableDistance(Vector3 endPos)
        {
            float dist = Vector2.Distance(startPos, endPos);
            return (Tools.ScaleScalar(dist) < TapMaxDistance);
        }

        public static event Action<Vector2> OnTap;
        public static event Action<Vector2> OnSwipe;

        private void Tap(Vector2 pos)
        {
#if (UIToolkit)
		if (UntappableToolkitsContains(Input.mousePosition))
			return;
#endif
            if (OnTap != null)
                OnTap(pos);

            Debug.Log(string.Format("Tap: {0}", pos));
        }

        private void Swipe(Vector2 dir)
        {
            if (OnSwipe != null)
                OnSwipe(dir);

            Debug.Log(string.Format("Swipe: {0}", dir));
        }

        public static InputController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            inputLocked = 0;
            isTouched = false;
        }

        // Update is called once per frame
        void Update()
        {
            Enlarge = GetEnlarge();
            Move = GetMove();
        }
    }

}