namespace Renderer;

public readonly struct ray
{
    private readonly point3 orig;
    private readonly vec3 dir;

    public ray(point3 origin, vec3 direction)
    {
        orig = origin;
        dir = direction;
    }

    public point3 origin() => orig;
    public vec3 direction() => dir;
    public point3 at(double t) => orig + t * dir;
}
