using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlgGui
{
	// loosely follows singleton pattern?
	class Master
	{
		private static MainWindow win;
		private static int RepID = -1;
		
		public static void assignWindow(MainWindow window) { win = window; }

		public static void log(string message) { win.log(message); }
		public static void log(string message, Color color) { win.log(message, color); }

		public static Canvas getCanvas() { return win.getMainCanvas(); } // I know the name for this now!! Delegation!
		public static void setDragging(bool dragging, Representation dragRep) { win.setDragging(dragging, dragRep); }
		public static void setDraggingConnection(bool dragging, Connection con) { win.setDraggingConnection(dragging, con); }
		public static Connection getDraggingConnection() { return win.getDraggingConnection(); }
		public static void setCommandPrompt(string text) { win.setCommandPrompt(text); }

		public static int getNextRepID() { RepID++; return RepID; }
	}
}
