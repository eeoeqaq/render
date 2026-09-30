namespace Renderer;

public sealed class camera
{
    public double aspect_ratio = 1.0;
    public int image_width = 100;
    public int samples_per_pixel = 10;
    public int max_depth = 10;

    public double vfov = 90;
    public point3 lookfrom = new(0, 0, 0);
    public point3 lookat = new(0, 0, -1);
    public vec3 vup = new(0, 1, 0);

    public double defocus_angle = 0;
    public double focus_dist = 10;

    private int image_height;
    private double pixel_samples_scale;
    private point3 center;
    private point3 pixel00_loc;
    private vec3 pixel_delta_u;
    private vec3 pixel_delta_v;
    private vec3 u, v, w;
    private vec3 defocus_disk_u;
    private vec3 defocus_disk_v;

    public void render(hittable world) => render(world, Console.Out, Console.Error);

    public void render(hittable world, TextWriter output, TextWriter progress)
    {
        initialize();

        output.Write($"P3\n{image_width} {image_height}\n255\n");

        for (int j = 0; j < image_height; j++)
        {
            progress.Write($"\rScanlines remaining: {image_height - j} ");
            progress.Flush();
            for (int i = 0; i < image_width; i++)
            {
                color pixel_color = new(0, 0, 0);
                for (int sample = 0; sample < samples_per_pixel; sample++)
                {
                    var r = get_ray(i, j);
                    pixel_color += ray_color(r, max_depth, world);
                }
                write_color(output, pixel_samples_scale * pixel_color);
            }
        }

        progress.Write("\rDone.                 \n");
    }

    private void initialize()
    {
        image_height = (int)(image_width / aspect_ratio);
        image_height = image_height < 1 ? 1 : image_height;

        pixel_samples_scale = 1.0 / samples_per_pixel;
        center = lookfrom;

        var theta = degrees_to_radians(vfov);
        var h = Math.Tan(theta / 2);
        var viewport_height = 2 * h * focus_dist;
        var viewport_width = viewport_height * ((double)image_width / image_height);

        w = unit_vector(lookfrom - lookat);
        u = unit_vector(cross(vup, w));
        v = cross(w, u);

        var viewport_u = viewport_width * u;
        var viewport_v = viewport_height * -v;

        pixel_delta_u = viewport_u / image_width;
        pixel_delta_v = viewport_v / image_height;

        var viewport_upper_left = center - focus_dist * w - viewport_u / 2 - viewport_v / 2;
        pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);

        var defocus_radius = focus_dist * Math.Tan(degrees_to_radians(defocus_angle / 2));
        defocus_disk_u = u * defocus_radius;
        defocus_disk_v = v * defocus_radius;
    }

    private ray get_ray(int i, int j)
    {
        var offset = sample_square();
        var pixel_sample = pixel00_loc
                         + (i + offset.x()) * pixel_delta_u
                         + (j + offset.y()) * pixel_delta_v;

        var ray_origin = defocus_angle <= 0 ? center : defocus_disk_sample();
        var ray_direction = pixel_sample - ray_origin;
        return new ray(ray_origin, ray_direction);
    }

    private vec3 sample_square() => new(random_double() - 0.5, random_double() - 0.5, 0);

    private point3 defocus_disk_sample()
    {
        var p = random_in_unit_disk();
        return center + p[0] * defocus_disk_u + p[1] * defocus_disk_v;
    }

    private color ray_color(ray r, int depth, hittable world)
    {
        if (depth <= 0) return new color(0, 0, 0);

        hit_record rec = new();
        if (world.hit(r, new interval(0.001, infinity), ref rec))
        {
            if (rec.mat!.scatter(r, rec, out var attenuation, out var scattered))
                return attenuation * ray_color(scattered, depth - 1, world);
            return new color(0, 0, 0);
        }

        var unit_direction = unit_vector(r.direction());
        var a = 0.5 * (unit_direction.y() + 1.0);
        return (1.0 - a) * new color(1.0, 1.0, 1.0) + a * new color(0.5, 0.7, 1.0);
    }
}
