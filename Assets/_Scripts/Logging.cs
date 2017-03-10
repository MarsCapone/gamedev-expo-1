using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Logging
{

	public enum LogLevel
	{
		DEBUG = 0,
		INFO = 1,
		WARNING = 2,
		ERROR = 3,
		CRITICAL = 4
	}

	public static LogLevel level = LogLevel.DEBUG;

	public static void Log (string message)
	{
		Logging.Log (LogLevel.INFO, message);
	}

	public static void Log (LogLevel level, string message)
	{
		if (level >= Logging.level) {
			UnityEngine.Debug.Log (string.Format ("[{0}]\t\t{1}", level, message));
		}
	}

	public static void Debug (string message)
	{
		Logging.Log (LogLevel.DEBUG, message);
	}

	public static void Info (string message)
	{
		Logging.Log (message);
	}

	public static void Warning (string message)
	{
		Logging.Log (LogLevel.WARNING, message);
	}

	public static void Error (string message)
	{
		Logging.Log (LogLevel.ERROR, message);
	}

	public static void Critical (string message)
	{
		Logging.Log (LogLevel.CRITICAL, message);
	}
}
