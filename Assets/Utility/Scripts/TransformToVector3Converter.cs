using UnityEngine;

namespace Utility
{
    public class TransformToVector3Converter : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;
        [SerializeField] private Vector3[] _vector3s;

        public void Convert()
        {
            var v3s = new Vector3[_transforms.Length];

            for (int i = 0; i < _transforms.Length; i++)
            {
                v3s[i] = _transforms[i].position;
            }

            _vector3s = v3s;
        }
    }
}