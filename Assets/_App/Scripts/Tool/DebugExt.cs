﻿using UnityEngine;

internal static class DebugExt
{
	public static void Log(object obj, string message)
	{
		Debug.Log($"[{obj.GetType().Name}] {message}");	
	}
	
	public static void LogWarning(object obj, string message)
	{
		Debug.LogWarning($"[{obj.GetType().Name}] {message}");	
	}
}
