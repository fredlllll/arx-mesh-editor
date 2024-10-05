using System;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    public class AutoDictionary<TKey, TVal> : Dictionary<TKey, TVal>
    {
        Func<TKey, TVal> generator;

        public AutoDictionary(Func<TKey, TVal> generator)
        {
            this.generator = generator;
        }

        public new TVal this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out TVal val))
                {
                    val = generator(key);
                    base[key] = val;
                }
                return val;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
