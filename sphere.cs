namespace Renderer;

public sealed class sphere : hittable
{
    private readonly point3 center;
    private readonly double radius;
    private readonly material mat;

    public sphere(point3 center, double radius, material mat)
    {
        this.center = center;
        this.radius = Math.Max(0, radius);
        this.mat = mat;
    }

    public override bool hit(ray r, interval ray_t, ref hit_record rec)
    {
        var oc = center - r.origin();
        var a = r.direction().length_squared();
        var h = dot(r.direction(), oc);
        var c = oc.length_squared() - radius * radius;

        var discriminant = h * h - a * c;
        if (discriminant < 0) return false;

        var sqrtd = Math.Sqrt(discriminant);
        var root = (h - sqrtd) / a;
        if (!ray_t.surrounds(root))
        {
            root = (h + sqrtd) / a;
            if (!ray_t.surrounds(root)) return false;
        }

        rec.t = root;
        rec.p = r.at(rec.t);
        var outward_normal = (rec.p - center) / radius;
        rec.set_face_normal(r, outward_normal);
        rec.mat = mat;
        return true;
    }
}
