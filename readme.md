# Furious Birds
Remake of Angry Birds using Unity. Consist of 3 levels and 1 final level.

## Improvements
From the base tutorial I make lots of improvements which are:
- Refactor base code so that each script has single purpose responsibility and optimize performance (remove unused delegate, GetComponent function call, etc)
- Implement dynamic camera control that follows bird upon shooting, zoom in and out, and also pan to look around the map
- Woods now can be destroyed
- Add many particles system to make the gameplay more "alive" like bird fur when the bird dies, smokes when the woods and pigs dissapear, and explosion effect when the brown bird explodes
- Add rope on the slingshooter
- Bird trail has contant distance between each other independent of the bird shoot initial velocity
- Aim trajectory trail has contant length independent of the bird shoot initial velocity

## Showcase
![showcase](showcase.gif)

## Controls
Uses mouse only. Pan to move around the map, scroll to zoom in or zoom out.