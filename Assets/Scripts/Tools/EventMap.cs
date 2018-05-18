using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class EventMap<T, U> : ISerializationCallbackReceiver, IDictionary<T, U> where T : IConvertible {
    Dictionary<T, U> map = new Dictionary<T, U>();
    [SerializeField]
    List<T> left = new List<T>();
    [SerializeField]
    List<U> right = new List<U>();

    public ICollection<T> Keys {
        get {
            return map.Keys;
        }
    }

    public ICollection<U> Values {
        get {
            return map.Values;
        }
    }

    public int Count {
        get {
            return map.Count;
        }
    }

    public bool IsReadOnly {
        get {
            return false;
        }
    }

    public U this[T key] {
        get {
            return map[key];
        }

        set {
            map[key] = value;
        }
    }

    public EventMap(){
        OnBeforeSerialize();
        OnAfterDeserialize();
    }

    public void OnAfterDeserialize() {
        for(int i = 0; i < left.Count; i++) {
            Add(left[i], right[i]);
        }
    }

    public void OnBeforeSerialize() {
        foreach(var kvp in map) {
            left.Add(kvp.Key);
            right.Add(kvp.Value);
        }
    }

    public void Add(T key, U value) {
        map.Add(key, value);
    }

    public bool ContainsKey(T key) {
        return map.ContainsKey(key);
    }

    public bool Remove(T key) {
        return map.Remove(key);
    }

    public bool TryGetValue(T key, out U value) {
        return map.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<T, U> item) {
        map.Add(item.Key, item.Value);
    }

    public void Clear() {
        map.Clear();
    }

    public bool Contains(KeyValuePair<T, U> item) {
        return map.Contains(item);
    }

    public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex) {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<T, U> item) {
        return map.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<T, U>> GetEnumerator() {
        return map.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return map.GetEnumerator();
    }
}