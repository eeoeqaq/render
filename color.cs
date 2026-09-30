namespace Renderer;

public static class color_utils
{
    public static double linear_to_gamma(double linear_component) =>
        linear_component > 0 ? Math.Sqrt(linear_component) : 0;

    public static void write_color(TextWriter output, color pixel_color)
    {
        var r = linear_to_gamma(pixel_color.x());
        var g = linear_to_gamma(pixel_color.y());
        var b = linear_to_gamma(pixel_color.z());

        var intensity = new interval(0.000, 0.999);
        int rbyte = (int)(256 * intensity.clamp(r));
        int gbyte = (int)(256 * intensity.clamp(g));
        int bbyte = (int)(256 * intensity.clamp(b));

        output.WriteLine($"{rbyte} {gbyte} {bbyte}");
    }
}
