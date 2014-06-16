#if !UNITY_EDITOR 
#define DEBUG_LOG_OVERWRAP
#endif


using UnityEngine;

#if DEBUG_LOG_OVERWRAP
public static class Debug 
{
    static public void Break(){
        if( IsEnable() )    UnityEngine.Debug.Break();
    }


    #region DrawLine
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawLine(start, end);
	}
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawLine(start, end, color);
	}
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawLine(start, end, color, duration);
	}
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
	}
	#endregion DrawLine



	#region DrawRay
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawRay(start, dir);
	}
	public static void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawRay(start, dir, color);
	}
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawRay(start, dir, color, duration);
	}
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
	{
		if (IsEnable())
			UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
	}
	#endregion DrawRay


	#region log
	static public void Log(object message)
	{
		if (IsEnable())
			UnityEngine.Debug.Log(message);
	}
	static public void Log(object message, Object context)
	{
		if (IsEnable())
			UnityEngine.Debug.Log(message, context);
	}



	static public void LogWarning(object message)
	{
		if (IsEnable())
			UnityEngine.Debug.LogWarning(message);
	}
	static public void LogWarning(object message, Object context)
	{
		if (IsEnable())
			UnityEngine.Debug.LogWarning(message, context);
	}



	static public void LogError(object message)
	{
		if (IsEnable())
			UnityEngine.Debug.LogError(message);
	}
	static public void LogError(object message, Object context)
	{
		if (IsEnable())
			UnityEngine.Debug.LogError(message, context);
	}
	#endregion log



	static bool IsEnable(){ return UnityEngine.Debug.isDebugBuild; }
}
#endif