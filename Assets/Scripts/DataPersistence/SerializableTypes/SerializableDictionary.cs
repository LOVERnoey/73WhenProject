using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    
    public void OnBeforeSerialize()
    {
       keys.Clear();
       values.Clear();
       foreach (KeyValuePair<TKey, TValue> pair in this)
       {
           keys.Add(pair.Key);
           values.Add(pair.Value);
       }
    } 
    public void OnAfterDeserialize()
    {
        this.Clear();
        
        if (keys.Count != values.Count)
        {
            Debug.LogError("Keys and values count mismatch after deserialization.");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
