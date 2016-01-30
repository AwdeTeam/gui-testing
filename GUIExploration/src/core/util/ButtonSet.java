package core.util;

import java.awt.Graphics;
import java.util.ArrayList;

import core.Drawable;
import core.boxes.Box;
import core.boxes.ButtonBox;

public class ButtonSet implements Drawable
{
	private int length;
	private ArrayList<ButtonBox> buttons;
	private int xpos;
	private int ypos;
	
	public ButtonSet(int x, int y, int l)
	{
		this();
		xpos = x;
		ypos = y;
		length = l;
	}
	
	public ButtonSet()
	{
		buttons = new ArrayList<ButtonBox>();
	}
	
	public Graphics draw(Graphics g)
	{
		try{
			int space = (length / buttons.size()) - (ButtonBox.BUTTON_SIZE / 2);
			for(int i = 0; i < buttons.size(); i++)
			{
				buttons.get(i).move(xpos + space * (i+0), ypos);
				g = buttons.get(i).draw(g);
			}
		}catch(ArithmeticException e){}
		
		return g;
	}
	
	public void add(ButtonBox bb)
	{
		buttons.add(bb);
	}
	
	public void clipTop(Box b)
	{
		length = b.getWidth();
		xpos = b.getX();
		ypos = b.getY() - ButtonBox.BUTTON_SIZE;
		for(ButtonBox bb : buttons)
			bb.input = true;
	}
	
	public void clipBottom(Box b)
	{
		length = b.getWidth();
		xpos = b.getX();
		ypos = b.getY() + b.getHeight();
		for(ButtonBox bb : buttons)
			bb.input = false;
	}
	
	public ArrayList<ButtonBox> getButtons()
	{
		return buttons;
	}
}
