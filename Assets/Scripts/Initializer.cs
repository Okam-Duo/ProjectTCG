using UnityEngine;

public static class Initializer
{
    //씬이 로딩되기 전
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void BeforeSceneLoad()
    {
        
    }

    //씬과 씬의 객체들이 로딩된 후, Awake실행 전
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AfterSceneLoad() 
    {
        
    }
}
