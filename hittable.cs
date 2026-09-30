namespace Renderer;

public struct hit_record
{
    public point3 p;
    public vec3 normal;
    public material? mat;
    public double t;
    public bool front_face;

    public void set_face_normal(ray r, vec3 outward_normal)
    {
        front_face = dot(r.direction(), outward_normal) < 0;
        normal = front_face ? outward_normal : -outward_normal;
    }
}

public abstract class hittable
{
    public abstract bool hit(ray r, interval ray_t, ref hit_record rec);
}
