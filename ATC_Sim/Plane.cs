using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ATC_Sim;

public class Plane
{
    private Texture2D Texture { get; set; }

    public Vector2 Location { get; set; }
    
    private Vector2 lastLocation { get; set; }

    public float Speed { get; private set; } = 2f;
    
    private float Rotation { get; set; }
    private float Alpha { get; set; } = 0f;

    public Plane(int size, Vector2 position, GraphicsDevice graphicsDevice)
    {
        Texture = GraphicsHelper.CreateRegularPolygon(3, size, graphicsDevice, Color.LightGreen);
        Location = position;
        Rotation = 0f;
    }


    public void Update(float dt, bool passed = false)
    {
        Rotation = RotateToCenterScreen();

        if (passed)
        {
            Alpha = 1f;
            lastLocation = Location;
        }
        else
        {
            Alpha -= 0.25f * dt;
            if (Alpha < 0f)
                Alpha = 0f;
        }

        //move a little closer to center of screen
        Location += Vector2.Normalize(new Vector2(360, 360) - Location) * Speed * dt;
        var distFromCenter = Vector2.Distance(new Vector2(360, 360), Location);
        if (distFromCenter < 0.5f)
            Alpha = 0f;
    }


    private float RotateToCenterScreen()
    {
        var center = new Vector2(360, 360);
        var direction = center - Location;
        var angle = (float)System.Math.Atan2(direction.Y, direction.X);
        return angle - 180;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        var col = Color.White * Alpha;
        spriteBatch.Draw(
            Texture,
            lastLocation,
            null,
            col,
            MathHelper.ToRadians(Rotation),
            new Vector2(Texture.Width / 2, Texture.Height / 2),
            1f,
            SpriteEffects.None,
            0f);
    }
}