namespace Renderer;

public struct vec3
{
    private double e0, e1, e2;

    public vec3(double e0, double e1, double e2)
    {
        this.e0 = e0;
        this.e1 = e1;
        this.e2 = e2;
    }

    public readonly double x() => e0;
    public readonly double y() => e1;
    public readonly double z() => e2;

    public double this[int i]
    {
        readonly get => i switch { 0 => e0, 1 => e1, 2 => e2, _ => throw new IndexOutOfRangeException() };
        set
        {
            switch (i)
            {
                case 0: e0 = value; break;
                case 1: e1 = value; break;
                case 2: e2 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    public static vec3 operator -(vec3 v) => new(-v.e0, -v.e1, -v.e2);
    public static vec3 operator +(vec3 u, vec3 v) => new(u.e0 + v.e0, u.e1 + v.e1, u.e2 + v.e2);
    public static vec3 operator -(vec3 u, vec3 v) => new(u.e0 - v.e0, u.e1 - v.e1, u.e2 - v.e2);
    public static vec3 operator *(vec3 u, vec3 v) => new(u.e0 * v.e0, u.e1 * v.e1, u.e2 * v.e2);
    public static vec3 operator *(double t, vec3 v) => new(t * v.e0, t * v.e1, t * v.e2);
    public static vec3 operator *(vec3 v, double t) => t * v;
    public static vec3 operator /(vec3 v, double t) => (1 / t) * v;

    public readonly double length() => Math.Sqrt(length_squared());
    public readonly double length_squared() => e0 * e0 + e1 * e1 + e2 * e2;
    public readonly bool near_zero() => Math.Abs(e0) < 1e-8 && Math.Abs(e1) < 1e-8 && Math.Abs(e2) < 1e-8;

    public static vec3 random() => new(random_double(), random_double(), random_double());
    public static vec3 random(double min, double max) => new(random_double(min, max), random_double(min, max), random_double(min, max));
}

public static class vec3_utils
{
    public static double dot(vec3 u, vec3 v) => u.x() * v.x() + u.y() * v.y() + u.z() * v.z();

    public static vec3 cross(vec3 u, vec3 v) => new(
        u.y() * v.z() - u.z() * v.y(),
        u.z() * v.x() - u.x() * v.z(),
        u.x() * v.y() - u.y() * v.x());

    public static vec3 unit_vector(vec3 v) => v / v.length();

    public static vec3 random_in_unit_disk()
    {
        while (true)
        {
            var p = new vec3(random_double(-1, 1), random_double(-1, 1), 0);
            if (p.length_squared() < 1) return p;
        }
    }

    public static vec3 random_unit_vector()
    {
        while (true)
        {
            var p = vec3.random(-1, 1);
            var lensq = p.length_squared();
            if (1e-160 < lensq && lensq <= 1) return p / Math.Sqrt(lensq);
        }
    }

    public static vec3 random_on_hemisphere(vec3 normal)
    {
        var on_unit_sphere = random_unit_vector();
        return dot(on_unit_sphere, normal) > 0.0 ? on_unit_sphere : -on_unit_sphere;
    }

    public static vec3 reflect(vec3 v, vec3 n) => v - 2 * dot(v, n) * n;

    public static vec3 refract(vec3 uv, vec3 n, double etai_over_etat)
    {
        var cos_theta = Math.Min(dot(-uv, n), 1.0);
        var r_out_perp = etai_over_etat * (uv + cos_theta * n);
        var r_out_parallel = -Math.Sqrt(Math.Abs(1.0 - r_out_perp.length_squared())) * n;
        return r_out_perp + r_out_parallel;
    }
}
