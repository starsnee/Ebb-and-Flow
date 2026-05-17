using Godot;
using System;


//Echo note - this file has a lot of commented out code, that's default code kept for reference only


public partial class PlayerCharacter : CharacterBody2D
{
    //Tweakables
    [Export] public float AccelerationForce = 25.0f;
    [Export] public float DragForce = 0.05f;
    [Export] public float MaxSpeed = 500f;
    [Export] public float MaxHealth = 100f;
    [Export] public float DeathFloatSpeed = 200f;


    private float currentHealth;
    private bool isAlive = true;




    //The OnStart
    public override void _Ready()
    {
        currentHealth = MaxHealth;
    }




    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 acceleration;


        // Add the gravity.
        //if (!IsOnFloor())
        //{
        //	velocity += GetGravity() * (float)delta;
        //}


        // Handle Jump.
        //if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        //{
        //	velocity.Y = JumpVelocity;
        //}


        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        //Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        //if (direction != Vector2.Zero)
        //{
        //	velocity.X = direction.X * Speed;
        //}
        //else
        //{
        //	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        //}






        //Echo Code begins


        if (!isAlive)
        {
            // Ignore all input, just float up
            velocity = velocity.Lerp(Vector2.Zero, DragForce);
            acceleration = Vector2.Zero;
            acceleration.Y = DeathFloatSpeed * DragForce;
            velocity.Y -= DeathFloatSpeed * (float)delta;
            Velocity = velocity;
            MoveAndSlide();
            return;
        }
        else
        {
            //First, handle user input
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");


            if (direction != Vector2.Zero)
            {
                acceleration.X = direction.X * AccelerationForce;
                acceleration.Y = direction.Y * AccelerationForce;
            }
            else
            {
                acceleration = Vector2.Zero;
            }


            //Turn Acceleration Into Velocity


            velocity += acceleration;


            //And now for some drag
            velocity = velocity.Lerp(Vector2.Zero, DragForce);


            //clamping the speed
            velocity = velocity.LimitLength(MaxSpeed);


            //and then this is Godot's built in ending, I can tweak it later if you want
            Velocity = velocity;
            MoveAndSlide();
        }



        if (Input.IsKeyPressed(Key.P))
        {
            TakeDamage(20f);
        }
        GD.Print(currentHealth);
    }


    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            BeginDeath();
        }
    }


    private void BeginDeath()
    {
        //Sprite Change Should Occur
        isAlive = false;
        Die(); //thisll be invoked
    }


    private void Die()
    {
        //Reckon this is where a restart screen would go initialized
    }
}

