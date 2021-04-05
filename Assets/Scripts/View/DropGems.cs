using System.Collections.Generic;
using System.Linq;

namespace View {
    public sealed class DropGems {
        public IEnumerable<DropGem> List { get{ return gems; } }
        List<DropGem> gems = new List<DropGem>();

        public DropGems(IEnumerable<DropGem> gems) {
            this.gems = gems.ToList();
        }
    }
    public sealed class DropGem {
        public Model.Gems GemType { get; private set; }
        public int Line { get; private set; }

        public DropGem(Model.Gems gemType, int line) {
            GemType = gemType;
            Line = line;
        }
    }
}
