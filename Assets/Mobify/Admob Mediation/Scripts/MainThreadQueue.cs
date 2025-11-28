using UnityEngine;

public sealed class MainThreadQueue : MonoBehaviour
{
    private static readonly System.Collections.Concurrent.ConcurrentQueue<System.Action> _q = new System.Collections.Concurrent.ConcurrentQueue<System.Action>();
    public static void Enqueue(System.Action a) { if (a != null) _q.Enqueue(a); }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        new GameObject("MainThreadQueue").AddComponent<MainThreadQueue>();
        DontDestroyOnLoad(GameObject.Find("MainThreadQueue"));
    }

    void Update()
    {
        const int maxPerFrame = 5;
        int n = 0;
        while (n++ < maxPerFrame && _q.TryDequeue(out var a))
        {
            try { a(); } catch { /* swallow or log lightweight */ }
        }
    }
}