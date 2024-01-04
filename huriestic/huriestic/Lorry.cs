using System;
namespace huriestic
{
    public class Lorry
    {
        public List<BrickGroup> BrickGroups { get; private set; }
        public Double TotalWeight => BrickGroups.Sum(bg => bg.Weight);

        public Lorry()
        {
            BrickGroups = new List<BrickGroup>();
        }

        public void AddBrickGroup(BrickGroup brickGroup)
        {

            BrickGroups.Add(brickGroup);
        }

        public void RemoveBrickGroup(BrickGroup brickGroup)
        {
            BrickGroups.Remove(brickGroup);
        }
    }
}

