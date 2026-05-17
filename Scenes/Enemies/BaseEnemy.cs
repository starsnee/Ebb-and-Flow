using Godot;
using System;
using System.Threading.Tasks;

public partial class BaseEnemy : CharacterBody2D
{
    public enum State { Idle, Attacking, Dead }
    protected State currentState = State.Idle;
    private PlayerCharacter player;
    private Vector2 spawnPos;
    private Vector2 currentPos;
    private float distanceToPlayer;
    private Vector2 idleDestination;
    private bool canAttack = true;

    [Export] float RangeToBeginAttacking = 500f;
    [Export] float DisengageDistance = 700f;
    [Export] float AttackRange = 50f;
    [Export] float AttackDamage = 20f;
    [Export] float Speed = 100f;
    [Export] float IdlePointRadius = 100f;
    [Export] int AttackDelay = 3000;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        player = GetTree().GetFirstNodeInGroup("Player") as PlayerCharacter;
        spawnPos = GlobalPosition;
        GetNewIdleSpot();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        currentPos = GlobalPosition;
        if (player != null)
        {
            distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
        }

        switch (currentState)
        {
            case State.Idle:
                HandleIdle(delta);
                break;
            case State.Attacking:
                HandleAttacking(delta);
                break;
            case State.Dead:
                HandleDead(delta);
                break;
        }
    }

    protected virtual void HandleIdle(double delta)
    {
        // default idle behavior
        if (idleDestination != null)
        {
            //Moving to destination
            Vector2 direction = GlobalPosition.DirectionTo(idleDestination);
            Velocity = direction * Speed;

            if (GlobalPosition.DistanceTo(idleDestination) < 1.0f) //Stop moving if close enough
            {
                Velocity = Vector2.Zero;
                TryForNewIdleDestination();
            }

            MoveAndSlide();
        }
        else
        {
            GetNewIdleSpot();
        }

        //how to switch to attacking
        if (distanceToPlayer < RangeToBeginAttacking)
        {
            currentState = State.Attacking;
        }
    }
    protected virtual void HandleAttacking(double delta)
    {
        Vector2 playerPos = player.GlobalPosition;
        // default attack behavior
        if (canAttack)
        {
            Vector2 direction = GlobalPosition.DirectionTo(playerPos);
            Velocity = direction * Speed;
            if (GlobalPosition.DistanceTo(playerPos) < AttackRange) //Stop moving if close enough
            {
                Velocity = Vector2.Zero;
                Attack();
            }
        }

        MoveAndSlide();

        //how to go back to idle
        if (distanceToPlayer > DisengageDistance)
        {
            currentState = State.Idle;
        }
    }
    protected virtual void HandleDead(double delta)
    {

    }

    protected virtual void GetNewIdleSpot()
    {
        float angle = GD.Randf() * Mathf.Tau;
        float distance = Mathf.Sqrt(GD.Randf()) * IdlePointRadius;

        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        idleDestination = (spawnPos + (direction * distance));
    }

    protected virtual async Task TryForNewIdleDestination()
    {
        await Task.Delay(2000);
        if (GlobalPosition.DistanceTo(idleDestination) < 1.0f)
        {
            GetNewIdleSpot();
        }
    } //Method use: Once an enemy Reaches their idle spot, they stay there for a bit, and then move on. How I'm handling that is to run the check if it's at it's position twice

    protected virtual async Task Attack()
    {
        canAttack = false;
        player.TakeDamage(AttackDamage);
        await Task.Delay(AttackDelay);
        canAttack = true;
    }
}
