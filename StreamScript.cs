using Godot;
using System;

public partial class StreamScript : Area2D
{
	[Export] public float StreamStrength = 200f;
	[Export] public Vector2 StreamDirection = Vector2.Right;

	private PlayerCharacter player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        player = GetTree().GetFirstNodeInGroup("Player") as PlayerCharacter;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (GetOverlappingBodies().Contains(player))
		{
            player.Velocity += StreamDirection * StreamStrength * (float)delta;
        }
	}
}
