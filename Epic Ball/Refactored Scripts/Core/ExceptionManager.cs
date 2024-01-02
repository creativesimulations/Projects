using System;
using UnityEngine;
public class ExceptionManager : MonoBehaviour
{
    public static ExceptionManager instance;

    void Awake()
    {
        Application.logMessageReceived += LogCaughtException;
        instance = this;
    }
    /// <summary>
    /// Displays an exception error.
    /// </summary>
    /// <param name="logText"></param>
    /// <param name="stackTrace"></param>
    /// <param name="logType"></param>
    void LogCaughtException(string logText, string stackTrace, LogType logType)
    {
        if (logType == LogType.Exception)
        {
            Debug.LogError(logText);
        }
    }

    /// <summary>
    /// Sends an error message regarding a missing object.
    /// </summary>
    /// <param name="missingObject"></param> The object that is missing.
    /// <param name="scriptName"></param> The script that requires a reference to the missing object.
    /// <param name="objectWithScript"></param> The object where the script can be found.
    public void SendMissingObjectMessage(string missingObject, string scriptName, string objectWithScript)
    {
        Debug.LogError(missingObject + " is missing. Please add the desired" + missingObject + " to the" + scriptName + " script. It is located on the " + objectWithScript + " game object.");
    }
    /// <summary>
    /// Sends an error message regarding a missing component.
    /// </summary>
    /// <param name="missingComponent"></param> The component that is missing.
    /// <param name="scriptName"></param> The script that requires a reference to the missing component.
    /// <param name="objectWithScript"></param> The object where the script can be found.
    public void SendMissingComponentMessage(string missingComponent, string scriptName, string objectWithScript)
    {
        Debug.LogError("The " + scriptName + " script on the " + objectWithScript + " game object cannot find the " + missingComponent + ". Please check to make sure it is available.");
    }
    /// <summary>
    /// Sends an error message regarding an empty List or Array.
    /// </summary>
    /// <param name="emptyContainer"></param> The container that is empty.
    /// <param name="scriptName"></param> The script with the empty container.
    /// <param name="objectWithScript"></param> The object where the script can be found.
    public void SendEmptyContainerMessage(string emptyContainer, string scriptName, string objectWithScript)
    {
        Debug.LogError("The list/array " + emptyContainer + " in " + scriptName + " script on the " + objectWithScript + " game object is empty. Please populate the array/list.");
    }
}