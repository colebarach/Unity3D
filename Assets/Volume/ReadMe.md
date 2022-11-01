# Volume Package
This package contains any code related to volumes in 3D space. A general outline of the scripts is given below.

## Volume Object
This script contains the basic concept of a volume. A volume can be either global or local. Global volumes are treated as being the inverse of every local volume. The weight of a volume determines it effect on the scene (0 has no effect, 1 has max effect). The blend distance of a volume determines its effect on the area around it.

The impact of a volume is measured using the GetWeight function. This function calculates the effect of the volume based on the proximity of the test point and the effect of other volumes. A volume's shape is defined by its colliders.

## Volume Masters
For any desired type of volume (lighting, postprocessing, sound) an associated master script must be created. This script should sum the effects of the individual volumes then apply these effects appropriately. For a lighting volume, the master script should sum the ambient lighting of every volume and apply the calculated ambience to the scene.

## Volume Properties
For the desired type of volume, the properties of individual volumes should be held in a properties script. This scipt simply contains the properties of the volume such that they can be easily modified. For a lighting volume, the properties should contain an ambient light color, and whatever else may be appropriate.