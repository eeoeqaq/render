namespace Renderer;

public class material
{
    public virtual bool scatter(ray r_in, hit_record rec, out color attenuation, out ray scattered)
    {
        attenuation = default;
        scattered = default;
        return false;
    }
}

public sealed class lambertian : material
{
    private readonly color albedo;

    public lambertian(color albedo) => this.albedo = albedo;

    public override bool scatter(ray r_in, hit_record rec, out color attenuation, out ray scattered)
    {
        var scatter_direction = rec.normal + random_unit_vector();
        if (scatter_direction.near_zero()) scatter_direction = rec.normal;
        scattered = new ray(rec.p, scatter_direction);
        attenuation = albedo;
        return true;
    }
}

public sealed class metal : material
{
    private readonly color albedo;
    private readonly double fuzz;

    public metal(color albedo, double fuzz)
    {
        this.albedo = albedo;
        this.fuzz = fuzz < 1 ? fuzz : 1;
    }

    public override bool scatter(ray r_in, hit_record rec, out color attenuation, out ray scattered)
    {
        var reflected = reflect(r_in.direction(), rec.normal);
        reflected = unit_vector(reflected) + fuzz * random_unit_vector();
        scattered = new ray(rec.p, reflected);
        attenuation = albedo;
        return dot(scattered.direction(), rec.normal) > 0;
    }
}

public sealed class dielectric : material
{
    private readonly double refraction_index;

    public dielectric(double refraction_index) => this.refraction_index = refraction_index;

    public override bool scatter(ray r_in, hit_record rec, out color attenuation, out ray scattered)
    {
        attenuation = new color(1.0, 1.0, 1.0);
        var ri = rec.front_face ? 1.0 / refraction_index : refraction_index;

        var unit_direction = unit_vector(r_in.direction());
        var cos_theta = Math.Min(dot(-unit_direction, rec.normal), 1.0);
        var sin_theta = Math.Sqrt(1.0 - cos_theta * cos_theta);
        var cannot_refract = ri * sin_theta > 1.0;

        var direction = cannot_refract || reflectance(cos_theta, ri) > random_double()
            ? reflect(unit_direction, rec.normal)
            : refract(unit_direction, rec.normal, ri);

        scattered = new ray(rec.p, direction);
        return true;
    }

    private static double reflectance(double cosine, double refraction_index)
    {
        var r0 = (1 - refraction_index) / (1 + refraction_index);
        r0 *= r0;
        return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
    }
}
