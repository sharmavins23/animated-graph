# Animated Graph

This project is a follow-along of
[Jasper Flick's Catlike Coding](https://catlikecoding.com/) Unity
[basics]https://catlikecoding.com/unity/tutorials/basics/) tutorials. This
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
here's a small gif:

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
