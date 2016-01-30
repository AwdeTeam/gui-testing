package core;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import java.awt.BorderLayout;
import javax.swing.GroupLayout;
import javax.swing.GroupLayout.Alignment;
import java.awt.FlowLayout;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import java.awt.Component;
import javax.swing.Box;
import javax.swing.JLabel;
import java.awt.Font;
import java.awt.Color;
import javax.swing.border.LineBorder;
import javax.swing.LayoutStyle.ComponentPlacement;
import javax.swing.JScrollPane;
import java.awt.Rectangle;

public class TensorGui 
{

	private JFrame frame;
	private MainPanel mainPanel;
	private JMenuBar menuBar;
	private JMenu mnFile;
	private JMenu mnEdit;
	private JMenu mnInspect;
	private JMenu mnBuild;
	private JMenu mnHelp;
	private JPanel infoPanel;
	private JPanel controlPanel;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) 
	{
		EventQueue.invokeLater(new Runnable() 
		{
			public void run() 
			{
				try {
					TensorGui window = new TensorGui();
					window.frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the application.
	 */
	public TensorGui() 
	{
		initialize();
	}

	/**
	 * Initialize the contents of the frame.
	 */
	private void initialize() 
	{
		frame = new JFrame();
		frame.setBounds(100, 100, 450, 300);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setExtendedState(JFrame.MAXIMIZED_BOTH);
		frame.setLocationRelativeTo(null);
		
		JPanel rootPanel = new JPanel();
		
		JPanel toolPanel = new JPanel();
		toolPanel.setBorder(new LineBorder(new Color(0, 0, 0)));
		
		infoPanel = new JPanel();
		infoPanel.setBorder(new LineBorder(new Color(0, 0, 0)));
		
		controlPanel = new JPanel();
		
		JScrollPane scrollPanel = new JScrollPane();
		
		GroupLayout gl_rootPanel = new GroupLayout(rootPanel);
		gl_rootPanel.setHorizontalGroup(
			gl_rootPanel.createParallelGroup(Alignment.TRAILING)
				.addComponent(infoPanel, Alignment.LEADING, GroupLayout.DEFAULT_SIZE, 1868, Short.MAX_VALUE)
				.addGroup(gl_rootPanel.createSequentialGroup()
					.addGroup(gl_rootPanel.createParallelGroup(Alignment.LEADING, false)
						.addComponent(scrollPanel)
						.addComponent(toolPanel, GroupLayout.DEFAULT_SIZE, 1536, Short.MAX_VALUE))
					.addPreferredGap(ComponentPlacement.RELATED)
					.addComponent(controlPanel, GroupLayout.PREFERRED_SIZE, 298, GroupLayout.PREFERRED_SIZE)
					.addGap(27))
		);
		gl_rootPanel.setVerticalGroup(
			gl_rootPanel.createParallelGroup(Alignment.LEADING)
				.addGroup(gl_rootPanel.createSequentialGroup()
					.addGroup(gl_rootPanel.createParallelGroup(Alignment.LEADING)
						.addGroup(gl_rootPanel.createSequentialGroup()
							.addComponent(toolPanel, GroupLayout.PREFERRED_SIZE, 33, GroupLayout.PREFERRED_SIZE)
							.addPreferredGap(ComponentPlacement.RELATED)
							.addComponent(scrollPanel, GroupLayout.DEFAULT_SIZE, 936, Short.MAX_VALUE))
						.addComponent(controlPanel, GroupLayout.DEFAULT_SIZE, 976, Short.MAX_VALUE))
					.addPreferredGap(ComponentPlacement.RELATED)
					.addComponent(infoPanel, GroupLayout.PREFERRED_SIZE, 36, GroupLayout.PREFERRED_SIZE))
		);
		
		mainPanel = new MainPanel();
		mainPanel.setAutoscrolls(true);
		mainPanel.setBackground(Color.WHITE);
		scrollPanel.setColumnHeaderView(mainPanel);
		rootPanel.setLayout(gl_rootPanel);
		GroupLayout groupLayout = new GroupLayout(frame.getContentPane());
		groupLayout.setHorizontalGroup(
			groupLayout.createParallelGroup(Alignment.LEADING)
				.addGroup(groupLayout.createSequentialGroup()
					.addComponent(rootPanel, GroupLayout.PREFERRED_SIZE, 1842, GroupLayout.PREFERRED_SIZE)
					.addContainerGap(GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
		);
		groupLayout.setVerticalGroup(
			groupLayout.createParallelGroup(Alignment.LEADING)
				.addComponent(rootPanel, GroupLayout.DEFAULT_SIZE, 1019, Short.MAX_VALUE)
		);
		frame.getContentPane().setLayout(groupLayout);
		
		menuBar = new JMenuBar();
		frame.setJMenuBar(menuBar);
		
		mnFile = new JMenu("File");
		menuBar.add(mnFile);
		
		mnEdit = new JMenu("Edit");
		menuBar.add(mnEdit);
		
		mnInspect = new JMenu("Inspect");
		menuBar.add(mnInspect);
		
		mnBuild = new JMenu("Build");
		menuBar.add(mnBuild);
		
		mnHelp = new JMenu("Help");
		menuBar.add(mnHelp);
		
		Component horizontalStrut = Box.createHorizontalStrut(1521);
		menuBar.add(horizontalStrut);
		
		JLabel lblFixedBuildInfo = new JLabel("FIXED BUILD INFO");
		lblFixedBuildInfo.setFont(new Font("Calibri", Font.PLAIN, 16));
		menuBar.add(lblFixedBuildInfo);
	}
}
