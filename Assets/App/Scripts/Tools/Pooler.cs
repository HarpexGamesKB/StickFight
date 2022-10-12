using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Pooler
    {
        private Component _poolObject;
        private List<Component> _objects = new List<Component>();

        private Transform _parent;

        public Pooler(Component poolObject, Transform parent = null)
        {
            _poolObject = poolObject;
            _parent = parent;
        }

        public Component GetObject(bool enable = true)
        {
            ClearNull();
            foreach (Component Object in _objects)
            {
                if (!Object.gameObject.activeSelf)
                {
                    if (enable)
                    {
                        Object.gameObject.SetActive(true);
                    }

                    return Object;
                }
            }
            Component newPoolObject = Object.Instantiate(_poolObject, _parent);

            if (enable)
            {
                newPoolObject.gameObject.SetActive(true);
            }

            _objects.Add(newPoolObject);

            return newPoolObject;
        }

        public void DisableAll()
        {
            ClearNull();
            foreach (Component Object in _objects)
            {
                if (Object.gameObject.activeSelf)
                {
                    Object.gameObject.SetActive(false);
                }
            }
        }

        public void Clear()
        {
            ClearNull();
            foreach (Component Object in _objects)
            {
                GameObject.Destroy(Object.gameObject);
            }

            _objects.Clear();
        }

        public void CreatePool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Component newPoolObject = Object.Instantiate(_poolObject, _parent);
                newPoolObject.gameObject.SetActive(false);
                _objects.Add(newPoolObject);
            }
        }

        public void ClearInactive()
        {
            ClearNull();
            foreach (Component Object in _objects)
            {
                if (!Object.gameObject.activeSelf)
                {
                    GameObject.Destroy(Object.gameObject);
                }
            }
        }

        private void ClearNull()
        {
            _objects.RemoveAll(item => item == null);
        }
    }
}