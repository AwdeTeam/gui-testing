package core.boxes;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.MouseEvent;
import java.util.ArrayList;

import core.util.Connection;

public class ButtonBox implements Box
{
	public static final int BUTTON_SIZE = 8;
	private int xpos;
	private int ypos;
	public boolean input;
	public boolean hover;
	
	ArrayList<Connection> connection = new ArrayList<Connection>();
	ArrayList<ButtonBox> conTo = new ArrayList<ButtonBox>();
	
	@Override
	public Graphics draw(Graphics g) 
	{
		Color c = g.getColor();
		Color fill = connection.size() == 0 ? Color.white : Color.green;
		g.setColor(fill);
		g.fillOval(xpos, ypos, BUTTON_SIZE, BUTTON_SIZE);
		Color bord = hover ? Color.yellow : Color.black;
		g.setColor(bord);
		((Graphics2D) g).setStroke(new BasicStroke(2));	
		((Graphics2D) g).drawOval(xpos, ypos, BUTTON_SIZE, BUTTON_SIZE);
		
		g.setColor(c);
		
		return g;
	}

	public boolean pointCheck(int x, int y) 
	{
		return x > xpos && x < xpos + BUTTON_SIZE + 4 && y > ypos && y < ypos + BUTTON_SIZE + 4;
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
		
		for(Connection c : connection)
		{
			if(c != null)
			{
				if(input)
					c.setStart(new Dimension(x, y));
				else
					c.setEnd(new Dimension(x, y));
			}
		}
	}
	
	public void connect(Connection c, ButtonBox b)
	{
		connection.add(c);
		conTo.add(b);
	}
	
	public void breakConnect()
	{
		if(connection.size() > 0)
		{
			connection.remove(connection.size()-1);
			conTo.remove(conTo.size()-1);
		}
	}

	@Override
	public int getX() { return xpos; }

	@Override
	public int getY() { return ypos; }

	@Override
	public int getWidth() { return BUTTON_SIZE; }

	@Override
	public int getHeight() { return BUTTON_SIZE; }

}
