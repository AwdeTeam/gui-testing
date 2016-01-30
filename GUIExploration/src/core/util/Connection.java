package core.util;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.MouseEvent;
import java.awt.geom.QuadCurve2D;

import core.Drawable;

public class Connection implements Drawable
{
	int x0;
	int y0;
	int x1;
	int y1;
	
	public Connection(Dimension start, Dimension end)
	{
		x0 = start.width;
		y0 = start.height;
		x1 = end.width;
		y1 = end.height;
	}
	
	public Connection(int x0, int y0, int x1, int y1)
	{
		this.x0 = x0;
		this.y0 = y0;
		this.x1 = x1;
		this.y1 = y1;
	}
	
	public Connection(MouseEvent e){ this(e.getX(), e.getY(), e.getX(), e.getY()); }

	public void setEnd(Dimension end)
	{
		x1 = end.width;
		y1 = end.height;
	}
	
	public void setStart(Dimension start)
	{
		x0 = start.width;
		y0 = start.height;
	}
	
	@Override
	public Graphics draw(Graphics g)
	{
		g.setColor(Color.black);
		((Graphics2D) g).setStroke(new BasicStroke(4));
		new ConnectCurve(x0, y0, x1, y1).draw(g);
		return g;
	}

}

class ConnectCurve implements Drawable
{
	int x0, y0, x1, y1;
	
	public ConnectCurve(int x0, int y0, int x1, int y1)
	{
		this.x0 = x0;
		this.y0 = y0;
		this.x1 = x1;
		this.y1 = y1;
	}
	
	@Override
	public Graphics draw(Graphics g) 
	{
		int dx = x1 - x0;
		int dy = y1 - y0;
		QuadCurve2D c0 = new QuadCurve2D.Double();
		QuadCurve2D c1 = new QuadCurve2D.Double();
		
		c0.setCurve(       x0,        y0, x0, y0 + dy/4, x0 + dx/2, y0 + dy/2);
		c1.setCurve(x0 + dx/2, y0 + dy/2, x1, y1 - dy/4,        x1,        y1);
		((Graphics2D) g).draw(c0);
		((Graphics2D) g).draw(c1);
		return g;
	}
}
