# RandomWalkOnSphere

RandomWalkOnSphere es un proyecto de Unity que muestra justo lo que dice su nombre: un camino generado proceduralmente sobre una esfera, el cual sigue un tipo de [paseo aleatorio](https://en.wikipedia.org/wiki/Random_walk)

A parte de la generación de este camino se adorna la escena con una serie de efectos especiales basados en partículas y algún shader.

##El camino aleatorio
Como decía arriba, en este proyecto se genera un camino aleatorio sobre una esfera. Esto se hace en el script [DrawLineMesh.cs](/Assets/Scripts/DrawLineMesh.cs). Las partes importantes de este script son,

`Inicializamos el primer vertice`
```csharp
v[0] = radius*Random.onUnitSphere;
indv[0] = 0;
uv[0] = new Vector2(0f,0f);
Vector3 vel = Vector3.zero;
```
`Definimos el resto de vertices de la malla. Cada uno, en funcion del anterior`
```csharp
for(int i=1;i<N;i++){
	vel+= delta0*Random.insideUnitSphere;
	v[i] = v[i-1] + delta1*vel + delta2*Random.insideUnitSphere;
	vel.Normalize();
	v[i].Normalize();
	v[i] *= radius;
	indv[i] = i;
	uv[i] = new Vector2(i/(N-1f),0f);
}
```
`Definimos la topologia de la malla`
```csharp
mesh.SetIndices(indv,MeshTopology.LineStrip,0);
```

La idea básica es fijar una posición inicial sobre la esfera, y a partir de ahí ir definiendo los siguientes vértices, a partir de los anteriores, mediante pequeños movimientos.

