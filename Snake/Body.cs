using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Body : Area2D
{
	private PackedScene headScene = GD.Load<PackedScene>("res://head.tscn");
	private Vector2 Cell;
	private int index;
	private head Head;
	private int Length;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node main = GetParent();
		Head = (head)main.FindChild("Head");
		Length = Head.Length;
		index = 0;
		Cell = Head.CellAt(index);
		Position = Cell*64;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void CellUpdate(){
		Cell = Head.CellAt(index+1);
		Position = Cell*64;
	}
	public void IndexUp(){
		index++;
	}
}



