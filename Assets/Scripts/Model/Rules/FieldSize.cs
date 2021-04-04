namespace Model.Rules {
    public sealed class FieldSize {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public FieldSize(int w, int h) {
            Width = w;
            Height = h;
        }
    }
}
