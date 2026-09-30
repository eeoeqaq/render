global using color = Renderer.vec3;
global using point3 = Renderer.vec3;
global using static Renderer.rtweekend;
global using static Renderer.vec3_utils;
global using static Renderer.color_utils;

namespace Renderer;

public static class rtweekend
{
    public const double infinity = double.PositiveInfinity;
    public const double pi = 3.1415926535897932385;

    private static readonly Random generator = new(5489);

    public static double degrees_to_radians(double degrees) => degrees * pi / 180.0;

    public static double random_double() => generator.NextDouble();

    public static double random_double(double min, double max) => min + (max - min) * random_double();
}
