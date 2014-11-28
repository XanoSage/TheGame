using System;
using UnityEngine;
using System.Collections;
//using UnityTools.Other;
//using Motion = UnityTools.Other.Motion;

namespace UnityTools.Other
{

    public class CameraMovement : MonoBehaviour
    {

        public static CameraMovement Instance { get; private set; }

        private const float DefaultAnimationTime = 1.3f;

        public Vector3 centralPoint = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector2 movingSpeed = new Vector2(-0.01f, 0.01f);

        public float enlargingSpeedMin = 0.3f;
        public float enlargingSpeedMax = 0.3f;
        public float enlargingRatio = 0.5f;

        public int smoothRate = 10;

        public event Action OnRadiusMinimumAchieved;
        public event Action OnRadiusMaximumAchieved;

        public static bool IsCameraMoving { get; private set; }

        public SphericalCoordinates startingCoordinates = new SphericalCoordinates
        {
            radius = 3f,
            azimuthAngle = 1.0f,
            polarAngle = 1.0f,
        };

        public SphericalCoordinates minimum = new SphericalCoordinates
        {
            radius = 0.01f,
            azimuthAngle = float.NegativeInfinity,
            polarAngle = 0.01f
        };

        public SphericalCoordinates maximum = new SphericalCoordinates
        {
            radius = 10f,
            azimuthAngle = float.PositiveInfinity,
            polarAngle = 1.65f
        };

        public float AndroidCameraLookSensitivity = 1.0f;

        private SphericalCoordinates currentCoordinates;

        public SphericalCoordinates Coordinates
        {
            get
            {
                return currentCoordinates;
            }

            set
            {
                currentCoordinates = value;
                UpdatePosition();
            }
        }

        private SmoothUnit smoothMove;
        private SmoothUnit smoothEnlarge;

        public Vector3 LookingPoint
        {
            get { return transform.parent.position; }

            private set
            {
                transform.parent.position = value;
                UpdatePosition();
            }
        }

        private SphericalCoordinates lastCoords;
        private Vector3 lastParentPos;
        private Vector3 lastParentForward;

        private enum LookingModeType
        {
            LookAt,
            Reset,
        }

        private LookingModeType lookingMode;

        public void ResetLooking()
        {
            Coordinates = startingCoordinates;
            LookingPoint = centralPoint;
            transform.parent.rotation = Quaternion.identity;

            if (lookingMode == LookingModeType.LookAt)
                InputController.UnlockInput();

            lookingMode = LookingModeType.Reset;
        }

        public void LookAt(Transform target, SphericalCoordinates coords)
        {
            if (lookingMode == LookingModeType.Reset)
            {
                lastCoords = Coordinates;
                lastParentPos = LookingPoint;

                InputController.LockInput();
            }

            Coordinates = coords;
            LookingPoint = target.position;

            transform.parent.rotation = target.rotation;

            lookingMode = LookingModeType.LookAt;
        }

        private IEnumerator MoveAndRotate(Vector3 targetForward, Vector3 targetPos, SphericalCoordinates targetCoords,
            float time, float delay)
        {
            MotionsCollection collection = new MotionsCollection();

            collection.AddMotion(Motion.AddMotion(transform.parent.gameObject,
                Motion.MotionType.Position,
                Motion.BehaviourType.To,
                Motion.FunctionType.Smooth,
                time,
                delay,
                targetPos,
                Vector3.zero,
                false));

            float rotAngle = Tools.Angle(transform.parent.forward, targetForward, Vector3.up);

            collection.AddMotion(Motion.AddMotion(transform.parent.gameObject,
                Motion.MotionType.Rotation,
                Motion.BehaviourType.By,
                Motion.FunctionType.Smooth,
                time,
                delay,
                new Vector3(0, rotAngle, 0),
                Vector3.zero,
                false,
                true,
                false,
                false,
                false));

            SphericalCoordinates delta = SphericalCoordinates.Delta(Coordinates, targetCoords);

            Motion cameraRot = Motion.AddMotion(gameObject,
                Motion.MotionType.Function,
                Motion.BehaviourType.FromTo,
                Motion.FunctionType.Smooth,
                time,
                delay,
                Coordinates.AsVector3,
                (Coordinates + delta).AsVector3,
                false);

            cameraRot.Function = vec => Coordinates = SphericalCoordinates.GetFromVector3(vec);

            collection.AddMotion(cameraRot);

            yield return new WaitForMotionsCollection(collection);
        }

        private void BeforeLookAt()
        {
            if (lookingMode == LookingModeType.Reset)
            {
                lastCoords = Coordinates;
                lastParentPos = LookingPoint;
                lastParentForward = transform.parent.forward;

                InputController.LockInput();
            }

            lookingMode = LookingModeType.LookAt;
        }

        private void AfterReset()
        {
            if (lookingMode == LookingModeType.LookAt)
                InputController.UnlockInput();

            lookingMode = LookingModeType.Reset;
        }

        public IEnumerator LookAtSmoothly(Transform target, SphericalCoordinates coords)
        {
            BeforeLookAt();
            yield return
                this.StartSmartRoutine(MoveAndRotate(target.forward, target.position, coords, DefaultAnimationTime, 0));
        }

        public IEnumerator LookAtSmoothly(Transform target, SphericalCoordinates coords, float time, float delay)
        {
            BeforeLookAt();
            yield return this.StartSmartRoutine(MoveAndRotate(target.forward, target.position, coords, time, delay));
        }

        public IEnumerator LookAtSmoothly(Vector3 targetForward, Vector3 targetPosition, SphericalCoordinates coords,
            float time, float delay)
        {
            BeforeLookAt();
            yield return this.StartSmartRoutine(MoveAndRotate(targetForward, targetPosition, coords, time, delay));
        }

        public IEnumerator ResetSmoothly()
        {
            yield return
                this.StartSmartRoutine(MoveAndRotate(lastParentForward, lastParentPos, lastCoords, DefaultAnimationTime,
                    0));
            AfterReset();
        }

        public IEnumerator ResetWithoutReturn()
        {
            AfterReset();
            yield break;
        }

        public IEnumerator ResetSmoothly(float time)
        {
            yield return this.StartSmartRoutine(MoveAndRotate(lastParentForward, lastParentPos, lastCoords, time, 0));
            AfterReset();
        }

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        private void Start()
        {
            GameObject gObj = new GameObject("CameraParent");
            transform.parent = gObj.transform;

            lastCoords = new SphericalCoordinates(startingCoordinates);
            lookingMode = LookingModeType.Reset;

            ResetLooking();

            smoothMove = new SmoothUnit(SmoothUnit.SmoothUnitType.Vec2, smoothRate);
            smoothEnlarge = new SmoothUnit(SmoothUnit.SmoothUnitType.Float, smoothRate);

            UpdatePosition();
        }

        // Update is called once per frame
        private void Update()
        {
            if (lookingMode == LookingModeType.Reset && !UpdateInput())
            {
                IsCameraMoving = false;
                return;
            }

            IsCameraMoving = true;

            UpdatePosition();
        }

        private float EnlargeSpeedNormalized(float radius)
        {
            float lambda = -enlargingRatio*radius + (radius - 1)*(6 + enlargingRatio);
            return -enlargingRatio*(6 + enlargingRatio + lambda)/(6 + lambda);
        }

        private bool UpdateInput()
        {
            smoothEnlarge.Add(InputController.Instance.Enlarge);
            float enlarge = smoothEnlarge.GetSmoothFloat();

            bool enlarged = Mathf.Abs(enlarge) > 0.01f;

            if (enlarged)
            {
                float speed = enlargingSpeedMin +
                              (enlargingSpeedMax - enlargingSpeedMin)*
                              EnlargeSpeedNormalized((Coordinates.radius - minimum.radius)/
                                                     (maximum.radius - minimum.radius));
                float newRadius = Coordinates.radius + enlarge*speed;

                if (newRadius > maximum.radius)
                    RadiusMaximumAchieved();
                else if (newRadius < minimum.radius)
                    RadiusMinimumAchieved();

                Coordinates.radius = newRadius;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            smoothMove.Add(InputController.Instance.Move * AndroidCameraLookSensitivity);
#else
            smoothMove.Add(InputController.Instance.Move);
#endif
            Vector2 delta = smoothMove.GetSmoothVector2();

            Coordinates.azimuthAngle += delta.x*movingSpeed.x;
            Coordinates.polarAngle += delta.y*movingSpeed.y;

            Coordinates.Limit(minimum, maximum);

            return enlarged || delta != Vector2.zero;
        }

        private void RadiusMinimumAchieved()
        {
            if (OnRadiusMinimumAchieved != null)
                OnRadiusMinimumAchieved();
        }

        private void RadiusMaximumAchieved()
        {
            if (OnRadiusMaximumAchieved != null)
                OnRadiusMaximumAchieved();
        }

        private void UpdatePosition()
        {
            transform.localPosition =
                new Vector3(
                    Coordinates.radius*Mathf.Sin(Coordinates.polarAngle)*Mathf.Cos(Coordinates.azimuthAngle),
                    Coordinates.radius*Mathf.Cos(Coordinates.polarAngle),
                    Coordinates.radius*Mathf.Sin(Coordinates.polarAngle)*Mathf.Sin(Coordinates.azimuthAngle));
            //Debug.Log(string.Format("UpdatePosition: localPosition({0},{1},{2}), coordinates({3})",
            //						transform.localPosition.x,
            //						transform.localPosition.y,
            //						transform.localPosition.z,
            //						Coordinates));

            transform.LookAt(transform.parent);
        }

    }
}
