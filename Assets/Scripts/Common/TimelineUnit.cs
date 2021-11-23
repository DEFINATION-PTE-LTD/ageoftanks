using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class TimelineUnit
{
    public string name;
    public PlayableDirector director;
    public PlayableAsset asset;
    public Dictionary<string, PlayableBinding> bindings;
    public Dictionary<string, Dictionary<string, PlayableAsset>> clips;
    public void Init(string name, PlayableDirector director, PlayableAsset asset)
    {
        director.playableAsset = asset;
        this.name = name;
        this.director = director;
        this.asset = asset;
        bindings = new Dictionary<string, PlayableBinding>();
        clips = new Dictionary<string, Dictionary<string, PlayableAsset>>();
        foreach (var o in asset.outputs)
        {
            var trackName = o.streamName;
            bindings.Add(trackName, o);
            var track = o.sourceObject as TrackAsset;
            var clipList = track.GetClips();
            foreach (var c in clipList)
            {
                if (!clips.ContainsKey(trackName))
                {
                    clips[trackName] = new Dictionary<string, PlayableAsset>();
                }
                var name2Clips = clips[trackName];
                if (!name2Clips.ContainsKey(c.displayName))
                {
                    name2Clips.Add(c.displayName, c.asset as PlayableAsset);
                }
            }
        }
    }
    public void SetBinding(string trackName, Object o)
    {
        director.SetGenericBinding(bindings[trackName].sourceObject, o);
    }
    public T GetTrack<T>(string trackName) where T : TrackAsset
    {
        return bindings[trackName].sourceObject as T;
    }
    public T GetClip<T>(string trackName, string clipName) where T : PlayableAsset
    {
        if (clips.ContainsKey(trackName))
        {
            var track = clips[trackName];
            if (track.ContainsKey(clipName))
            {
                return track[clipName] as T;
            }
            else
            {
                Debug.LogError("GetClip Error,Track does not contain clip,trackName: " + trackName + ",clipName: " + clipName);
            }
        }
        else
        {
            Debug.LogError("GetClip Error,clipName: " + clipName);
        }
        return null;
    }
    public void Play()
    {
        director.Play();
    }
}