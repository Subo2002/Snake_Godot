using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class head : Area2D
{
	public int Length = 1;
	
	[Signal]
	public delegate void BorderCollisionEventHandler();
	
	//Location of the Snake Head
	private Vector2 Cell = Vector2.Zero; 
	
	//Stores the locations of the Snake's "Cells", and then one more so it's ready for when the snake grows.
	private CircularBuffer<Vector2> Cells = new CircularBuffer<Vector2>(3);
	
	private int Speed;
	
	private Main main;
	
	//To convert player input in to direction of movement vector
	private Dictionary<String, Vector2> InputDirection = new Dictionary<String, Vector2>{
		{"North", new Vector2(0, -1)},
		{"South", new Vector2(0,  1)},
		{"West", new Vector2(-1, 0)},
		{"East", new Vector2(1, 0)}
	};
	
	
	
	public Vector2 ScreenSize;
	public Vector2 Direction = Vector2.Zero; //The next direction of travel
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = new Vector2(32, 32);
		ScreenSize = GetViewportRect().Size;
		main = GetParent() as Main;
		Speed = main.Speed;
	}

	
	//each cell is 64 by 64 pixels, epsilon keeps track of how close the head is to having moved to the next cell (i.e. like "discrete distance")
	double epsilon = 64;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Convert input
		foreach (String input in InputDirection.Keys){
			if (Input.IsActionPressed(input) && InputDirection[input] != -Direction)
				Direction = InputDirection[input];
		}
		/* 
		&& Direction == new Vector2(-1, 0)
		&& Direction == new Vector2(1, 0)
		&& Direction == new Vector2(0, -1)
		&& Direction == new Vector2(0, 1)
		*/
		if (Cell.X == -1 ||
			Cell.X == main.X ||
			Cell.Y == -1 ||
			Cell.Y == main.Y )
			{
				SetProcess(false);
				EmitSignal("BorderCollision");
			}
			
		//Cases or 1. travels over one cell, 2. travels less than 1 cell
		if (delta*Speed >= epsilon)
		{
			//Updates the locations of the snakes body cells
			
			Position = Cell*64;
			epsilon = 64 - (delta*Speed-epsilon);
			Cell += Direction;
			Cells.Add(Cell);
			GetTree().CallGroup("Body", "CellUpdate");
		}
		else
		{
			epsilon -= delta*Speed;
		}
		
		
		
	}
	private PackedScene BodyScene = GD.Load<PackedScene>("res://Body.tscn");
	public void HasEaten(Area2D area)
	{
		Length++;
		Cells.Grow();
		Body body = (Body) BodyScene.Instantiate();
		body.AreaEntered += main._head_collides_into_body;
		body.AreaEntered += STOP;
		GetTree().CallGroup("Body", "IndexUp");
		body.AddToGroup("Body");
		CallDeferred("HasEatenDeferred", body);
		
	}
	private void HasEatenDeferred(Body body){
		main.AddChild(body);
	}
	public Vector2 CellAt(int i){
		return Cells.Elements.ElementAt(i);
	}
	
	public bool SnakeIsOn(Vector2 Cell)
	{
		foreach (Vector2 cell in Cells.Elements)
		{
			if (Cell == cell)
				return true;
		}
		return false;
	}
	
	private void STOP(Area2D area){
		SetProcess(false);
	}
	
	public void Restart()
	{
		SetProcess(true);
		Length = 1;
		Direction = Vector2.Zero;
		Cell = Vector2.Zero;
		Position = Vector2.Zero;
		Cells = new CircularBuffer<Vector2>(3);
		foreach(Node node in GetTree().GetNodesInGroup("Body"))
		{
			node.QueueFree();
		}
	}
}








