namespace Renderer;

public struct interval
{
    public double min, max;

    public interval() : this(+infinity, -infinity) { }
    public interval(double min, double max)
    {
        this.min = min;
        this.max = max;
    }

    public readonly double size() => max - min;
    public readonly bool contains(double x) => min <= x && x <= max;
    public readonly bool surrounds(double x) => min < x && x < max;
    public readonly double clamp(double x) => x < min ? min : x > max ? max : x;

    public static readonly interval empty = new(+infinity, -infinity);
    public static readonly interval universe = new(-infinity, +infinity);
}
