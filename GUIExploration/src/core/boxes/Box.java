package core.boxes;

import java.awt.event.MouseEvent;

import core.Drawable;

public interface Box extends Drawable
{
	boolean pointCheck(MouseEvent e);
	
	int getX();
	int getY();
	int getWidth();
	int getHeight();
	
	void move(int x, int y);
}
