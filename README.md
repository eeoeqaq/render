# C# Renderer

This branch ports the ray tracer to C# while keeping the original one-file-per-module layout and the same scene, camera, intersection, scattering, and PPM rendering flow.

Requires the .NET 8 SDK. Render the original 1200-pixel scene to a PPM file:

```sh
dotnet run -c Release --project Renderer.csproj > image.ppm
```

For a quick preview, override the width, sample count, and bounce depth:

```sh
dotnet run -c Release --project Renderer.csproj -- --width 160 --samples 4 --depth 10 > preview.ppm
```

Image data goes to standard output; scanline progress goes to standard error. The random generator uses a fixed seed so repeated runs produce the same C# image. Its sample sequence can differ from the C++ standard library.
