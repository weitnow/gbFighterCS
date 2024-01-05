using gbFighter.objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using System;

namespace gbFighter;



public class Game1 : Game
{


    private SpriteSheet _spriteSheet;
    private AnimatedSprite _idleAnimation;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    RenderTarget2D renderTarget;

    Texture2D rectangleTexture;
    Rectangle rectangle;
    Rectangle rectangle2;

    Fighter fighter;

    Texture2D circleTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        // Set the window size
        _graphics.PreferredBackBufferWidth = 1920;  // Set this value to the desired width
        _graphics.PreferredBackBufferHeight = 1080; // Set this value to the desired height
        _graphics.ApplyChanges();

        // Create the render target
        renderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        // Load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("assets/gbFighter.aseprite");

        // Create a 1x1 texture that will be scaled to the size of the rectangle
        rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White }); // Fill the texture with white

        // Define the rectangle
        rectangle = new Rectangle(200, 40, 32, 32); // x, y, width, height

        rectangle2 = new Rectangle(200, 200, 32, 32); // x, y, width, height

        circleTexture = CreateCircleTexture(32, Color.White); // Create a red circle with a radius of 50


        SpriteSheet _spriteSheet = SpriteSheetProcessor.Process(GraphicsDevice, aseFile);
        _idleAnimation = _spriteSheet.CreateAnimatedSprite("Idle");
        _idleAnimation.Speed = 0.3f;
        _idleAnimation.Scale = new Vector2(2, 2);
        _idleAnimation.Play();

        fighter = new Fighter(_spriteSheet, new Vector2(10, 10));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _idleAnimation.Update(gameTime);


        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        // Set the render target
        GraphicsDevice.SetRenderTarget(renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _idleAnimation.Draw(_spriteBatch, position: new Vector2(50, 50));
        _spriteBatch.Draw(rectangleTexture, rectangle, Color.Red); // Draw the rectangle
        DrawRectangleOutline(_spriteBatch, rectangleTexture, rectangle2, Color.Black, 1); // Draw the outline of the rectangle
        _spriteBatch.Draw(circleTexture, new Vector2(100, 100), Color.Blue); // Draw the circle at position (100, 100)


        _spriteBatch.End();


        // Reset the render target
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black); // Clear with desired screen color

        // Draw the scaled up render target
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);
        _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1920, 1080), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawRectangleOutline(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color, int thickness)
    {
        // Draw top line
        spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);

        // Draw bottom line
        spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);

        // Draw left line
        spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);

        // Draw right line
        spriteBatch.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
    }

    private Texture2D CreateCircleTexture(int radius, Color color)
    {
        int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
        Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

        Color[] data = new Color[outerRadius * outerRadius];

        // Colour the entire texture transparent first.
        for (int i = 0; i < data.Length; i++)
            data[i] = Color.Transparent;

        // Work out the minimum step necessary using trigonometry + sine approximation.
        double angleStep = 1f / radius;

        for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
        {
            int x = (int)Math.Round(radius + radius * Math.Cos(angle));
            int y = (int)Math.Round(radius + radius * Math.Sin(angle));

            data[y * outerRadius + x + 1] = color;
        }

        texture.SetData(data);
        return texture;
    }
}
