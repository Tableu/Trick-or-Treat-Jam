
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IPortraitCollection
{
    public Sprite GetPortrait(string id);
}

public abstract class PortraitCollectionScriptableObject : ScriptableObject
{
    public abstract IPortraitCollection MakePortraitCollection();
}

[CreateAssetMenu(fileName = "PortraitCollection", menuName = "Portraits/PortraitCollection", order = 0)]
public class PortraitCollection : PortraitCollectionScriptableObject
{
    public List<string> keys;
    public List<Sprite> sprites;
    public Sprite defaultPortrait;
    public override IPortraitCollection MakePortraitCollection()
    {
        return new Collection(keys, sprites, defaultPortrait);
    }

    private class Collection : IPortraitCollection
    {
        private readonly Dictionary<string,Sprite> _portraits;
        private readonly Sprite _defaultPortrait;
        public Collection(List<string> keys, List<Sprite> sprites, Sprite defaultPortrait)
        {
            _portraits = keys.Zip(sprites, ((k, v) => new {Key = k, Value = v}))
                .ToDictionary(x => x.Key, x=> x.Value);
            _defaultPortrait = defaultPortrait;
        }

        public Sprite GetPortrait(string key)
        {
            if (_portraits.TryGetValue(key, out Sprite sprite))
            {
                return sprite;
            }

            return _defaultPortrait;
        }
    }
}