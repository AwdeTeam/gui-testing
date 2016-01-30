package core;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.util.ArrayList;

import javax.swing.BorderFactory;
import javax.swing.JPanel;

import core.boxes.Box;
import core.boxes.ButtonBox;
import core.boxes.DefaultBox;
import core.boxes.TensorBox;
import core.util.ButtonSet;
import core.util.Connection;

public class MainPanel extends JPanel
{
	private ArrayList<Box> boxes;
	private ArrayList<Drawable> others;
	
	private Box dragging = null;
	private Connection connecting = null;
	private ButtonBox open = null;
	private boolean shiftDown;
	private int[] selectBox;
	private ArrayList<DefaultBox> wrap;
	//private MainPanel thisref = this;
	
    public MainPanel() 
    {
        setBorder(BorderFactory.createLineBorder(Color.black));
        this.setBounds(0, 0, 400, 400);
        boxes = new ArrayList<Box>();
        others = new ArrayList<Drawable>();
        addBox(new TensorBox(64, 64, 102, 125, 2, 3));
        addBox(new TensorBox(64, 32, 12, 46, 4, 5, new Color(100, 20, 45)));
        
        this.addKeyListener(new KeyListener()
        {
			@Override
			public void keyTyped(KeyEvent e) {}
			
			@Override
			public void keyPressed(KeyEvent e) 
			{
				if(e.equals(KeyEvent.VK_SHIFT))
				{
					shiftDown = true;
					setCursor(new Cursor(Cursor.HAND_CURSOR));
					System.out.println(e);
				}
			}
			@Override
			public void keyReleased(KeyEvent e) 
			{
				if(e.equals(KeyEvent.VK_SHIFT))
				{
					shiftDown = false;
					setCursor(Cursor.getDefaultCursor());
					System.out.println(e);
				}
			}
        });
        
        this.addMouseListener(new MouseListener()
        {
			@Override
			public void mouseClicked(MouseEvent e){}

			@Override
			public void mousePressed(MouseEvent e) 
			{
				if(shiftDown)
				{
					selectBox = new int[]{e.getX(), e.getY(), e.getX(), e.getY()};
				}
				if(connecting == null)
				{
					//connecting = new Connection(e.getX(), e.getY(), e.getX(), e.getY());
					//others.add(connecting);
					for(Drawable d : others)
					{
						if(d instanceof ButtonSet)
						{
							for(ButtonBox b : ((ButtonSet) d).getButtons())
							{
								if(b.pointCheck(e))
								{
									connecting = new Connection(e);
									//b.connect(connecting, null);
									open = b;
								}
							}
						}
					}
					if(connecting != null)
						others.add(0, connecting);
				}
				
			}

			@Override
			public void mouseReleased(MouseEvent e) 
			{
				selectBox = null;
				if(dragging != null)
				{
					dragging = null;
				}
				if(connecting != null)
				{
					boolean kill = true;
					
					for(Drawable d : others)
					{
						if(d instanceof ButtonSet)
						{
							for(ButtonBox b : ((ButtonSet) d).getButtons())
							{
								if(b.pointCheck(e))
								{
									kill = false;
									((ButtonBox) b).connect(connecting, open);
									open.connect(connecting, (ButtonBox) b);
								}
							}
						}
					}
					
					if(kill)
					{
						others.remove(connecting);
						open.breakConnect();
						open = null;
					}
					connecting = null;
				}
			}

			@Override
			public void mouseEntered(MouseEvent e){}
			
			@Override
			public void mouseExited(MouseEvent e){}
        });
        
        this.addMouseMotionListener(new MouseMotionListener()
        {
			@Override
			public void mouseDragged(MouseEvent e) 
			{
				if(selectBox != null)
				{
					selectBox[3] = e.getX();
					selectBox[4] = e.getY();
				}
				if(connecting == null)
				{
					if(dragging == null)
					{
						for(Box b : boxes)
						{
							if(b.pointCheck(e))
							{
								b.move(e.getX(), e.getY());
								dragging = b;
								repaint();
							}
						}
					}
					else
					{
						dragging.move(e.getX(), e.getY());
						repaint();
					}
				}else
				{
					connecting.setEnd(new Dimension(e.getX(), e.getY()));
					repaint();
				}
			}

			@Override
			public void mouseMoved(MouseEvent e) 
			{
				for(Drawable d : others)
				{
					if(d instanceof ButtonSet)
					{
						for(ButtonBox b : ((ButtonSet) d).getButtons())
						{
							if(b instanceof ButtonBox && ((Box) b).pointCheck(e))
							{
								((ButtonBox) b).hover = true;
							}else if(b instanceof ButtonBox)
							{
								((ButtonBox) b).hover = false;
							}
						}
					}
				}
				for(Box b : boxes)
				{
					if(b instanceof DefaultBox)
					{
						if(b.pointCheck(e))
							((DefaultBox)b).selected = true;
						else
							((DefaultBox)b).selected = false;
					}
				}
				repaint();
				requestFocus();
			}
        });
    }

    private void addBox(DefaultBox box) 
    {
    	boxes.add(box);
        others.add(box.getTopButtons());
        others.add(box.getBottomButtons());
	}

	public Dimension getPreferredSize() 
    {
        return new Dimension(250,200);
    }

    public void paintComponent(Graphics g) 
    {
        super.paintComponent(g);       
        
        if(selectBox != null)
        {
        	g.setColor(Color.darkGray);
        	/*((Graphics2D)g).setStroke(new BasicStroke(1.0f, BasicStroke.CAP_BUTT, 
        			BasicStroke.JOIN_MITER, 10.0f, new float[]{10.0f}, 0.0f));*/
        	g.drawRect(selectBox[0], selectBox[1], 
        			Math.abs(selectBox[0] - selectBox[2]), Math.abs(selectBox[1] - selectBox[3]));
        }
        
        for(Drawable d : others)
        	d.draw(g);
        for(Box b : boxes)
        	b.draw(g);
        
        g.drawString("GUI Draw Panel",10,20);
    }  
}
