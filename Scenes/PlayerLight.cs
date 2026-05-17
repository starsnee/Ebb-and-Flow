using Godot;
using System;

public partial class PlayerLight : PointLight2D
{
    public float LightDepthMin; //currently set to where you spawn
    [Export] public float LightDepthMax = 1000f; // Y position where light is dimmest (compared to spawn)
    [Export] public float LightEnergyMin = 0.1f; // dimmest value
    [Export] public float LightEnergyMax = 1.0f; // brightest value

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        LightDepthMin = GetParent<CharacterBody2D>().GlobalPosition.Y;
        LightDepthMax += GetParent<CharacterBody2D>().GlobalPosition.Y;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
        if (Input.IsActionJustPressed("ui_accept")) //ui_accept is the space bar
        {
            Enabled = !Enabled; //Enabled makes it turn on or off
        }

        float distancePercent = Mathf.InverseLerp(LightDepthMin, LightDepthMax, GetParent<CharacterBody2D>().GlobalPosition.Y);
        Energy = Mathf.Lerp(LightEnergyMax, LightEnergyMin, distancePercent);
    }
}
