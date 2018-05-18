using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class IDContainer<T> {
    [SerializeField]
    List<T> items = new List<T>();
    [SerializeField]
    List<int> ids = new List<int>();

    public T this[int id] {
        get {
            return items[id];
        }
        set {
            items[id] = value;
        }
    }

    public void Remove(int id) {
        ids.Add(id);
        ids.Sort();
    }

    public void Remove(T item) {
        int index = items.FindIndex((i) => i.Equals(item));
        Remove(index);
    }

    public int Add(T item) {
        if (ids.Count > 0) {
            int id = ids[0];
            items[id] = item;
            ids.RemoveAt(0);
            return id;
        }
        else {
            items.Add(item);
            return items.Count;
        }
    }

    public IEnumerator<T> GetEnumerator() {
        for(int i = 0; i < items.Count; i++) {
            if (!ids.Contains(i)) {
                yield return items[i];
            }
        }
    }

    public IEnumerable<int> GetIDs() {
        for (int i = 0; i < items.Count; i++) {
            if (!ids.Contains(i)) {
                yield return i;
            }
        }
    }

    public int Count {
        get {
            return items.Count - ids.Count;
        }
    }

    public int FindIndex(Predicate<T> predicat) {
        return items.FindIndex(predicat);
    }

    public List<T> FindAll(Predicate<T> predicat) {
        List<T> r = new List<T>();
        foreach(var s in this) {
            if (predicat(s)) {
                r.Add(s);
            }
        }
        return r;
    }

    public List<int> FindAllIndex(Predicate<T> predicat) {
        List<int> r = new List<int>();
        for(int i = 0; i < items.Count; i++) {
            if (!ids.Contains(i) && predicat(items[i])) {
                r.Add(i);
            }
        }
        return r;
    }

    public bool Contains(int id) {
        return id >= 0 && id < items.Count && !ids.Contains(id);
    }
}
