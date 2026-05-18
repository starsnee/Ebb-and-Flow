using Godot;
using System;

public partial class SacredItem : Area2D
{
    private PlayerCharacter player;
    
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        player = GetTree().GetFirstNodeInGroup("Player") as PlayerCharacter;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (GetOverlappingBodies().Contains(player))
        {
            player.ObtainSacredItem();
            QueueFree();
        }
    }
}
