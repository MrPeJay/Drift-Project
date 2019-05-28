using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance of type " + typeof(T) + " will not be returned because the application is quitting.");
                return null;
            }
            lock (m_Lock)
            {
                if (m_Instance != null)
                {
                    return m_Instance;
                }
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                    {
                        return m_Instance = instances[0];
                    }
                    Debug.LogWarning("There should never be more than one [Singleton] Instance of type " + typeof(T) + " in the scene, but " + count + " were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                    {
                        Destroy(instances[i]);
                    }
                    return m_Instance = instances[0];
                }
                return null;
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}