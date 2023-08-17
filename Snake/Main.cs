using Godot;
using System;

public partial class Main : Node2D
{
	[Export]
	public int Speed{get; set;} = 200;
	
	private PackedScene AppleScene = GD.Load<PackedScene>("res://apple.tscn");
	
	private Texture2D dirt_tile;
	private Vector2 tile_position;
	
	private Vector2 ScreenSize;
	public int X;
	public int Y;
	
	Apple apple;

	public void MoveApple()
	{
		var random = new Random();
		Vector2 randPos = Vector2.Zero;
		bool t = true;
		while (t)
		{
			int a = random.Next(1, X);
			int b = random.Next(1, Y);
			randPos = new Vector2(a, b);
			if (!((head)FindChild("Head")).SnakeIsOn(randPos))
				t = false;
		}
		randPos = randPos*64;
		apple.Move(randPos);
		
	}
	
	
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		X = (int)Math.Floor(ScreenSize.X/64);
		Y = (int)Math.Floor(ScreenSize.Y/64);
		dirt_tile = (Texture2D)GD.Load("res://Art/Snake_DIrtTile.png");
		
		apple = (Apple)AppleScene.Instantiate();
		AddChild(apple);
		apple.AreaEntered += _eaten;
		apple.AreaEntered += ((head)GetNode("Head")).HasEaten;
		MoveApple();
	}
	
	private void _eaten(Area2D area)
	{
		CallDeferred("MoveApple");
	}
	
	public override void _Draw()
	{
		for (int x = 0; x<X; x++)
		{
			for (int y = 0; y<Y; y++)
			{
				tile_position = new Vector2(64*x, 64*y);
				DrawTexture(dirt_tile, tile_position);
			}
		}
	}
	private void _on_head_border_collision()
	{
		NewGame();
	}
	public void _head_collides_into_body(Area2D area)
	{
		NewGame();
	}
	 
	private void NewGame()
	{
		head Head = GetNode<head>("Head");
		
		Head.Restart();
	}
	
}








