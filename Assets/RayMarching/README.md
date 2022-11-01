# Ray Marching
This package is an implementation of the Ray Marching algorithm of rendering.
## Ray Marching Renderer (Object)
This component may be attached to a gameObject to indicate it should be rendered. It contains information about the object's shape and material.
## Ray Marching Camera
The main object, the Ray Marching Camera, is responsible for rendering a scene. It begins by creating a buffer from all of the RayMarchingRenderers in the scene, containing the renderer's information combined with the objects transform matrix. After this, the buffer and the camera's rendering properties are sent to the RayMarchRenderer shader.
## Ray Marching Renderer (Shader)
This shader is a compute shader that contains all of the code for Ray Marching itself. The shader references an include file which contains all of the information for the objects distance functions, indicated by the objects render ID. The shader will generate an image, which is sent back to the camera and rendered to the screen.