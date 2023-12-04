# Animated Graph

This project is a follow-along of
[Jasper Flick's Catlike Coding](https://catlikecoding.com/) Unity
[basics](https://catlikecoding.com/unity/tutorials/basics/) tutorials. This
repository covers the projects from Building a Graph all the way up to Organic
Variety.

## Part 1 - [Building a Graph](https://catlikecoding.com/unity/tutorials/basics/building-a-graph/)

![img](img/sineWave.png)

The first part of the project entails creating a graph of a sinusoidal wave that
animates over time, instantiating points through a C# script and moving them on
an update loop. I've also added graphed axes as well. The project uses Universal
Render Pipeline (URP) for speed-ups as well.

## Part 2 - [Mathematical Surfaces](https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/)

This section starts off with implementing other waves:

| Wave                       | Notes                                                                                                           |
| -------------------------- | --------------------------------------------------------------------------------------------------------------- |
| ![img](img/multiWave.png)  | This wave is an addition of two sine waves.                                                                     |
| ![img](img/rippleWave.png) | A more complicated addition of sinusoidal waves.                                                                |
| ![img](img/spiderWave.png) | I created this interesting 'spider' periodic function on a whim. It devolves into aspiraling 3D effect quickly. |

Speaking of 3D...

| Wave                         | Notes                                                                 |
| ---------------------------- | --------------------------------------------------------------------- |
| ![img](img/going3D.png)      | The project's core functionality was easily extended to 3 dimensions. |
| ![img](img/diagonalWave.gif) | Of course, the third dimension can be wrapped into the structure...   |
| ![img](img/multiWaveXYZ.gif) | Some interesting physical wave-like behavior can be seen here.        |
| ![img](img/rippleXYZ.gif)    | I love how this one 'drops' in a physical sense.                      |
| ![img](img/crab.gif)         | Extending the 'spider' to 3 dimensions returned to crab.              |

The next portion moved on from the XZ plane into more interesting meshes and
surfaces with the UV coordinate space. This also allowed for a refactor of the
code in order to change resolution on the fly:

![img](img/vectorUVResolution.gif)

Finally, this part of the project ended with creating spheres and perturbing
their surfaces to make more complicated 3D objects. The following are a series
of the spherical implementations:

| Object                         | Notes                                                                 |
| ------------------------------ | --------------------------------------------------------------------- |
| ![img](img/sphere.png)         | A basic sphere surface.                                               |
| ![img](img/bouncyBall.gif)     | Adding some time variance to the sphere...                            |
| ![img](img/vertBands.png)      | The sphere's surface can also have bands.                             |
| ![img](img/horizBands.png)     | The bands can go in different directions...                           |
| ![img](img/twistingSphere.gif) | Combining this vertical and horizontal nature yields a pretty visual. |

Toroidal shapes were also added:

| Object                         | Notes                                                                                          |
| ------------------------------ | ---------------------------------------------------------------------------------------------- |
| ![img](img/spindle.png)        | A basic self-intersecting spindle torus was step 1.                                            |
| ![img](img/ringTorus.png)      | Pulling the inner radius apart reveals a proper ring toroidal shape.                           |
| ![img](img/torusLowRes.png)    | An artifact of the maths means decreasing the resolution results in these skeletal rings.      |
| ![img](img/spiralStar.gif)     | Applying some funky time variance to the radii creates this beautiful optical illusion.        |
| ![img](img/illusionRuined.gif) | Decreasing the resolution shows the star isn't spinning - It's just concentric stars rotating. |

At this point I went off on a bit of a tangent implementing some other
parametric surfaces.

| Surface                       | Notes                                                                                                                                                                                                                   |
| ----------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ![img](img/lineHelix.gif)     | This effect came about when looking for parametrized helixes.                                                                                                                                                           |
| ![img](img/sinRS.gif)         | Wikipedia had a listing of Surfaces of Revolution, where any univariate function could be passed in to create an interesting effect. I tried it with sinusoids and got this bifurcated slinky pattern.                  |
| ![img](img/cosRS.gif)         | Interestingly, trying this with cosine instead of sine combined the bifurcated pattern into concentric circles. Odd!                                                                                                    |
| ![img](img/tanRS.gif)         | I can't say I _didn't_ expect something strange here, but I definitely wasn't let down. This is with the tangent function.                                                                                              |
| ![img](img/secRS.gif)         | Trying again with the secant function bifurcated the entire surface. Neat!                                                                                                                                              |
| ![img](img/tanStar.gif)       | I wanted to try applying tangents where they shouldn't be (by modifying the spiral star example). Needless to say... Nailed it.                                                                                         |
| ![img](img/Enneper.gif)       | Moving back to actual surfaces, this is the [Enneper surface](https://en.wikipedia.org/wiki/Enneper_surface). I found it through a rabbit hole of Weierstrass functions when I was playing with Surfaces of Revolution. |
| ![img](img/duplinCyclide.gif) | When trying to implement a Duplin Cyclide (Sort of a croissant shape?), I happened upon the hyperbolic variety. It's still pretty, so I figured I'd leave it in.                                                        |

## Part 3 - [Measuring Performance](https://catlikecoding.com/unity/tutorials/basics/measuring-performance/)

This section contains a series of interesting statistics. I think my
configuration of URP was inaccurate, however, as modifying particular parameters
and setting up debuggers didn't work properly.

However, the visual change in this section is the ability to morph the graph's
points to other functions automagically. It's a fairly beautiful experience, so
here's a long GIF:

![img](img/morphograph.gif)

## Part 4 - [Compute Shaders](https://catlikecoding.com/unity/tutorials/basics/compute-shaders/)

With a resolution value of 100 (as in the previous images), the program was able
to run at 100 FPS on my machine. This represents 10,000 points rendered, for a
total of 20,000 batches on URP (once for each point drawn, and once for casting
a shadow onto any other points). Increasing to 200 increases the batches to
80,000 (40,000 points and 40,000 shadows), and decreases my machine's
performance to 30 FPS.

Increasing to 500 resolution drops my FPS further down to 5 FPS, which is
practically unusable, only for a measly 250,000 points. However, I would prefer
to run millions of points simultaneously, if possible.

Currently, all points are sorted, batched, and calculated through C# scripts
running on the CPU. Offloading this work onto the GPU would significantly speed
up the task.

When swapping over materials, I realized that URP (while saving performance) is
extremely finicky to set up. However, I was successfully able to port functions
over to the GPU. As such, we can view them in much higher resolution:

| GIF                     | Notes                                                                                                                                                                                                            |
| ----------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ![img](img/gpu10.gif)   | Running at a resolution of 10, or 100 cubes. This runs at ~1000 FPS on my machine's GPU.                                                                                                                         |
| ![img](img/gpu100.gif)  | Running at a resolution of 100, or 10,000 cubes, this runs at ~950 FPS on my machine's GPU. Clearly, the CPU is the bottleneck at high framerates.                                                               |
| ![img](img/gpu1000.gif) | Running at a resolution of 1000, or 1 million cubes, this runs at ~400 FPS on my machine's GPU. Beefy! The visuals are hampered by the small point size, though, giving the image an unfortunately 'dusty' look. |

Possibly the most annoying part of this implementation was working with all of
the `#pragma` directives and handling them. Since they had to be manually made,
and I had 18 unique functions, I had to manually declare all 324 functions (18
unique functions, and the remainder 'morphing' kernels from one wave to
another). Offloading the files into another compute shader file didn't seem to
work either, so I'm stuck having long (1000+ LoC) compute shader files if making
interesting tricks like these. In the future, I'll need to set up Python scripts
to manually generate this code.

## Part 5 - [Jobs](https://catlikecoding.com/unity/tutorials/basics/jobs/)

The first portion of this step was creating fractals. The fractal effectively
formed the Sierpinski triangle, but due to its recursive hierarchical nature, it
had to be calculated on the CPU. The following are some of my performance tests:

| Image                    | FPS (Approx.) | Object Count | Notes                                                    |
| ------------------------ | ------------- | ------------ | -------------------------------------------------------- |
| ![img](img/fractal1.png) | 950           | 1            | Just a sphere.                                           |
| ![img](img/fractal2.png) | 950           | 6            | Adding faces on each side...                             |
| ![img](img/fractal3.png) | 930           | 31           | A bit more complicated.                                  |
| ![img](img/fractal4.png) | 900           | 156          | That's starting to look correct.                         |
| ![img](img/fractal5.png) | 650           | 781          | FPS has started to tank.                                 |
| ![img](img/fractal6.png) | 290           | 3906         | Clearly a Sierpinski triangle.                           |
| ![img](img/fractal7.png) | 69            | 19531        | Performance has dropped below my monitor's refresh rate. |
| ![img](img/fractal8.png) | 10            | 97656        | A noticeable freeze occurred when adding the last layer. |
| ![img](img/fractal9.png) | 2             | 488281       | This almost crashed Unity.                               |

In my opinion there are obvious visual benefits to increasing depth. However,
the CPU has to handle computing all of these things twice (with URP) - One cast
for the object, and one cast for the shadow. As such, performance severely
degrades quickly. This problem exacerbates with rotations on fractals:

![img](img/rotatingFractal.gif)

At a depth of 7, or 19,531 objects, this cuts the FPS down from 69 to 40 on my
machine. As such, a series of updates can be made:

| Version                  | FPS (Approx.) | Notes                                                                                                                                                     |
| ------------------------ | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| v1: Initial              | 40            | This is at depth=7, or 19,531 objects, with rotations, for comparison's sake.                                                                             |
| v2: Flat hierarchy       | 48            | Rather than recursively nesting objects, placing them flatly in Unity's hierarchy speeds up its internal procedures.                                      |
| v3: Procedural drawing   | 200           | Rather than render each sphere as a separate GameObject, we can procedurally draw them on the GPU and avoid any overhead of thousands of components.      |
| v4: Jobs                 | 460           | Unity contains a system to automatically parallelize processes and aggressively optimize them. We'll use these to give more headroom for other code.      |
| v5: Optimizing for Burst | 800           | Unity's Burst compiler can be much more aggressive with our job system, so we optimize our data types with that in mind.                                  |
| v6: Sending less data    | 830           | The matrices we send to the GPU always have the same vector on the bottom row. As such, we don't have to send it, reducing our data transfer by 25% size. |
| v7: Using multiple cores | 900           | This is all running on one singular CPU core. Let's use multiple.                                                                                         |

Our implementation is much faster, and we're left with a far more performant
result. A depth of 9 ended up getting about 41 FPS on my machine, which is
significantly better than previous results.

![img](img/fasterFractal.gif)

## Part 6 - [Organic Variety](https://catlikecoding.com/unity/tutorials/basics/organic-variety/)

Now that our fractal is much faster, we can make it look a bit more 'life-like'.
In my opinion, I quite like the alien nature and shaping of the structure, so
I'll play with the colors.

| Image                              | Notes                                                                                                                                                                        |
| ---------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ![img](img/grayscaleFractal.gif)   | The implementation of changing colors by hierarchy means that depth should be modified in order to not divide by zero.                                                       |
| ![img](img/gradientFractal.gif)    | This kind of looks like a molecule. A strange artifact was occurring due tomeshes overlapping at the origin, which I decided to leave in as a feature (and ignore as a bug). |
| ![img](img/instanceIdentifier.png) | This applies coloration through the GPU.                                                                                                                                     |
| ![img](img/hashed.png)             | Applying an arbitrary modulo 'hashes' the output into looking more random.                                                                                                   |
| ![img](img/weyl.png)               | Applying a Weyl sequence and reducing the depth further helps, but we still have this ugly vertical column.                                                                  |
| ![img](img/randomFactors.png)      | Applying random offsets varies the numbers more. It does reintroduce that weird mesh error, though.                                                                          |
| ![img](img/clowny.png)             | Re-introducing color can be done with two gradients, which are interpolated between. Neat!                                                                                   |
| ![img](img/gradientEval.png)       | We can also specify particular colors for particular segments. The leaves here are green. I also set the base to black and ignored the meshing bug.                          |
| ![img](img/cubeLeaves.png)         | Other than changing the color alone, we can also change the mesh on a per-level basis.                                                                                       |

# License TL;DR

This project is distributed under the MIT license. This is a paraphrasing of a
[short summary](https://tldrlegal.com/license/mit-license).

This license is a short, permissive software license. Basically, you can do
whatever you want with this software, as long as you include the original
copyright and license notice in any copy of this software/source.

## What you CAN do:

-   You may commercially use this project in any way, and profit off it or the
    code included in any way;
-   You may modify or make changes to this project in any way;
-   You may distribute this project, the compiled code, or its source in any
    way;
-   You may incorporate this work into something that has a more restrictive
    license in any way;
-   And you may use the work for private use.

## What you CANNOT do:

-   You may not hold me (the author) liable for anything that happens to this
    code as well as anything that this code accomplishes. The work is provided
    as-is.

## What you MUST do:

-   You must include the copyright notice in all copies or substantial uses of
    the work;
-   You must include the license notice in all copies or substantial uses of the
    work.

If you're feeling generous, give credit to me somewhere in your projects.
