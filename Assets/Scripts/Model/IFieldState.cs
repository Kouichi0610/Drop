
namespace Model {
    public interface IFieldState {
        int Width { get; }
        int Height { get; }

        bool HasGem(int x, int y);
        int PiledHeight(int x);
    }
}
