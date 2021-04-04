namespace Model.Rules {
    public sealed class EraseCount {
        public int Count { get; private set; }

        public EraseCount(int c) {
            Count = c;
        }
    }
}
