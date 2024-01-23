using STU;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable {
    void OnSpawned();
    void OnRecycled();
}

public class PoolManager : Singleton<PoolManager> {

    private class Pool {

        private GameObject prefab;
        private Transform parent;
        private List<GameObject> spawns;
        private Queue<GameObject> recycles;

        public GameObject Prefab => prefab;
        public int SpawnedCount => spawns.Count;
        public int RecycledCount => recycles.Count;
        public int ToltalCount => SpawnedCount + RecycledCount;

        public IEnumerable<GameObject> GetAllSpawned() {
            foreach(GameObject pooling in spawns) {
                yield return pooling;
            }
        }

        public IEnumerable<GameObject> GetAllRecycled() {
            foreach(GameObject pooling in recycles) {
                yield return pooling;
            }
        }

        public IEnumerable<GameObject> GetAllPooling() {
            foreach(GameObject pooling in spawns) {
                yield return pooling;
            }
            foreach(GameObject pooling in recycles) {
                yield return pooling;
            }
        }

        public Pool(GameObject prefab, int capacity, Transform parent) {
            this.prefab = prefab;
            this.parent = parent;

            recycles = new Queue<GameObject>(capacity);
            spawns = new List<GameObject>(capacity);

            for(int i = 0; i < capacity; i++) {
                GameObject instance = Object.Instantiate(prefab);
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(parent);
                recycles.Enqueue(instance);
            }
        }

        public GameObject Spawn() {
            GameObject instance;
            if(recycles.Count > 0) {
                instance = recycles.Dequeue();
                if(instance == null) {
                    return Spawn();
                }
            } else {
                instance = Object.Instantiate(Prefab);
            }

            instance.gameObject.SetActive(true);
            spawns.Add(instance);

            foreach(IPoolable poolable in instance.GetComponents<IPoolable>()) {
                poolable.OnSpawned();
            }

            return instance;
        }

        public void Recycle(GameObject target) {
            target.gameObject.SetActive(false);
            target.transform.SetParent(parent);

            if(spawns.Remove(target)) {
                recycles.Enqueue(target);

                foreach(IPoolable poolable in target.GetComponents<IPoolable>()) {
                    poolable.OnRecycled();
                }
            }
        }

        public void Destroy() {
            for(int i = 0; i < spawns.Count; i++) {
                Object.Destroy(spawns[i].gameObject);
            }
            while(recycles.Count > 0) {
                Object.Destroy(recycles.Dequeue().gameObject);
            }

            spawns.Clear();
            recycles.Clear();
        }
    }

    private readonly Dictionary<int, Pool> pools = new Dictionary<int, Pool>();
    private readonly Dictionary<int, int> prefabs = new Dictionary<int, int>();

    public static void CreatePool<T>(T target, int capacity) where T : Component {
        CreatePool(target.gameObject, capacity);
    }

    public static void CreatePool(GameObject prefab, int capacity) {
        if(Instance == null)
            return;

        if(IsPooled(prefab))
            return;

        Instance.pools.Add(prefab.GetInstanceID(), new Pool(prefab, capacity, Instance.transform));
    }

    public static void DestroyPool<T>(T prefab) where T : Component {
        DestroyPool(prefab.gameObject);
    }

    public static void DestroyPool(GameObject prefab) {
        if(Instance == null)
            return;

        Pool pool;
        if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
            pool.Destroy();
            Instance.pools.Remove(prefab.GetInstanceID());
        }
    }

    public static bool IsPooled<T>(T prefab) where T : Component {
        return IsPooled(prefab.gameObject);
    }

    public static bool IsPooled(GameObject prefab) {
        if(Instance == null)
            return false;

        return Instance.pools.ContainsKey(prefab.GetInstanceID());
    }

    public static bool IsPooling<T>(T target) where T : Component {
        return IsPooling(target.gameObject);
    }

    public static bool IsPooling(GameObject target) {
        if(Instance == null)
            return false;

        return Instance.prefabs.ContainsKey(target.GetInstanceID());
    }

    public static GameObject GetPrefab(GameObject target) {
        if(Instance == null)
            return null;

        int prefabId;
        if(Instance.prefabs.TryGetValue(target.GetInstanceID(), out prefabId)) {
            Pool pool;
            if(Instance.pools.TryGetValue(prefabId, out pool)) {
                return pool.Prefab;
            }
        }
        return null;
    }

    public static int GetSpawnedCount<T>(T prefab) where T : Component {
        return GetSpawnedCount(prefab.gameObject);
    }

    public static int GetSpawnedCount(GameObject prefab) {
        if(Instance == null)
            return 0;

        Pool pool;
        if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
            return pool.SpawnedCount;
        }

        return 0;
    }

    public static int GetRecycledCount<T>(T prefab) where T : Component {
        return GetRecycledCount(prefab.gameObject);
    }

    public static int GetRecycledCount(GameObject prefab) {
        if(Instance == null)
            return 0;

        Pool pool;
        if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
            return pool.RecycledCount;
        }

        return 0;
    }

    public static int GetTotalCount<T>(T prefab) where T : Component {
        return GetTotalCount(prefab.gameObject);
    }

    public static int GetTotalCount(GameObject prefab) {
        if(Instance == null)
            return 0;

        Pool pool;
        if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
            return pool.ToltalCount;
        }

        return 0;
    }

    public IEnumerable<GameObject> GetAllSpawned(GameObject prefab) {
        if(Instance != null) {
            Pool pool;
            if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
                foreach(GameObject release in pool.GetAllSpawned()) {
                    yield return release;
                }
            }
        }
    }

    public IEnumerable<GameObject> GetAllRecycled(GameObject prefab) {
        if(Instance != null) {
            Pool pool;
            if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
                foreach(GameObject collect in pool.GetAllRecycled()) {
                    yield return collect;
                }
            }
        }
    }

    public IEnumerable<GameObject> GetAllPooling(GameObject prefab) {
        if(Instance != null) {
            Pool pool;
            if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
                foreach(GameObject pooling in pool.GetAllPooling()) {
                    yield return pooling;
                }
            }
        }
    }

    public static T Spawn<T>(T prefab, bool autoPool = true) where T : Component {
        return Spawned(prefab.gameObject, autoPool)?.GetComponent<T>();
    }

    public static GameObject Spawned(GameObject prefab, bool autoPool = true) {
        if(Instance == null)
            return null;

        if(autoPool && !IsPooled(prefab)) {
            CreatePool(prefab, 4);
        }

        Pool pool;
        if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
            GameObject instance = pool.Spawn();
            Instance.prefabs[instance.GetInstanceID()] = prefab.GetInstanceID();
            return instance;
        }

        return Object.Instantiate(prefab);
    }

    public static void Recycle<T>(T target) where T : Component {
        Recycle(target.gameObject);
    }

    public static void Recycle(GameObject target) {
        if(target == null) {
            return;
        }

        if(Instance == null) {
            Destroy(target);
            return;
        }

        int targetId = target.GetInstanceID();
        int prefabId;
        if(Instance.prefabs.TryGetValue(targetId, out prefabId)) {
            Pool pool;
            if(Instance.pools.TryGetValue(prefabId, out pool)) {
                pool.Recycle(target);
                Instance.prefabs.Remove(targetId);
            } else {
                Destroy(target);
            }
        } else {
            Destroy(target);
        }
    }

    public static void Destroy<T>(T prefab) where T : Component {
        Destroy(prefab.gameObject);
    }

    public static void Destroy(GameObject prefab) {
        if(Instance == null)
            return;

        Pool pool;
        if(Instance.pools.TryGetValue(prefab.GetInstanceID(), out pool)) {
            foreach(GameObject instance in pool.GetAllPooling()) {
                Instance.prefabs.Remove(instance.GetInstanceID());
            }
            pool.Destroy();
        }
    }

}

public static class PoolExtension {
    public static T Spawn<T>(this T prefab, bool autoPool = true) where T : Component {
        return PoolManager.Spawn(prefab, autoPool);
    }

    public static T Spawn<T>(this T prefab, Transform parent ,bool autoPool = true) where T : Component {
        var result = PoolManager.Spawn(prefab, autoPool);
        result.transform.parent = parent;
        return result;
    }

    public static GameObject Spawn(this GameObject prefab, bool autoPool = true) {
        return PoolManager.Spawned(prefab, autoPool);
    }
    
    
    public static GameObject Spawn(this GameObject prefab, Transform parent , bool autoPool = true) {
        var result = PoolManager.Spawned(prefab, autoPool);
        result.transform.parent = parent;
        return result;
    }


    public static void Recycle<T>(this T target) where T : Component {
        PoolManager.Recycle(target);
    }

    public static void Recycle(this GameObject target) {
        PoolManager.Recycle(target);
    }
}
