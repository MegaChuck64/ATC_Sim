using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ATC_Sim;

public static class GraphicsHelper
{
    public static Texture2D CreateCircle(int radius, GraphicsDevice graphicsDevice, Color? color = null)
    {
        int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
        var texture = new Texture2D(graphicsDevice, outerRadius, outerRadius);
        var tint = color ?? Color.White;
        var data = new Color[outerRadius * outerRadius];
        for (int x = 0; x < outerRadius; x++ )
        {
            for (int y = 0; y < outerRadius; y++)
            {
                int index = x * outerRadius + y;
                Vector2 pos = new (x - radius, y - radius);
                if (pos.Length() <= radius)
                    data[index] = tint;
                else
                    data[index] = Color.Transparent;
            }
        }
        texture.SetData(data);
        return texture;
    }

    public static Texture2D CreateRect(int width, int height, GraphicsDevice graphicsDevice, Color? color = null)
    {
        var texture = new Texture2D(graphicsDevice, width, height);

        var tint = color ?? Color.White;
        
        var data = new Color[width * height];
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                int index = x * width + y;
                data[index] = tint;
            }
        }
        texture.SetData(data);
        return texture;
    }

    public static Texture2D CreateRegularPolygon(int sides, float radius, GraphicsDevice graphicsDevice, Color? color = null)
    {
        var texture = new Texture2D(graphicsDevice, (int)radius * 2, (int)radius * 2);
        var tint = color ?? Color.White;
        var data = CreateRegularPolygonColorArray(sides, radius, tint);
        texture.SetData(data);
        return texture;
    }


    private static Color[] CreateRegularPolygonColorArray(int sides, float radius, Color color)
    {
        // Calculate the angle between each vertex of the polygon
        float angle = MathHelper.TwoPi / sides;

        // Calculate the size of the color array based on the radius
        int width = (int)(radius * 2);
        int height = (int)(radius * 2);
        int size = width * height;

        // Create the color array
        Color[] colors = new Color[size];

        // Calculate the center of the color array
        Vector2 center = new Vector2(width / 2, height / 2);

        // Calculate the position of the vertices based on the radius
        Vector2[] vertices = new Vector2[sides];
        for (int i = 0; i < sides; i++)
        {
            float x = center.X + radius * (float)Math.Cos(i * angle);
            float y = center.Y + radius * (float)Math.Sin(i * angle);
            vertices[i] = new Vector2(x, y);
        }

        // Plot pixels to the color array
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = y * width + x;

                // Check if the pixel is inside the polygon
                if (IsInsidePolygon(new Vector2(x, y), vertices))
                {
                    colors[index] = color;
                }
                else
                {
                    colors[index] = Color.Transparent;
                }
            }
        }

        return colors;
    }

    // Helper method to check if a point is inside a polygon
    private static bool IsInsidePolygon(Vector2 point, Vector2[] vertices)
    {
        int count = vertices.Length;
        bool inside = false;

        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            if (((vertices[i].Y > point.Y) != (vertices[j].Y > point.Y)) &&
                (point.X < (vertices[j].X - vertices[i].X) * (point.Y - vertices[i].Y) / (vertices[j].Y - vertices[i].Y) + vertices[i].X))
            {
                inside = !inside;
            }
        }

        return inside;
    }

}