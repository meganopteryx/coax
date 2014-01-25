private var countr =0;
private var countg =0;
private var countb =0;
private var rshift = 0.0007;
private var gshift = 0.0015;
private var bshift = 0.0021;

public var mat1 : Material;
private var color : Color;
private var startcolor : Color;

function Awake()
{
	//mat1 = renderer.sharedMaterial;
	startcolor = mat1.GetColor("_TintColor");
	color = startcolor;
	
	//Start w/ decrease
	rshift = -rshift;
	gshift = -gshift;
	bshift = -bshift;

}


function FixedUpdate () 
{
	countr++;
	countb++;
	countg++;
	if (countr==100)
	{
		rshift = -rshift;
		countr = 0;
	}
	if (countg==150)
	{
		gshift = -gshift;
		countg = 0;
	}
	if (countb==200)
	{
		bshift = -bshift;
		countb = 0;
	}
		
	color.r += rshift;
	color.g += gshift;
	color.b += bshift;
	
	mat1.SetColor("_TintColor", color);
}

function OnDestroy()
{
	mat1.SetColor("_TintColor", startcolor);
	
	rshift = Mathf.Abs(rshift);
	gshift = Mathf.Abs(gshift);
	bshift = Mathf.Abs(bshift);
}