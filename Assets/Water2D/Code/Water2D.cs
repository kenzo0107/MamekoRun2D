using UnityEngine;
using System.Collections;

using Audio;

public class Water2D : MonoBehaviour {

	#region Public vars
	
	/// <summary>
	/// The width of the water plane
	/// </summary>
	public float 		width = 100;
	
	/// <summary>
	/// The height of the water plane
	/// </summary>
	public float 		height = 100;
	
	/// <summary>
	/// Number of vertical subdivisions. This will set the plane resolution
	/// </summary>
	public int			waterSubdivisions = 1;
	
	
	/// <summary>
	/// Its the force of each srping. Highr values gives a moe gelly filling
	/// </summary>
	public float 		Tension = 0.025f;
	
	/// <summary>
	///  Foce that goes againt the spring movement. Use it to stabilize the springs
	/// </summary>
	public float 		Dampening = 0.025f;
	
	/// <summary>
	/// How far the force is going to spread to the neighbours springs
	/// </summary>
	public float 		Spread = 0.25f;
	
	/// <summary>
	/// When a big object is thrown to the water N springs will be affected. This value reduces the force applied to the neighbours
	/// </summary>
	public float 		objectSizeDampening = 1f; 
	
	/// <summary>
	/// How many neighbours are going to be affected when a object enters the water. Biger value = lower performance
	/// </summary>
	public int			neighbours			= 8;
	
	/// <summary>
	/// The water material
	/// </summary>
	public Material		waterMaterial;
	
	/// <summary>
	/// The uv animation of the material
	/// </summary>
	public Vector2		uvAnimationSpeed = new Vector2(1,0);
	
	/// <summary>
	/// The particle effect that is going to be instantiated when an object enters or exits the water
	/// </summary>
	public GameObject 	waterSplash; //Big splash
	
	/// <summary>
	/// Particle effect to use when a water wake excees the water "heightlimit to splash particles"
	/// </summary>
	public GameObject 	afterPeakwaterSplash; //Small ones
	
	/// <summary>
	/// The height limit to splash particles. After an object enters the water a wave appears. If this wave goes over the limit a particle emission 
	/// will be done in its peak (only qhen it goes down)
	/// </summary>
	public float		heightLimitToSplashParticles = 10;
	
	/// <summary>
	/// The splash sounds. Will be selected randomy
	/// </summary>
	public AudioClip[]	splashSounds;
	
	/// <summary>
	/// The force applied when the water is in idle to simulate a bit of movement. Be very conservative with this
	/// </summary>
	public float		idleFactor;
	
	//The speed of the idle waves
	public float 		idleWavesSpeed;
	
	/// <summary>
	/// The color of the deep. The water plane uses vertex color for this
	/// </summary>
	public Color		deepColor 				= new Color(0.2f,0.2f,0.2f,1);
	
	/// <summary>
	/// The color of the surface. The water plane uses vertex color for this
	/// </summary>
	public Color		surfaceColor 			= new Color(1f,1f,1f,1);
	
	/// <summary>
	/// The width of the surface line. No line will be created if this value is 0
	/// </summary>
	public float		surfaceLineWidth		= 1;	
	
	/// <summary>
	/// The surface line material.
	/// </summary>
	public Material		surfaceLineMaterial;
	
	/// <summary>
	/// The color of the surface line.
	/// </summary>
	public Color		surfaceLineColor		= new Color(0,0,0.8f,0.5f);	
	
	/// <summary>
	/// Automatically create collider. This will create a box collider of the correct size and set it to trigger
	/// </summary>
	public bool			createCollider			= true;
	
	/// <summary>
	/// The collider Z size.
	/// </summary>
	public float		colliderZsize			=	100;

	/// <summary>
	/// This will enable 2D physics on the water instead of 3D
	/// </summary>
	public bool			use2DPhysics			= false;
	 
	/// <summary>
	/// Clamp the max force applied when an object enters the water
	/// </summary>
	public float		maxWaterForceApplied	= 50;

	/// <summary>
	/// The draw debug info. Only on editor. It may cause slowdowns when the water object is selected
	/// </summary>
	public bool			drawDebugInfo			= false;
	
	#endregion


	class WaterColumn
	{
		public float 	TargetHeight;
		public float 	Height;
		public float 	Speed;		
		public float	lastHeight;	
		public Water2D	water;
		public int		index;
		public bool		enableSplash; //Only the first column hit can splash on rebounce
		
		private bool	hasToSplash;
		
		
		public void Update(float dampening, float tension)
		{

			float x = TargetHeight - Height;
			Speed += tension * x - Speed * dampening ;
			Height += Speed;
			
			if (Height > TargetHeight+ water.heightLimitToSplashParticles)
				hasToSplash = true;
			else
				hasToSplash = false;
		
			
			if (enableSplash && hasToSplash && Height < lastHeight )
			{
				water.SplashParticles(index);
				enableSplash = false;
			}
			
			
			lastHeight = Height;
		}
	}
	
	
	#region Private vars
	
	private WaterColumn[] 		columns;
	private Mesh 				proceduralMesh;
	private Vector3[]			meshVertices;
	private Mesh				myMesh;
	private int 				firstUpVertex;
	private ParticleSystem		instantiatedWaterSplash;
	private ParticleSystem		instantiatedAfterPeakwaterSplash;
	private Transform			myTransform;
	private BoxCollider			myCollider;
	private float[] 			lDeltas;
	private float[]				rDeltas;
	
	//Surface lines
	private LineRenderer		surfaceLineRenderer;
	
	//Idle waves
	private float idleWaveCounter;
	private int lastIdleWaveColumn = -1;
	
	//Material
	private Material			myMaterial;
		
	
	
	#endregion


	void Awake()
	{
		if (GetComponent<MeshFilter>() == null)
			CreateWaterPlane();
		
		InitializeWater();
		
		//Cache vars
		myTransform = transform;
	}
	
	public void CreateWaterPlane()
	{
		int widthSegments = waterSubdivisions;
		int lengthSegments = 1;
		
		//Check if there is any mesh or renderer created 
		MeshFilter tempMeshfilter = GetComponent<MeshFilter>();
		
		if (Application.isEditor)
			DestroyWater();
		
		if (tempMeshfilter != null)
			DestroyImmediate(tempMeshfilter);
		
		
		if (renderer != null)
			DestroyImmediate(renderer);
		
		//Lets create the mesh
        proceduralMesh = new Mesh();
        proceduralMesh.name = "Water plane";
		
        int hCount2 = widthSegments+1;
		
        int vCount2 = lengthSegments+1;
        int numTriangles = widthSegments * lengthSegments * 6;
        int numVertices = hCount2 * vCount2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles];

        int index = 0;
        float uvFactorX = 1.0f/widthSegments;
        float uvFactorY = 1.0f/lengthSegments;
        float scaleX = width/widthSegments;
        float scaleY = height/lengthSegments;
        for (float y = 0.0f; y < vCount2; y++)
        {
            for (float x = 0.0f; x < hCount2; x++)
            {
                vertices[index] = new Vector3(x*scaleX - width/2f, y*scaleY - height/2f, 0.0f);
                
				uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
            }
        }

        index = 0;
        for (int y = 0; y < lengthSegments; y++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                triangles[index]   = (y     * hCount2) + x;
                triangles[index+1] = ((y+1) * hCount2) + x;
                triangles[index+2] = (y     * hCount2) + x + 1;

                triangles[index+3] = ((y+1) * hCount2) + x;
                triangles[index+4] = ((y+1) * hCount2) + x + 1;
                triangles[index+5] = (y     * hCount2) + x + 1;
                index += 6;
            }
        }

		//Vertex color:
		Color[] colors = new Color[vertices.Length];
		
		
		for (int i = 0; i <(int)(vertices.Length*0.5f); i++) //Deep vertex
		{
			colors[i] = deepColor;
		}
		for (int i = (int)(vertices.Length*0.5f); i <vertices.Length; i++) //Surface vertex
		{
			colors[i] = surfaceColor;
		}
		
		
		//Asign mesh properties
		proceduralMesh.vertices 	= vertices;
		proceduralMesh.triangles 	= triangles;
		proceduralMesh.uv 			= uvs;
		proceduralMesh.colors		= colors;
		
		MeshFilter myMeshFilter = gameObject.AddComponent<MeshFilter>();
		myMeshFilter.mesh = proceduralMesh;
		gameObject.AddComponent<MeshRenderer>();
		
		//Set collider properties
	
		if (createCollider)
		{
			if (!use2DPhysics && collider == null)  // 3D Collisions
			{
				BoxCollider	tempCollider =  gameObject.AddComponent<BoxCollider>();
				Vector3 tempColliderSize = tempCollider.size;
				tempColliderSize.z = colliderZsize;
				tempCollider.size = tempColliderSize;
				tempCollider.isTrigger = true;	
			}
			else if (use2DPhysics && collider2D == null)  //2D Collisions
			{
				BoxCollider2D tempCollider = gameObject.AddComponent<BoxCollider2D>();
				tempCollider.isTrigger = true;
			}
		}
		
		//Set surface lines properties
		if (surfaceLineWidth > 0 )
		{
			surfaceLineRenderer = gameObject.AddComponent<LineRenderer>();
			surfaceLineRenderer.SetWidth(surfaceLineWidth,surfaceLineWidth);
			surfaceLineRenderer.SetColors(surfaceLineColor,surfaceLineColor);
			surfaceLineRenderer.SetVertexCount(waterSubdivisions+1);
			surfaceLineRenderer.sharedMaterial = surfaceLineMaterial;
			UpdateSurfaceLinePosition();
		}
		
		//Set material
		renderer.sharedMaterial = waterMaterial;
	}
	
	public void DestroyWater()
	{
		DestroyImmediate(renderer);
		DestroyImmediate(collider);
		DestroyImmediate(collider2D);
		DestroyImmediate(GetComponent<MeshFilter>());
		DestroyImmediate(GetComponent<LineRenderer>());	
	}
	
	private void InitializeWater()
	{
		//Cache mesh and material
		myMesh = GetComponent<MeshFilter>().mesh;
		myMesh.MarkDynamic();
		myMaterial = renderer.material;
		surfaceLineRenderer = GetComponent<LineRenderer>();
		myCollider = collider as BoxCollider;
		
		
		meshVertices = myMesh.vertices;
			
		//Store first up vertice for future calcs
		firstUpVertex = (int)(meshVertices.Length*0.5f);
		
		//Initilize water physics. This is setting up each spring/column 
		columns = new WaterColumn[waterSubdivisions+1];
		float topWaterIdlePosition = meshVertices[meshVertices.Length-1].y; //Get the y coordinate of one of the top water vertex
		
		//Set properties for each spring
		for (int i = 0; i < columns.Length ; i++)
		{
			columns[i] = new WaterColumn();
			columns[i].Height = topWaterIdlePosition;
			columns[i].TargetHeight = topWaterIdlePosition;
			columns[i].Speed = 0;
			columns[i].lastHeight = topWaterIdlePosition;
			columns[i].water = this;
			columns[i].index = i;
		}
		
		//Get particles ready
		GameObject tempWaterSplash = Instantiate(waterSplash) as GameObject;
		instantiatedWaterSplash = tempWaterSplash.particleSystem;
		tempWaterSplash = Instantiate(afterPeakwaterSplash) as GameObject;
		instantiatedAfterPeakwaterSplash = tempWaterSplash.particleSystem;

		lDeltas = new float[columns.Length];
		rDeltas = new float[columns.Length];
	}



	void Update()
	{
		for (int i = 0; i < columns.Length; i++)
			columns[i].Update(Dampening, Tension);
		
		// Do some passes where columns pull on their neighbours
		for (int j = 0; j < neighbours; j++)
		{
			for (int i = 0; i < columns.Length; i++)
			{
				if (i > 0)
				{
					lDeltas[i] = Spread * (columns[i].Height - columns[i - 1].Height);
					columns[i - 1].Speed += lDeltas[i];
				}
				if (i < columns.Length - 1)
				{
					rDeltas[i] = Spread * (columns[i].Height - columns[i + 1].Height);
					columns[i + 1].Speed += rDeltas[i];
				}
			}

			for (int i = 0; i < columns.Length; i++)
			{
				if (i > 0)
					columns[i - 1].Height += lDeltas[i];
				if (i < columns.Length - 1)
					columns[i + 1].Height += rDeltas[i];
			}
		}
		
		//Update vertex data
		int columIterator = 0;
		for (int i = firstUpVertex; i <  meshVertices.Length ; i++)
		{
			meshVertices[i].y = columns[columIterator].Height;
			columIterator++;
		}
		myMesh.vertices = meshVertices;
		
		if (idleFactor > 0)
		{
				
			idleWaveCounter = Mathf.MoveTowards(idleWaveCounter,columns.Length-1,Time.deltaTime*idleWavesSpeed);
			
			if((int)idleWaveCounter != lastIdleWaveColumn)
			{
				MoveSpring((int)idleWaveCounter, idleFactor);
				lastIdleWaveColumn = (int)idleWaveCounter;
			}
			
			if (idleWaveCounter == columns.Length-1)
				idleWaveCounter = 0;
		}
		
		//Update line position
		if (surfaceLineWidth > 0)
			UpdateSurfaceLinePosition();
		
		
		//Update material UV
		if (uvAnimationSpeed.sqrMagnitude != 0)
		{
			myMaterial.mainTextureOffset = uvAnimationSpeed*Time.timeSinceLevelLoad;
		}
			
	
	}
	
	#region Public methods
	/// <summary>
	/// When an object enters the water.
	/// </summary>
	/// <param name='_position'>
	/// Where the object is falling into the water
	/// </param>
	/// <param name='_speed'>
	/// How fast is going. Negative is into de water direction
	/// </param>
	/// <param name='_size'>
	/// The size of the object determine how many springs are going to be affected
	/// </param>
	public void ObjectEnteredWater(Vector3 _position, float _speed, float _size, bool _emitParticles)
	{
		//We have to get the nearest spring to apply the force
		
		float currentMinDistance = float.MaxValue;
		int selectedVertex 	= 0;
		int selectedColum 	= 0;
		for (int i = firstUpVertex; i <  meshVertices.Length ; i++)
		{
			float distance = (myTransform.TransformPoint(meshVertices[i])-_position).sqrMagnitude;
			
			if (distance < currentMinDistance)
			{	
				currentMinDistance = distance;
				selectedVertex = i;
				selectedColum = i-firstUpVertex;
			}
		}
		
		//Set colums state 
		columns[selectedColum].Speed 			+= _speed;
		columns[selectedColum].enableSplash  	= true;
		
		//Affect other springs to simulate a bigger object
		
		//Get how many springs are going to be affected
		float distanceBetweenSprings =  width/waterSubdivisions;
		int affectedSprings = Mathf.Clamp(Mathf.FloorToInt(_size/distanceBetweenSprings), 0, columns.Length-1);
		
		
		for (int i = 1; i <affectedSprings ; i++)
		{
			if (selectedColum+i < columns.Length)
				columns[selectedColum+i].Speed = _speed / (objectSizeDampening * (i+1));
			if (selectedColum-i >= 0)
				columns[selectedColum-i].Speed = _speed / (objectSizeDampening * (i+1));
		}
		
		//FX and sound
		if(_emitParticles)
		{
			instantiatedWaterSplash.transform.position =  myTransform.TransformPoint(meshVertices[selectedVertex]);
			instantiatedWaterSplash.Emit(Random.Range(10,20));
		}
		
		//Audio splash
		audio.PlayOneShot(splashSounds[Random.Range(0, splashSounds.Length)]);
	}
	
	public void ObjectEnteredWater(Vector3 _position, float _speed, Bounds _size, bool _emitParticles)
	{
		//Calc size based on bounds
		ObjectEnteredWater(_position,_speed,_size.extents.x,_emitParticles);
		
	}
	
	
	/// <summary>
	/// Sets the water height. Usefull for changing it at run time
	/// </summary>
	/// <param name='_newHeight'>
	/// The new height. 0 its the pivot
	/// </param>
	public void SetHeight(float _newHeight)
	{
		//Resize water
		for (int i = 0; i < columns.Length ; i++)
		{
			columns[i].TargetHeight = _newHeight;
		}
		
		//Resize collider
		Vector3 colliderSize = myCollider.size;
		colliderSize.x = width;
		colliderSize.y = height*0.5f + _newHeight;
		myCollider.size = colliderSize;
		myCollider.center = new Vector3(0,-height*0.25f+_newHeight*0.5f,0);
	}
	#endregion
	
	
	private void MoveSpring(int _springIndex, float _speed)
	{
		columns[_springIndex].Speed 			+= _speed;
	}
	
	void SplashParticles(int _column)
	{
		instantiatedAfterPeakwaterSplash.transform.position =  myTransform.TransformPoint(meshVertices[_column+firstUpVertex]);
		instantiatedAfterPeakwaterSplash.Emit(Random.Range(5,9));
	}
	
	private void UpdateSurfaceLinePosition()
	{
		if (meshVertices == null)
		{
			meshVertices = GetComponent<MeshFilter>().sharedMesh.vertices;
			firstUpVertex = (int)(meshVertices.Length*0.5f);
		}

#if UNITY_EDITOR
		if (myTransform == null)
			myTransform = transform;
#endif

		for (int i = 0; i < meshVertices.Length - firstUpVertex; i ++)
		{
			surfaceLineRenderer.SetPosition(i,myTransform.TransformPoint(meshVertices[i+firstUpVertex]));
		}
	}
	
	#region Collision


	void OnTriggerEnter(Collider _collider)  //3D collision
	{
		//Get the contact point
		Vector3 contactPoint =  _collider.ClosestPointOnBounds(_collider.transform.position);
		
		//Get the regidbody speed
		Vector3 rigidBodySpeed = _collider.rigidbody.velocity;
		
		//We only use the vertical component
		
		ObjectEnteredWater(contactPoint, Mathf.Clamp(rigidBodySpeed.y, -maxWaterForceApplied,maxWaterForceApplied), _collider.bounds, true);
		
	}
	
	void OnTriggerExit(Collider _collider)  //3D collision
	{
		//Get the contact point
		Vector3 contactPoint =  _collider.ClosestPointOnBounds(_collider.transform.position);
		
		//Get the regidbody speed
		Vector3 rigidBodySpeed = _collider.rigidbody.velocity;
		
		//We only use the vertical component
		
		ObjectEnteredWater(contactPoint, Mathf.Clamp(rigidBodySpeed.y, -maxWaterForceApplied,maxWaterForceApplied), _collider.bounds, true);
		
	}

	void OnTriggerEnter2D(Collider2D _collider2D)
	{
		//Get the contact point
		Vector3 contactPoint =  new Vector3(_collider2D.transform.position.x,myTransform.position.y+height*0.5f,myTransform.position.z);
		
		//Get the regidbody speed
		Vector3 rigidBodySpeed = _collider2D.rigidbody2D.velocity;
		
		//We only use the vertical component
		if ( _collider2D.renderer ) {
			ObjectEnteredWater(contactPoint, Mathf.Clamp(rigidBodySpeed.y, -maxWaterForceApplied,maxWaterForceApplied), _collider2D.renderer.bounds, true);
		}
	}

	void OnTriggerExit2D(Collider2D _collider2D)
	{
		//Get the contact point
		Vector3 contactPoint =  new Vector3(_collider2D.transform.position.x,myTransform.position.y+height*0.5f,myTransform.position.z);
		
		//Get the regidbody speed
		Vector3 rigidBodySpeed = _collider2D.rigidbody2D.velocity;
		
		//We only use the vertical component
		if ( _collider2D.renderer ) {
			ObjectEnteredWater(contactPoint, Mathf.Clamp(rigidBodySpeed.y, -maxWaterForceApplied,maxWaterForceApplied), _collider2D.renderer.bounds, true);
		}
	}
	
	
	#endregion
	
	
	void OnDrawGizmosSelected()
	{
		if (!drawDebugInfo)
			return;


		Gizmos.color =Color.blue;
		Gizmos.DrawWireCube(transform.position, new Vector3(width,height,0));		
		Gizmos.color = Color.red;
		
		//Splash line
		Vector3 lineHeightStart = new Vector3 (transform.position.x-width*0.5f,transform.position.y+height*0.5f+heightLimitToSplashParticles, transform.position.z);
		Vector3 lineHeightFinish = new Vector3 (transform.position.x+width*0.5f,transform.position.y+height*0.5f+heightLimitToSplashParticles, transform.position.z);
		Debug.DrawLine(lineHeightStart, lineHeightFinish, Color.red);
		
		MeshFilter tempMeshFilter = GetComponent<MeshFilter>();
		
		if (tempMeshFilter != null)
		{
			Mesh tempMesh = tempMeshFilter.sharedMesh;
			
			int upperVertexStart = (int)(tempMesh.vertices.Length*0.5f);
			
			for (int i = upperVertexStart; i <  tempMesh.vertices.Length; i++)
				Gizmos.DrawCube(transform.TransformPoint(tempMesh.vertices[i]),new Vector3 (1,1,0)*surfaceLineWidth);
			
		}
	}

}
