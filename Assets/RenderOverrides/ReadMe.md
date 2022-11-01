# Render Overrides Package
This package contains generic overrides for the standard rendering process. Overrides exist as either C# scripts that modify the render source, or as materials with a custom shader that the source is blitted to.
## Render Override Master
This is the main script for the package. This script intercepts a camera's rendered image and allows it to be modified before passing it to the target destination.
## Render Override Resolution
This script overrides the resolution variable of the master script. The resolution can be scaled based on the vertical or horizontal axis of the screen, which can be multiplied by an arbitrary scalar.