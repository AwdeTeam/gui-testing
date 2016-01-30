package core.boxes;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.LinearGradientPaint;
import java.awt.Stroke;
import java.awt.event.MouseEvent;
import java.awt.geom.Point2D;
import java.awt.geom.RoundRectangle2D;
import java.util.Collection;

import core.Drawable;
import core.util.NodeWrapper;
import core.util.SignalProfile;

public abstract class DefaultBox implements Box
{
	protected int wide = 25;
	protected int high = 25;
	protected int xpos = 0;
	protected int ypos = 0;
	
	public final static int BOX_BORDER_THICKNESS = 3;
	protected Color baseColor = new Color(  0,  64, 183);
	protected Color bordColor = new Color(  0,  38, 110);
	protected Color lighColor = new Color(102, 144, 224);
	protected Color highColor = new Color(242, 183,  55);
	
	public boolean selected = false;
	
	//ButtonSet set0, set1;
	
	public DefaultBox(Dimension size, Dimension position)
	{
		wide = size.width;
		high = size.height;
		xpos = position.width;
		ypos = position.height;
		
		/*set0 = new ButtonSet();
		set0.add(new ButtonBox());
		set0.add(new ButtonBox());
		set0.clipBottom(this);
		
		set1 = new ButtonSet();
		set1.add(new ButtonBox());
		set1.add(new ButtonBox());
		set1.add(new ButtonBox());*/
	}
	
	public DefaultBox(int w, int h, int x, int y)
	{
		this(new Dimension(w, h), new Dimension(x, y));
	}
	
	public DefaultBox(){}
	
	@Override
	public Graphics draw(Graphics g) 
	{
		Color c = g.getColor();
		Stroke s = ((Graphics2D) g).getStroke();
		
		g.setColor(baseColor);
		Point2D start = new Point2D.Float(xpos, ypos);
		Point2D end = new Point2D.Float(xpos + wide, ypos + high);
		float[] dist = {0.0f, 0.5f, 0.55f, 1.0f};
		Color[] colors = {baseColor, lighColor, baseColor, baseColor};
		LinearGradientPaint p = new LinearGradientPaint(start, end, dist, colors);
		((Graphics2D) g).setPaint(p);
		g.fillRoundRect(xpos, ypos, wide, high, 10, 10);
		
		((Graphics2D) g).setStroke(new BasicStroke(BOX_BORDER_THICKNESS));
		g.setColor(selected ? highColor : bordColor);
		((Graphics2D) g).draw(new RoundRectangle2D.Double(xpos, ypos, wide, high, 10, 10));
		
		g.setColor(c);
		((Graphics2D) g).setStroke(s);
		
		return g;
	}

	public boolean pointCheck(int x, int y) 
	{
		return x > xpos && x < xpos + wide && y > ypos && y < ypos + high;
	}
	
	public boolean pointCheck(MouseEvent e) 
	{
		return pointCheck(e.getX(), e.getY());
	}

	@Override
	public void move(int x, int y) 
	{
		xpos = x;
		ypos = y;
	}

	@Override
	public int getX() { return xpos; }

	@Override
	public int getY() { return ypos; }

	@Override
	public int getWidth() { return wide; }

	@Override
	public int getHeight() { return high; }

	public abstract Collection<Box> getAll();

	public abstract Drawable getTopButtons();
	public abstract Drawable getBottomButtons();
	public abstract ButtonBox getTop(int index);
	public abstract ButtonBox getBottom(int index);
	
	public abstract String getName();
	public abstract String getBrief();
	public abstract String getDescription();
	
	public abstract SignalProfile getInputProfile();
	public abstract SignalProfile getOutputProfile();
	public abstract NodeWrapper getNode();
	
	protected Color colorShift(Color color, float shift)
	{
		int r = trunk(color.getRed() * shift);
		int g = trunk(color.getGreen() * shift);
		int b = trunk(color.getBlue() * shift);
		
		return new Color(r, g, b);
	}
	
	private int trunk(float f) 
	{
		if(f > 255)
			return 255;
		return (int)f;
	}
}
