package core.boxes;

import java.awt.Color;
import java.awt.Graphics;
import java.util.ArrayList;
import java.util.Collection;

import core.Drawable;
import core.util.ButtonSet;
import core.util.NodeWrapper;
import core.util.SignalProfile;

public class TensorBox extends DefaultBox
{
	ButtonSet top;
	ButtonSet bot;
	
	public TensorBox()
	{
		super();
		
		top = new ButtonSet();
		top.clipTop(this);
		bot = new ButtonSet();
		bot.clipBottom(this);
	}
	
	public TensorBox(int xpos, int ypos, int wide, int high)
	{
		super(xpos, ypos, wide, high);
		
		top = new ButtonSet();
		top.clipTop(this);
		bot = new ButtonSet();
		bot.clipBottom(this);
	}
	
	public TensorBox(int xpos, int ypos, int wide, int high, int numIn, int numOut)
	{
		super(xpos, ypos, wide, high);
		
		top = new ButtonSet();
		for(int i = 0; i < numIn; i++)
			top.add(new ButtonBox());
		top.clipTop(this);
		
		bot = new ButtonSet();
		for(int i = 0; i < numOut; i++)
			bot.add(new ButtonBox());
		bot.clipBottom(this);
	}
	
	public TensorBox(int xpos, int ypos, int wide, int high, int numIn, int numOut, Color baseColor)
	{
		super(xpos, ypos, wide, high);
		
		top = new ButtonSet();
		for(int i = 0; i < numIn; i++)
			top.add(new ButtonBox());
		top.clipTop(this);
		
		bot = new ButtonSet();
		for(int i = 0; i < numOut; i++)
			bot.add(new ButtonBox());
		bot.clipBottom(this);
		
		this.baseColor = baseColor;
		this.bordColor = colorShift(baseColor, 0.75f);
		this.lighColor = colorShift(baseColor, 3.75f);
	}
	
	@Override
	public Graphics draw(Graphics g)
	{
		super.draw(g);
		top.clipTop(this);
		bot.clipBottom(this);
		return g;
	}

	@Override
	public Collection<Box> getAll() 
	{
		ArrayList<Box> r = new ArrayList<Box>();
		r.add(this);
		r.addAll(top.getButtons());
		r.addAll(bot.getButtons());
		return r;
	}

	@Override
	public Drawable getTopButtons() { return top; }

	@Override
	public Drawable getBottomButtons() { return bot; }
	
	@Override
	public ButtonBox getTop(int index) { return top.getButtons().get(index); }

	@Override
	public ButtonBox getBottom(int index) { return top.getButtons().get(index); }

	@Override
	public String getName() { return getNode().name; }

	@Override
	public String getBrief() { return getNode().brief; }

	@Override
	public String getDescription() { return getNode().description; }

	@Override
	public SignalProfile getInputProfile() { return null; }

	@Override
	public SignalProfile getOutputProfile() { return null; }

	@Override
	public NodeWrapper getNode() { return null; }
}
