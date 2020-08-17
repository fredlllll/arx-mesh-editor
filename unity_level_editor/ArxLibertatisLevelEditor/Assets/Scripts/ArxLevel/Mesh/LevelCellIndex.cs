using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ArxLevel.Mesh
{
    public struct LevelCellIndex
    {
        public readonly int x, z, index;

        public LevelCellIndex(int x, int z, int index)
        {
            this.x = x;
            this.z = z;
            this.index = index;
        }

        public static IEnumerable<LevelCellIndex> GetCellIndices(int sizeX, int sizeZ)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    int index = x * sizeZ + z;
                    yield return new LevelCellIndex(x, z, index);
                }
            }
        }
    }
}
