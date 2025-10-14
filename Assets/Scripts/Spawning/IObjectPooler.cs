using UnityEngine;

namespace Spawning
{
    public interface IObjectPooler
    {
        public void InitializePool();

        public void ActivateObject(GameObject obj, Vector3 position);

        public void ReturnObject(GameObject obj);
    }
}