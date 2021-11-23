using UnityEngine;
using UnityEngine.Playables;
public class TimelineHelper
{
    public static TimelineUnit AddTimeline(GameObject go, string timelineName)
    {
        var unit = new TimelineUnit();
        var director = go.GetComponent<PlayableDirector>();
        if (null == director)
            director = go.AddComponent<PlayableDirector>();
        var asset = Resources.Load<PlayableAsset>("TimelineRes/" + timelineName);
        unit.Init(timelineName, director, asset);
        return unit;
    }
}