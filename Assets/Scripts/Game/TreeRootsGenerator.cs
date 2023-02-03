using System.Collections.Generic;
using UnityEngine;

namespace MudOverload.Game
{
    public class TreeRootsGenerator : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer template;
        [SerializeField]
        private float rootRandomSize = 1;

        private void Awake()
        {
            for (var i = 0; i < 3; i++)
            {
                var newLine = Instantiate(template, transform);

                var positions = new List<Vector3>();
                positions.Add(new Vector3());

                for(var a = 0; a < 7; a++)
                {
                    positions.Add(new Vector3(Random.Range(-rootRandomSize, rootRandomSize), -a - 1, 0));
                }

                newLine.positionCount = positions.Count;
                newLine.SetPositions(positions.ToArray());
            }
        }
    }
}
