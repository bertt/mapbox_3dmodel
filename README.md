# mapbox_3dmodel

Experiment of drawing dynamic 3D glTF models in MapBox GL JS

Input: GeoJSON file with 3D Points

Output: GlTF/GLB with 3D model. Points are converted to 3D cubes and the points are classified using the "amount" attribute. 

Live site: https://bertt.github.io/mapbox_3dmodel/

In the site a MapBOX GL Javascript map is displayed. The 3D model is drawn on top using the Three.JS library.

Dependencies for creating model:

- SharpGLTF: https://github.com/vpenades/SharpGLTF

- GeoJSON.NET: https://github.com/GeoJSON-Net/GeoJSON.Net
