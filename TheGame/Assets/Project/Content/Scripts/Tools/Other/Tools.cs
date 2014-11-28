#define UIToolkit

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityTools.Other
{
	public static class Tools
	{

		/// <summary>
		/// Starts radical routine.
		/// </summary>
		/// <param name="enumerator">Routine represented as IEnumerator.</param>
		/// <param name="layer">Layer of routines.</param>
		/// <returns>Created coroutine.</returns>
		public static Coroutine StartSmartRoutine(IEnumerator enumerator, int layer = 0)
		{
			return RoutinesManager.Instance.StartRoutine(RadicalRoutine.Run(enumerator), layer);
		}

		/// <summary>
		/// Starts radical routine.
		/// </summary>
		/// <param name="enumerator">Routine represented as IEnumerator.</param>
		/// <param name="onFinished">Action that is called on routine finished.</param>
		/// <param name="layer">Layer of routines.</param>
		/// <returns>Created coroutine.</returns>
		public static Coroutine StartSmartRoutine(IEnumerator enumerator, Action<bool> onFinished, int layer = 0)
		{
			RadicalRoutine routine = RadicalRoutine.Create(enumerator, layer);
			routine.Finished += onFinished;
			return RoutinesManager.Instance.StartRoutine(routine.enumerator, layer);
		}

		/// <summary>
		/// Starts radical routine.
		/// </summary>
		/// <param name="enumerator">Routine represented as IEnumerator.</param>
		/// <param name="routine">Created radical routine.</param>
		/// <param name="layer">Layer of routines.</param>
		/// <returns>Created coroutine.</returns>
		public static Coroutine StartSmartRoutine(IEnumerator enumerator, out RadicalRoutine routine, int layer = 0)
		{
			routine = RadicalRoutine.Create(enumerator, layer);
			return RoutinesManager.Instance.StartRoutine(routine.enumerator, layer);
		}

		/// <summary>
		/// Starts radical routine.
		/// </summary>
		/// <param name="behaviour">Behaviour that runs a routine.</param>
		/// <param name="enumerator">Routine represented as IEnumerator.</param>
		/// <returns>Created coroutine.</returns>
		public static Coroutine StartSmartRoutine(this MonoBehaviour behaviour, IEnumerator enumerator)
		{
			return behaviour.StartCoroutine(RadicalRoutine.Run(enumerator));
		}

		/// <summary>
		/// Starts radical routine.
		/// </summary>
		/// <param name="behaviour">Behaviour that runs a routine.</param>
		/// <param name="enumerator">Routine represented as IEnumerator.</param>
		/// <param name="onFinished">Action that is called on routine finished.</param>
		/// <returns>Created coroutine.</returns>
		public static Coroutine StartSmartRoutine(this MonoBehaviour behaviour, IEnumerator enumerator,
		                                          Action<bool> onFinished)
		{
			RadicalRoutine routine = RadicalRoutine.Create(enumerator, 0);
			routine.Finished += onFinished;
			return behaviour.StartCoroutine(routine.enumerator);
		}

		/// <summary>
		/// Starts radical routine.
		/// </summary>
		/// <param name="behaviour">Behaviour that runs a routine.</param>
		/// <param name="enumerator">Routine represented as IEnumerator.</param>
		/// <param name="routine">Created radical routine.</param>
		/// <returns>Created coroutine.</returns>
		public static Coroutine StartSmartRoutine(this MonoBehaviour behaviour, IEnumerator enumerator,
		                                          out RadicalRoutine routine)
		{
			routine = RadicalRoutine.Create(enumerator, 0);
			return behaviour.StartCoroutine(routine.enumerator);
		}


		/// <summary>
		/// Stops all radical routines.
		/// </summary>
		public static void StopAllRoutines()
		{
			//GameController.Instance.StopAllCoroutines();
			RadicalRoutine.CancelAll();
		}


		/// <summary>
		/// Stops radical routines at the specified layer.
		/// </summary>
		/// <param name="layer">Routines layer.</param>
		public static void StopSmartRoutines(int layer = 0)
		{
			RoutinesManager.Instance.StopRoutines(layer);
		}

		/// <summary>
		/// Stops and cancel radical routines and coroutine returns at the specified layer.
		/// </summary>
		/// <param name="layer">Routines layer.</param>
		public static void StopRoutines(int layer)
		{
			StopSmartRoutines(layer);
			RadicalRoutine.CancelLayer(layer);
		}

		/// <summary>
		/// Contains info about button.
		/// </summary>
		public class ListButtonInfo
		{

			/// <summary>
			/// Button's label.
			/// </summary>
			public string label;

			/// <summary>
			/// On click action.
			/// </summary>
			public Action action;

			/// <summary>
			/// Button's color.
			/// </summary>
			public Color contentColor = Color.white;

		}

		/// <summary>
		/// Displays Unity GUI buttons list.
		/// </summary>
		/// <param name="rect">List bounds.</param>
		/// <param name="buttonSize">Button width/height (depends on orientation).</param>
		/// <param name="info">Buttons info.</param>
		/// <param name="horizontal">Orientation (true - horizontal, false - vertical).</param>
		/// <param name="autoSelect">Auto select first button.</param>
		public static void DisplayButtonsList(Rect rect, float buttonSize, List<ListButtonInfo> info, bool horizontal,
		                                      bool autoSelect)
		{
			if (autoSelect && info.Count == 1)
			{
				info[0].action();
				return;
			}

			for (int i = 0; i != info.Count; i++)
			{
				GUI.contentColor = info[i].contentColor;

				if (GUI.Button(new Rect(rect.x + (horizontal ? (i + 0.5f)*(rect.width/info.Count) - buttonSize/2f : 0),
				                        rect.y + (horizontal ? 0 : (i + 0.5f)*(rect.height/info.Count) - buttonSize/2f),
				                        (horizontal ? buttonSize : rect.width),
				                        (horizontal ? rect.height : buttonSize)),
				               info[i].label))
					info[i].action();
			}
		}

		public static IEnumerator EnumeratorInvoke(Delegate[] delegates)
		{
			for (int i = 0; i != delegates.Length; i++)
			{
				yield return StartSmartRoutine(delegates[i].DynamicInvoke() as IEnumerator);
			}
		}

		/// <summary>
		/// Runs an action after specified time.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="action">Action.</param>
		/// <returns>Routine enumerator.</returns>
		private static IEnumerator StartWithDelayEnumerator(float time, Action action)
		{
			yield return new WaitForSeconds(time);
			action();
		}

		/// <summary>
		/// Runs an action after specified time.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="action">Action.</param>
		public static void StartWithDelay(float time, Action action)
		{
			StartSmartRoutine(StartWithDelayEnumerator(time, action));
		}

		#region Scale

		private const float DefaultScreenHeight = 480.0f;

		public static float ScreenHeightCoefficient
		{
			get { return Screen.height/DefaultScreenHeight; }
		}

		public static float ScaleScalar(float scalar)
		{
			return scalar*ScreenHeightCoefficient;
		}

		public static Vector3 ScaleVector(Vector3 vec)
		{
			return new Vector3(ScaleScalar(vec.x), ScaleScalar(vec.y), ScaleScalar(vec.z));
		}

		public static Rect ScaleRect(Rect rect)
		{
			return ScaleRect(rect, false);
		}

		public class BoolVector2
		{
			public bool x;
			public bool y;
		}

		public static Rect ScaleRect(Rect rect, bool forceNegative)
		{
			return ScaleRect(rect, new BoolVector2 {x = forceNegative, y = forceNegative});
		}

		public static Rect ScaleRect(Rect rect, BoolVector2 forceNegative)
		{
			float screenCoef = ScreenHeightCoefficient;

			return new Rect((rect.x >= 0 || forceNegative.x ? rect.x : rect.x + Screen.width/screenCoef)*screenCoef,
			                (rect.y >= 0 || forceNegative.y ? rect.y : rect.y + Screen.height/screenCoef)*screenCoef,
			                (rect.width >= 0 || forceNegative.x ? rect.width : rect.width + Screen.width/screenCoef)*screenCoef,
			                (rect.height >= 0 || forceNegative.y ? rect.height : rect.height + Screen.height/screenCoef)*
			                screenCoef);
		}

		public static Rect ScaleRect(Rect rect, bool centeredX, bool centeredY)
		{
			return ScaleRect(rect, centeredX, centeredY, false);
		}

		public static Rect ScaleRect(Rect rect, bool centeredX, bool centeredY, bool forceNegative)
		{
			Rect scaled = ScaleRect(rect, forceNegative);

			return new Rect(centeredX ? Screen.width/2.0f - scaled.width/2.0f + rect.x*ScreenHeightCoefficient : scaled.x,
			                centeredY ? Screen.height/2.0f - scaled.height/2.0f + rect.y*ScreenHeightCoefficient : scaled.y,
			                scaled.width,
			                scaled.height);
		}

		public static Rect ScaledRect(int x, int y, int width, int height)
		{
			return ScaledRect(x, y, width, height, false);
		}

		public static Rect ScaledRect(int x, int y, int width, int height, bool forceNegative)
		{
			return ScaleRect(new Rect(x, y, width, height), forceNegative);
		}

		public static Rect ScaledRect(float x, float y, float width, float height)
		{
			return new Rect(Mathf.CeilToInt(x*Screen.width),
			                Mathf.CeilToInt(y*Screen.height),
			                Mathf.CeilToInt(width*Screen.width),
			                Mathf.CeilToInt(height*Screen.height));
		}

		public static Rect ScaledRect(int x, int y, int width, int height, bool centeredX, bool centeredY)
		{
			return ScaleRect(new Rect(x, y, width, height), centeredX, centeredY, false);
		}

		public static Rect ScaledRect(float x, float y, float width, float height, bool centeredX, bool centeredY)
		{
			return ScaleRect(new Rect(x, y, width, height), centeredX, centeredY, false);
		}

		public static Rect ScaledRect(float x, float y, float width, float height, bool centeredX, bool centeredY,
		                              bool forceNegative)
		{
			return ScaleRect(new Rect(x, y, width, height), centeredX, centeredY, forceNegative);
		}

		#endregion Scale

		/*#region UI
		 * Unity tools does not include ngui as a submodule, so it cant relyon ngui.
	
		public static void ScaleToHeight (this UISprite sprite, float height) {
			float scaledHeight = ScaleScalar(height);
			float scaleCoef = scaledHeight / sprite.height;
			sprite.scale = new Vector3(scaleCoef, scaleCoef);
		}
	
		public static void ScaleTo (this UISprite sprite, Vector2 size) {
			Vector2 scaledSize = ScaleVector(size);
			Vector2 scaleCoef = new Vector2(scaledSize.x / sprite.width, scaledSize.y / sprite.height);
			sprite.scale = scaleCoef;
		}
	
		public static Vector3 GetScaleToHeight (float objectHeight, float height) {
			float scaledHeight = ScaleScalar(height);
			float scaleCoef = scaledHeight / objectHeight;
			return new Vector3(scaleCoef, scaleCoef, 1);
		}
	
		#endregion UI*/

		#region Math

		public static Vector3[] CalculatePositioningPoints(int count, float length)
		{

			float beta = 360f/count;
			float radius = length/Mathf.Sqrt(2*(1 - Mathf.Cos(beta*Mathf.Deg2Rad)));

			Vector3[] positions = new Vector3[count];

			positions = new Vector3[count];
			positions[0] = new Vector3(0, 0, radius);

			for (int i = 1; i != count; i++)
			{
				positions[i] = Quaternion.Euler(0, beta*i, 0)*positions[0];
			}

			return positions;
		}

		public static int[,] RotateMatrixRight90(int[,] matrix)
		{
			int[,] result = new int[matrix.GetLength(1),matrix.GetLength(0)];

			for (int i = 0; i != result.GetLength(0); i++)
			{
				for (int j = 0; j != result.GetLength(1); j++)
				{
					result[i, j] = matrix[matrix.GetLength(0) - j - 1, i];
				}
			}

			return result;
		}

		public static int[,] RotateMatrixLeft90(int[,] matrix)
		{
			int[,] result = new int[matrix.GetLength(1),matrix.GetLength(0)];

			for (int i = 0; i != result.GetLength(0); i++)
			{
				for (int j = 0; j != result.GetLength(1); j++)
				{
					result[i, j] = matrix[j, matrix.GetLength(1) - i - 1];
				}
			}

			return result;
		}

		public static int[,] RotateMatrix180(int[,] matrix)
		{
			int[,] result = new int[matrix.GetLength(0),matrix.GetLength(1)];

			for (int i = 0; i != result.GetLength(0); i++)
			{
				for (int j = 0; j != result.GetLength(1); j++)
				{
					result[i, j] = matrix[matrix.GetLength(0) - i - 1, matrix.GetLength(1) - j - 1];
				}
			}

			return result;
		}

		public static int[,] MatrixDiagonalReflect(int[,] matrix)
		{
			int[,] result = new int[matrix.GetLength(1),matrix.GetLength(0)];

			for (int i = 0; i != matrix.GetLength(1); i++)
			{
				for (int j = 0; j != matrix.GetLength(0); j++)
				{
					result[i, j] += matrix[j, i];
				}
			}

			return result;
		}

		public static int Sign(int x)
		{
			if (x == 0)
				return 0;

			return x > 0 ? 1 : -1;
		}

		public static int Sign(float x)
		{
			return x >= 0 ? 1 : -1;
		}

		public static int IntFromDistribution(List<float> distribution)
		{
			float rnd = Random.value*distribution.Sum(f => f);
			float sum = 0;

			for (int i = 0; i != distribution.Count; i++)
			{
				if (rnd <= sum)
					return i - 1;

				sum += distribution[i];
			}

			return distribution.Count - 1;
		}

		/// <summary>
		/// Calculate direction of angle between vectors
		/// </summary>
		/// <param name="forward">Forward direction</param>
		/// <param name="target">Target direction</param>
		/// <param name="up">Up direction</param>
		/// <returns>-1 when to the left, 1 to the right, and 0 for forward/backward</returns>
		public static int AngleDir(Vector3 forward, Vector3 target, Vector3 up)
		{
			Vector3 perp = Vector3.Cross(forward, target);
			float dir = Vector3.Dot(perp, up);

			return dir >= 0.0f ? 1 : -1;
		}

		public static float Angle(Vector3 forward, Vector3 target, Vector3 up)
		{
			return Vector3.Angle(forward, target)*AngleDir(forward, target, up);
		}

		public static float RotationAngle(Transform objectToRotate, Vector3 target, Vector3 up)
		{

			// Save Quaternion before LookAt
			Quaternion beforeLookAt = objectToRotate.rotation;

			// LookAt
			objectToRotate.LookAt(target);

			// Save direction after LookAt
			Vector3 afterLookAt = objectToRotate.forward;

			// Revert Quaternion to before LookAt state
			objectToRotate.rotation = beforeLookAt;

			// Calculate Angle
			return Angle(objectToRotate.forward, afterLookAt, up);

		}


		#endregion Math

		#region Custom Linq

		// ReSharper disable LoopCanBeConvertedToQuery

		public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			int sum = 0;

			foreach (TSource obj in source)
			{
				sum += selector(obj);
			}

			return sum;
		}

		public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			float sum = 0;

			foreach (TSource obj in source)
			{
				sum += selector(obj);
			}

			return sum;
		}

		public static TResult Aggregate<TSource, TResult>(this IEnumerable<TSource> source, TResult seed,
		                                                  Func<TResult, TSource, TResult> aggregator)
		{
			TResult result = seed;

			foreach (TSource obj in source)
			{
				result = aggregator(result, obj);
			}

			return result;
		}

		public static TResult Aggregate<TSource, TResult>(this IEnumerable<TSource> source,
		                                                  Func<TResult, TSource, TResult> aggregator)
		{
			TResult result = default(TResult);

			foreach (TSource obj in source)
			{
				result = aggregator(result, obj);
			}

			return result;
		}

		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			List<TSource> result = new List<TSource>();

			foreach (TSource obj in source)
			{
				if (predicate(obj))
					result.Add(obj);
			}

			return result;
		}

		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			List<TSource> result = new List<TSource>();

			foreach (TSource obj in source)
			{
				result.Add(obj);
			}

			return result;
		}

		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
		{
			List<TSource> list = source.ToList();
			TSource[] result = new TSource[list.Count];

			for (int i = 0; i != result.Length; i++)
			{
				result[i] = list[i];
			}

			return result;
		}

		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			return source.Where(predicate).First();
		}

		public static TSource First<TSource>(this IEnumerable<TSource> source)
		{
			IList<TSource> list = source as IList<TSource>;

			if (list == null)
			{
				throw new Exception("IList == null!");
			}

			if (list.Count == 0)
			{
				throw new Exception("No elements satisfying the predicate!");
			}

			return list[0];
		}

		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			return source.Where(predicate).FirstOrDefault();
		}

		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			IList<TSource> list = source as IList<TSource>;

			if (list == null)
			{
				throw new Exception("IList == null!");
			}

			if (list.Count == 0)
			{
				return default(TSource);
			}

			return list[0];
		}

		public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			return source.Where(predicate).Count();
		}

		public static int Count<TSource>(this IEnumerable<TSource> source)
		{
			IList<TSource> list = source as IList<TSource>;

			if (list == null)
			{
				throw new Exception("IList == null!");
			}

			return list.Count;
		}

		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source,
		                                                            Func<TSource, TResult> selector)
		{
			List<TResult> result = new List<TResult>();

			foreach (TSource obj in source)
			{
				result.Add(selector(obj));
			}

			return result;
		}

		public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource obj in source)
			{
				if (!predicate(obj))
					return false;
			}

			return true;
		}

		public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource obj in source)
			{
				if (predicate(obj))
					return true;
			}

			return false;
		}

		public static bool Any<TSource>(this IEnumerable<TSource> source)
		{
			return source.Any(src => true);
		}

		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source,
		                                                                Func<TSource, IEnumerable<TResult>> selector)
		{
			IList<TResult> result = new List<TResult>();

			foreach (TSource obj in source)
			{
				foreach (TResult rObj in selector(obj))
				{
					result.Add(rObj);
				}
			}

			return result;
		}

		public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			int min = int.MaxValue;

			foreach (TSource obj in source)
			{
				int value = selector(obj);

				if (value < min)
					min = value;
			}

			return min;
		}

		public static int Min(this IEnumerable<int> source)
		{
			return source.Min(i => i);
		}

		public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			float min = float.PositiveInfinity;

			foreach (TSource obj in source)
			{
				float value = selector(obj);

				if (value < min)
					min = value;
			}

			return min;
		}

		public static float Min(this IEnumerable<float> source)
		{
			return source.Min(i => i);
		}

		public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			int max = int.MinValue;

			foreach (TSource obj in source)
			{
				int value = selector(obj);

				if (value > max)
					max = value;
			}

			return max;
		}

		public static int Max(this IEnumerable<int> source)
		{
			return source.Max(i => i);
		}

		public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			float max = float.MinValue;

			foreach (TSource obj in source)
			{
				float value = selector(obj);

				if (value > max)
					max = value;
			}

			return max;
		}

		public static float Max(this IEnumerable<float> source)
		{
			return source.Max(i => i);
		}

		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			IList<TSource> result = new List<TSource>();
			IList<TSource> lSecond = second as IList<TSource>;

			if (lSecond == null)
				throw new Exception("IList == null!");


			foreach (TSource obj in first)
			{
				if (lSecond.IndexOf(obj) == -1)
					result.Add(obj);
			}

			return result;
		}

		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
		{
			IList<TResult> result = new List<TResult>();

			foreach (object obj in source)
			{
				result.Add((TResult) obj);
			}

			return result;
		}

		public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			IEnumerable<TSource> enumerable = source.ToList();
			return enumerable.Sum(selector)/enumerable.Count();
		}

		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
		{
			int i = 0;

			foreach (TSource obj in source)
			{
				if (i == index)
					return obj;

				i++;
			}

			throw new Exception("No such element!");
		}

		public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			int i = 0;

			foreach (TSource obj in source)
			{
				if (obj.Equals(element))
					return i;

				i++;
			}

			return -1;
		}

		#endregion Custom Linq

		#region Debug

		public static void DrawDebugRect2D(Rect rect, Color color)
		{
			Vector3 p1 = ScreenToWorld2DPoint(new Vector2(rect.x, rect.y));
			Vector3 p2 = ScreenToWorld2DPoint(new Vector2(rect.x + rect.width, rect.y));
			Vector3 p3 = ScreenToWorld2DPoint(new Vector2(rect.x + rect.width, rect.y - rect.height));
			Vector3 p4 = ScreenToWorld2DPoint(new Vector2(rect.x, rect.y - rect.height));

			Debug.DrawLine(p1, p2, color);
			Debug.DrawLine(p2, p3, color);
			Debug.DrawLine(p3, p4, color);
			Debug.DrawLine(p4, p1, color);
		}

		public static void DrawDebugCross2D(Vector2 pos, float size, Color color)
		{
			Vector3 p1 = ScreenToWorld2DPoint(pos + Vector2.right*size + Vector2.up*size);
			Vector3 p2 = ScreenToWorld2DPoint(pos + Vector2.right*size - Vector2.up*size);
			Vector3 p3 = ScreenToWorld2DPoint(pos - Vector2.right*size - Vector2.up*size);
			Vector3 p4 = ScreenToWorld2DPoint(pos - Vector2.right*size + Vector2.up*size);

			Debug.DrawLine(p1, p3, color);
			Debug.DrawLine(p2, p4, color);
		}

		public static void DrawDebugCircle2D(Vector2 pos, float radius, Color color)
		{
			int pointsCount = (int) radius;
			Vector3 prevPt = ScreenToWorld2DPoint(new Vector2(pos.x + radius, pos.y));
			for (int i = 1; i <= pointsCount; i++)
			{
				float angle = i*Mathf.PI*2/(float) pointsCount;
				Vector3 currPt = ScreenToWorld2DPoint(new Vector2(
					                                      pos.x + radius*Mathf.Cos(angle),
					                                      pos.y - radius*Mathf.Sin(angle)));
				Debug.DrawLine(prevPt, currPt, color);
				prevPt = currPt;
			}
		}

		public static void DrawDebugLine2D(Vector2 start, Vector2 end, Color color)
		{
			Vector3 pos1 = ScreenToWorld2DPoint(start);
			Vector3 pos2 = ScreenToWorld2DPoint(end);
			Debug.DrawLine(pos1, pos2, color);
		}

		private static Vector3 ScreenToWorld2DPoint(Vector2 pos)
		{
			return pos; //Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Camera.main.nearClipPlane + 1));
		}

		#endregion Debug

		public static string MatrixToText(int[,] matrix)
		{
			string str = "";

			for (int i = 0; i != matrix.GetLength(0); i++)
			{
				str += "{ ";

				for (int j = 0; j != matrix.GetLength(1); j++)
				{
					str += matrix[i, j] + ", ";
				}

				str += "}\n";
			}

			return str;
		}

		public static List<T> Unique<T>(this IEnumerable<T> list)
		{
			List<T> result = new List<T>();

			foreach (T item in list)
			{
				if (result.Contains(item))
					continue;

				result.Add(item);
			}

			return result;
		}

		public static string ListToText(IEnumerable<float> list)
		{
			return list.Aggregate("", (current, f) => current + String.Format("{0:0.000}, ", f));
		}

		public static string ListToText(IEnumerable<int> list)
		{
			return list.Aggregate("", (current, f) => current + String.Format("{0}, ", f));
		}

		/// <summary>
		/// Sort the list
		/// </summary>
		/// <returns>Sorted list from greater to lesser</returns>
		public static List<T> CustomSort<T>(this IEnumerable<T> list, Func<T, int> selector)
		{
			List<T> result = new List<T>();

			List<T> list1 = new List<T>();
			list1.AddRange(list);

			while (list1.Count != 1)
			{
				int maxIndex = 0;

				for (int i = 0; i != list1.Count; i++)
				{
					if (selector(list1[i]) <= selector(list1[maxIndex]))
						continue;

					maxIndex = i;
				}

				result.Add(list1[maxIndex]);
				list1.RemoveAt(maxIndex);
			}

			result.Add(list1[0]);

			return result;
		}

		public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
		{
			IList<TSource> list = source as IList<TSource>;

			if (list == null)
			{
				throw new Exception("IList == null!");
			}

			List<TSource> result = new List<TSource>();

			while (list.Count > 0)
			{
				int rIndex = Random.Range(0, list.Count);
				result.Add(list[rIndex]);
				list.RemoveAt(rIndex);
			}

			return result;
		}

		public static Queue<TSource> ToQueue<TSource>(this IEnumerable<TSource> source)
		{
			Queue<TSource> result = new Queue<TSource>();

			foreach (TSource src in source)
			{
				result.Enqueue(src);
			}

			return result;
		}

		#region Serialization

		public class InOutInfo
		{

			private readonly Dictionary<string, object> data;

			public InOutInfo()
			{
				data = new Dictionary<string, object>();
			}

			public T GetDatum<T>(string key)
			{
				if (data.ContainsKey(key))
					return (T) data[key];

				return default(T);
			}

			public void AddDatum(string key, object obj)
			{
				data.Add(key, obj);
			}

			public void Clear()
			{
				data.Clear();
			}

			public override string ToString()
			{
				return string.Format("{{ {0}}}",
				                     data.Aggregate("",
				                                    (current, pair) =>
				                                    current + string.Format("{0} : '{1}', ", pair.Key, pair.Value)));
			}

		    public bool HasDatum(string key)
		    {
		        return data.ContainsKey(key);
		    }
	
			

		}

		public class DataBuffer
		{

			private enum DataType : byte
			{
				Int,
				Float,
				Bool,
				String,
			}

			private class Datum
			{
				public byte[] Bytes
				{
					get { return bytes.ToArray(); }
				}

				public int Size
				{
					get
					{
						switch (Type)
						{
							case DataType.Int:
								return sizeof (int);
							case DataType.Float:
								return sizeof (double);
							case DataType.Bool:
								return sizeof (bool);
							case DataType.String:
								return sizeof (int) + ((string) Value).Length*sizeof (char);
						}

						return 0;
					}
				}

				public object Value { get; private set; }
				public DataType Type { get; private set; }

				private readonly List<byte> bytes;

				private Datum()
				{
					bytes = new List<byte>();
				}

				public Datum(int value) : this()
				{
					bytes.Add((byte) DataType.Int);
					bytes.AddRange(BitConverter.GetBytes(value));
				}

				public Datum(float value) : this()
				{
					bytes.Add((byte) DataType.Float);
					bytes.AddRange(BitConverter.GetBytes(Convert.ToDouble(value)));
				}

				public Datum(bool value) : this()
				{
					bytes.Add((byte) DataType.Bool);
					bytes.AddRange(BitConverter.GetBytes(value));
				}

				public Datum(string value) : this()
				{
					bytes.Add((byte) DataType.String);
					bytes.AddRange(BitConverter.GetBytes(value.Length));
					bytes.AddRange(Encoding.Unicode.GetBytes(value));
				}

				public Datum(byte[] buffer, int offset)
				{
					bytes = new List<byte>();

					for (int i = 0; i != buffer.Length - offset; i++)
					{
						bytes.Add(buffer[i + offset]);
					}

					Value = GetData();
				}

				private object GetData()
				{
					Type = (DataType) bytes[0];

					switch (Type)
					{
						case DataType.Int:
							return BitConverter.ToInt32(Bytes, 1);
						case DataType.Float:
							return BitConverter.ToDouble(Bytes, 1);
						case DataType.Bool:
							return BitConverter.ToBoolean(Bytes, 1);
						case DataType.String:
							int length = BitConverter.ToInt32(Bytes, 1);
							return Encoding.Unicode.GetString(Bytes, 1 + sizeof (int), length*sizeof (char));
					}

					return null;
				}
			}

			public bool IsEnd
			{
				get { return data.Count == 0; }
			}

			public byte[] Bytes
			{
				get
				{
					List<byte> bytes = new List<byte>();

					foreach (Datum datum in data)
					{
						bytes.AddRange(datum.Bytes);
					}

					return bytes.ToArray();
				}
			}

			public string StringFromBytes
			{
				get
				{
					BinaryFormatter bf = new BinaryFormatter();
					MemoryStream stream = new MemoryStream();
					bf.Serialize(stream, Bytes);
					return Convert.ToBase64String(stream.ToArray());
				}
			}

			private readonly List<Datum> data;

			public DataBuffer()
			{
				data = new List<Datum>();
			}

			public DataBuffer(byte[] buffer) : this()
			{
				int offset = 0;

				while (offset < buffer.Length)
				{
					Datum datum = new Datum(buffer, offset);
					data.Add(datum);

					offset += 1 + datum.Size;
				}
			}

			public DataBuffer(string s)
				: this((byte[]) (new BinaryFormatter()).Deserialize(new MemoryStream(Convert.FromBase64String(s))))
			{
			}

			public void Write(int value)
			{
				data.Add(new Datum(value));
			}

			public void Write(float value)
			{
				data.Add(new Datum(value));
			}

			public void Write(bool value)
			{
				data.Add(new Datum(value));
			}

			public void Write(string value)
			{
				data.Add(new Datum(value));
			}

			private void Write(object obj)
			{
				if (obj is int)
					Write((int) obj);
				else if (obj is float)
					Write((float) obj);
				else if (obj is bool)
					Write((bool) obj);
				else if (obj is string)
					Write(obj as string);
				else if (obj is ISerializable)
					Write(obj as ISerializable);
				else
					Debug.LogError(string.Format("Type '{0}' is not supported", obj.GetType()));
			}

			public void Write<T>(List<T> list)
			{
				Write(list.Count);

				for (int i = 0; i != list.Count; i++)
				{
					Write(list[i]);
				}
			}

			public void Write(ISerializable obj)
			{
				DataBuffer buffer = new DataBuffer();
				obj.Serialize(buffer);

				Write(buffer.StringFromBytes);
			}

			public object Read()
			{
				object obj = data[0].Value;
				data.RemoveAt(0);

				return obj;
			}

			public int ReadInt()
			{
				return (int) Read();
			}

			public float ReadFloat()
			{
				return (float) (double) Read();
			}

			public bool ReadBool()
			{
				return (bool) Read();
			}

			public string ReadString()
			{
				return (string) Read();
			}

			public List<T> ReadList<T>()
			{
				List<T> result = new List<T>();
				int count = ReadInt();

				for (int i = 0; i != count; i++)
				{
					result.Add((T) Read());
				}

				return result;
			}

			public delegate void BeforeDeserializationFunction(ISerializable obj);

			public List<T> ReadSerializableList<T>(BeforeDeserializationFunction foo = null) where T : ISerializable, new()
			{
				List<T> result = new List<T>();
				int count = ReadInt();

				for (int i = 0; i != count; i++)
				{
					result.Add(ReadSerializable<T>(foo));
				}

				return result;
			}

			public T ReadSerializable<T>(BeforeDeserializationFunction foo = null) where T : ISerializable, new()
			{
				DataBuffer buffer = new DataBuffer(ReadString());

				T result = new T();

				if (foo != null)
					foo(result);

				result.Deserialize(buffer);

				return result;
			}

		}

		#endregion Serialization

		#region Mesh

		public static void SetPlaneTexture(Transform plane, Texture2D texture)
		{
			Vector2 planeSize = new Vector2(plane.renderer.material.mainTexture.width,
			                                plane.renderer.material.mainTexture.height);
			Vector2 planeNewSize = new Vector2(texture.width, texture.height);

			Vector2 planeScale = new Vector2(plane.localScale.x, plane.localScale.y);
			Vector2 planeNewScale = new Vector2(planeScale.x*(planeNewSize.x/planeSize.x),
			                                    planeScale.y*(planeNewSize.y/planeSize.y));

			plane.localScale = new Vector3(planeNewScale.x, planeNewScale.y, plane.localScale.z);

			plane.renderer.material.mainTexture = texture;
		}

		public static Material CreateMaterial(string shader, Texture2D texture)
		{
			Material result = new Material(Shader.Find(shader)) {mainTexture = texture};
			return result;
		}

		public static Transform CreatePlaneMesh(Transform parent, string name, Rect rect, bool horizontal, Material material)
		{
			Transform result = CreatePlaneMesh(name, new Vector2(rect.width, rect.height), horizontal);

			result.parent = parent;
			result.localPosition = new Vector3(rect.x, horizontal ? 0f : rect.y, horizontal ? rect.y : 0f);
			result.renderer.material = material;

			return result;
		}

		public static Transform CreatePlaneMesh(string name, Vector2 size, bool horizontal)
		{
			Vector3[] vertices = new Vector3[4];
			Vector3[] normals = new Vector3[4];
			Vector2[] uv = new Vector2[4];
			int[] triangles = new int[6];

			Mesh mesh = new Mesh();

			GameObject newObj = new GameObject();
			newObj.name = name;

			newObj.AddComponent<MeshFilter>();
			newObj.GetComponent<MeshFilter>().mesh = mesh;

			Vector3 toSide = Vector3.right;
			Vector3 toUp = horizontal ? Vector3.forward : Vector3.up;

			size /= 2.0f;

			vertices[0] = (-size.x*toSide + size.y*toUp);
			vertices[1] = (size.x*toSide + size.y*toUp);
			vertices[2] = (size.x*toSide - size.y*toUp);
			vertices[3] = (-size.x*toSide - size.y*toUp);

			normals[0] = horizontal ? Vector3.up : -Vector3.forward;
			normals[1] = horizontal ? Vector3.up : -Vector3.forward;
			normals[2] = horizontal ? Vector3.up : -Vector3.forward;
			normals[3] = horizontal ? Vector3.up : -Vector3.forward;

			uv[0] = new Vector2(0, 1);
			uv[1] = new Vector2(1, 1);
			uv[2] = new Vector2(1, 0);
			uv[3] = new Vector2(0, 0);

			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 3;
			triangles[3] = 3;
			triangles[4] = 1;
			triangles[5] = 2;

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			newObj.AddComponent<MeshRenderer>();

			return newObj.transform;
		}

		#endregion Mesh

		public static void LookAt2D(this Transform transform, Transform target)
		{
			transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
		}

		public static void LookAt2D(this Transform transform, Vector3 target)
		{
			transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
		}


		#region Get interface from game object components

		public static T GetInterface<T>(this GameObject inObj) where T : class
		{
			if (!typeof (T).IsInterface)
			{
				Debug.LogError(typeof (T).ToString() + ": is not an actual interface!");
				return null;
			}

			return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
		}

		public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
		{
			if (!typeof (T).IsInterface)
			{
				Debug.LogError(typeof (T).ToString() + ": is not an actual interface!");
				return Enumerable.Empty<T>();
			}

			return inObj.GetComponents<Component>().OfType<T>();
		}

		#endregion

		#region Vector rotation

		public static Vector3 RotateX(this Vector3 v, float angle)

		{
			Vector3 vR = new Vector3(v.x, v.y, v.z);

			float sin = Mathf.Sin(angle);

			float cos = Mathf.Cos(angle);



			float ty = v.y;

			float tz = v.z;

			vR.y = (cos*ty) - (sin*tz);

			vR.z = (cos*tz) + (sin*ty);

			return vR;
		}



		public static Vector3 RotateY(this Vector3 v, float angle)

		{
			Vector3 vR = new Vector3(v.x, v.y, v.z);

			float sin = Mathf.Sin(angle);

			float cos = Mathf.Cos(angle);

			
			float tx = v.x;

			float tz = v.z;

			vR.x = (cos*tx) + (sin*tz);

			vR.z = (cos*tz) - (sin*tx);

			return vR;
		}

		public static Vector3 RotateZ(this Vector3 v, float angle)

		{
			Vector3 vR = new Vector3(v.x, v.y, v.z);

			float sin = Mathf.Sin(angle);

			float cos = Mathf.Cos(angle);



			float tx = v.x;

			float ty = v.y;

			vR.x = (cos*tx) - (sin*ty);

			vR.y = (cos*ty) + (sin*tx);

			return vR;
		}

		#endregion

	}

	public class WaitForRoutinesFinish : CoroutineReturn
	{

		private readonly bool[] isFinished;

		public WaitForRoutinesFinish(IEnumerator[] enumerators)
		{
			isFinished = new bool[enumerators.Length];

			for (int i = 0; i != enumerators.Length; i++)
			{
				isFinished[i] = false;

				int i1 = i;
				Tools.StartSmartRoutine(enumerators[i], b => isFinished[i1] = true);
			}
		}

		public override bool IsFinished
		{
			get { return isFinished.All(b => b); }
		}

	}
}