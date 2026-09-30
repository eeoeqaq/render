namespace Renderer;

internal static class Program
{
    private static void Main(string[] args)
    {
        var world = new hittable_list();

        var ground_material = new lambertian(new color(0.5, 0.5, 0.5));
        world.add(new sphere(new point3(0, -1000, 0), 1000, ground_material));

        for (int a = -11; a < 11; a++)
        {
            for (int b = -11; b < 11; b++)
            {
                var choose_mat = random_double();
                point3 center = new(a + 0.9 * random_double(), 0.2, b + 0.9 * random_double());

                if ((center - new point3(4, 0.2, 0)).length() > 0.9)
                {
                    material sphere_material;

                    if (choose_mat < 0.8)
                    {
                        var albedo = color.random() * color.random();
                        sphere_material = new lambertian(albedo);
                    }
                    else if (choose_mat < 0.95)
                    {
                        var albedo = color.random(0.5, 1);
                        var fuzz = random_double(0, 0.5);
                        sphere_material = new metal(albedo, fuzz);
                    }
                    else
                    {
                        sphere_material = new dielectric(1.5);
                    }

                    world.add(new sphere(center, 0.2, sphere_material));
                }
            }
        }

        var material1 = new dielectric(1.5);
        world.add(new sphere(new point3(0, 1, 0), 1.0, material1));

        var material2 = new lambertian(new color(0.4, 0.2, 0.1));
        world.add(new sphere(new point3(-4, 1, 0), 1.0, material2));

        var material3 = new metal(new color(0.7, 0.6, 0.5), 0.0);
        world.add(new sphere(new point3(4, 1, 0), 1.0, material3));

        var cam = new camera();
        cam.aspect_ratio = 16.0 / 9.0;
        cam.image_width = 1200;
        cam.samples_per_pixel = 50;
        cam.max_depth = 50;

        cam.vfov = 20;
        cam.lookfrom = new point3(13, 2, 3);
        cam.lookat = new point3(0, 0, 0);
        cam.vup = new vec3(0, 1, 0);

        cam.defocus_angle = 0.6;
        cam.focus_dist = 10.0;

        // Optional smaller settings make a quick render possible without changing the scene.
        for (int i = 0; i < args.Length; i += 2)
        {
            if (i + 1 >= args.Length || !int.TryParse(args[i + 1], out var value) || value <= 0)
                throw new ArgumentException("Use --width, --samples, or --depth followed by a positive integer.");

            switch (args[i])
            {
                case "--width": cam.image_width = value; break;
                case "--samples": cam.samples_per_pixel = value; break;
                case "--depth": cam.max_depth = value; break;
                default: throw new ArgumentException($"Unknown option: {args[i]}");
            }
        }

        cam.render(world);
    }
}
