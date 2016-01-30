using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class GUI : Form
{
	public GUI() {}

	[STAThread]
	public static void Main()
	{
	  Application.EnableVisualStyles();
	  Application.Run(new GUI());
	}
}
