# RandomWalkOnSphere

RandomWalkOnSphere es un proyecto de Unity que muestra justo lo que dice su nombre: un camino generado proceduralmente sobre una esfera, el cual sigue un tipo de [paseo aleatorio](https://en.wikipedia.org/wiki/Random_walk).

A parte de la generación de este camino se adorna la escena con una serie de efectos especiales basados en partículas y algún shader.

##El camino aleatorio `DrawLineMesh.cs`
Como decía arriba, en este proyecto se genera un camino aleatorio sobre una esfera. Esto se hace en el script [`DrawLineMesh.cs`](/Assets/Scripts/DrawLineMesh.cs). Las partes importantes de este script son,

```csharp
v[0] = radius*Random.onUnitSphere;
indv[0] = 0;
uv[0] = new Vector2(0f,0f);
Vector3 vel = Vector3.zero;
```
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
```csharp
mesh.SetIndices(indv,MeshTopology.LineStrip,0);
```

La idea básica es fijar una posición inicial sobre la esfera, y a partir de ahí ir definiendo los siguientes vértices, a partir de los anteriores, mediante pequeños movimientos. Por último, establecemos la topología de la malla para que la GPU sepa que hacer con los índices de los vértices que hemos definido 

<div align="center">
	<img src="https://github.com/josemorval/RandomWalkOnSphere/blob/master/img/img01.png" width="400">
	<img src="https://github.com/josemorval/RandomWalkOnSphere/blob/master/img/img02.png" width="400">
</div>


##Renderizando el camino `LineDrawerShader.shader`
Una vez se ha generado el camino hay que pintarlo. Para esto creo un material, que tiene asociado el shader [`LineDrawerShader.shader`](/Assets/Material/LineDrawerShader.shader).

Este shader tiene dos partes importantes,
```c
float BrightnessNearDrawer( float u , float appear ){
	float f = u-appear;
	return 1.0+4.0*exp(-10000.0*f*f);
}
```
```c
float4 frag(VertexOutput i) : COLOR {
	float4 colorLerpMesh = lerp(_col1,_col2,i.uv0.r);
	float brightnessNearDrawer = BrightnessNearDrawer( i.uv0.r , _appear );
	float4 blackColor = float4(0,0,0,0);

	float3 finalColor = (colorLerpMesh*step(i.uv0.r,_appear)+blackColor*step(_appear,i.uv0.r)).rgb;
	finalColor *= brightnessNearDrawer;
	float4 finalRGBA = fixed4(finalColor,1);
	return finalRGBA;
}
```

Básicamente, este shader usa la información de las coordenadas `UV` de la malla (definido en `DrawLineMesh.cs` en la instrucción `uv[i] = new Vector2(i/(N-1f),0f)`) para asignar un color a ese *fragmento* de malla. Más específicamente, interpolamos entre dos colores usando el valor `x` del `UV.

<div align="center" class="container">
	<img src="https://github.com/josemorval/RandomWalkOnSphere/blob/master/img/img03.gif" width="400">
</div>
