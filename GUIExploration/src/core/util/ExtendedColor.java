package core.util;

import java.awt.Color;

public class ExtendedColor extends Color
{
	public ExtendedColor(int r, int g, int b) 
	{
		super(r, g, b);
	}
	
	public ExtendedColor(int r, int g, int b, int a) 
	{
		super(r, g, b, a);
	}
	
	/*public Color value(float adj)
	{
		float[] vals = RGBtoHSB(getRed(), getGreen(), getBlue(), null);
		
	}*/
}
