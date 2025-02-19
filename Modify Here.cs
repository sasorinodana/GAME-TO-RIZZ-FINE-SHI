using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RizzerProgram
{
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //variabel karakter
        private Vector2 _characterPosition;
        private Texture2D[] _downFrames;
        private Texture2D[] _upFrames;
        private Texture2D[] _leftFrames;
        private Texture2D[] _rightFrames;
        //variabel npc
        private Texture2D bekgron;
        private Texture2D _npcTexture;
        private Vector2 _npcPosition;
        private Rectangle _npcBounds;
        //variabel dialog
        private Texture2D _dialogBoxTexture;
        private SpriteFont _dialogFont;
        private bool _showDialog = false;
        private string[] _dialogTexts = new string[10];
        private int _currentDialogIndex = 0;
        private Vector2 _dialogTextPosition;
        private bool _canAdvanceDialog = true; 
        private float _dialogCooldown = 0.3f; 
        private float _dialogTimer = 0f; 

        
        private float _speed = 150f;        
        private int _currentDownFrame = 0;
        private int _currentUpFrame = 0;
        private int _currentLeftFrame = 1;
        private int _currentRightFrame = 1;
        private float _frameTimer = 0f;       
        private float _frameDuration = 0.100f;   
        private string _currentDirection = "idle";
        private string _lastDirection = "down";
        private bool _showInteractText = false; 
        private const int _backgroundHeight = 480; 
        private const int _backgroundWidth = 800; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {           
            _characterPosition = new Vector2(100, 300);                                                        
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            _downFrames = new Texture2D[3];
            _downFrames[0] = Content.Load<Texture2D>("depan"); 
            _downFrames[1] = Content.Load<Texture2D>("depan1");
            _downFrames[2] = Content.Load<Texture2D>("depan2");

            _upFrames = new Texture2D[3];
            _upFrames[0] = Content.Load<Texture2D>("belakang");
            _upFrames[1] = Content.Load<Texture2D>("belakang1");
            _upFrames[2] = Content.Load<Texture2D>("belakang2");

            _leftFrames = new Texture2D[2];
            _leftFrames[0] = Content.Load<Texture2D>("kiri");
            _leftFrames[1] = Content.Load<Texture2D>("kiri1");

            _rightFrames = new Texture2D[2];
            _rightFrames[0] = Content.Load<Texture2D>("kanan");
            _rightFrames[1] = Content.Load<Texture2D>("kanan1");

            bekgron = Content.Load<Texture2D>("bekgron");

            _npcTexture = Content.Load<Texture2D>("npc");
            _npcPosition = new Vector2(500, 250);
            _npcBounds = new Rectangle((int)_npcPosition.X, (int)_npcPosition.Y,
                            128, 128);
            _dialogBoxTexture = Content.Load<Texture2D>("dialogbox");
            _dialogTexts[0] = "hi there";
            _dialogTexts[1] = "ure so cute";
            _dialogTexts[2] = "i wan to suck ur dick";
            //add others below here..just copy the exact statement and change the number        
            _dialogTextPosition = new Vector2(130, 330); 
            _dialogFont = Content.Load<SpriteFont>("dialogfont"); 
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();
            bool isMoving = false;
            Vector2 newPosition = _characterPosition;

            if (keyboardState.IsKeyDown(Keys.S))
            {
                newPosition.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentDirection = "down";
                _lastDirection = "down";
                isMoving = true;
            }
            else if (keyboardState.IsKeyDown(Keys.W))
            {
                newPosition.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentDirection = "up";
                _lastDirection = "up";
                isMoving = true;
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                newPosition.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentDirection = "left";
                _lastDirection = "left";
                isMoving = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                newPosition.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentDirection = "right";
                _lastDirection = "right";
                isMoving = true;
            }
            else
            {
                _currentDirection = "idle";
            }

            Rectangle characterBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, 64, 64); // Assuming character size is 64x64
                                                                                                       // Check for collision with NPC         

            if (characterBounds.Intersects(_npcBounds) && !_showDialog)
            {
                _showDialog = true; 
                _currentDialogIndex = 0; 
                _canAdvanceDialog = true; 
            }

            if (_npcBounds.Intersects(characterBounds))
            {
                _showInteractText = true; 
            }
            else
            {
                _showInteractText = false; 
            }

            if (isMoving)
            {
                _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_frameTimer >= _frameDuration)
                {
                    _frameTimer = 0f;
                    if (_currentDirection == "down")
                        _currentDownFrame = (_currentDownFrame + 1) % 3;
                    else if (_currentDirection == "up")
                        _currentUpFrame = (_currentUpFrame + 1) % 3;
                    else if (_currentDirection == "left")
                        _currentLeftFrame = (_currentLeftFrame + 1) % 2;
                    else if (_currentDirection == "right")
                        _currentRightFrame = (_currentRightFrame + 1) % 2;
                }
            }
            else
            {              
                _currentDownFrame = 0;
                _currentUpFrame = 0;
                _currentLeftFrame = 1;
                _currentRightFrame = 1;
            }

            if (newPosition.Y < 240)
                newPosition.Y = 240;


            else if (newPosition.Y > 480 - 128)
                newPosition.Y = 480 - 128;


            if (newPosition.X < 0)
                newPosition.X = 0;


            else if (newPosition.X > _backgroundWidth - 128)
             newPosition.X = _backgroundWidth - 128;
            _characterPosition = newPosition;

            MouseState mouseState = Mouse.GetState();
            IsMouseVisible = true; 
            Point mousePosition = mouseState.Position; 

            if (!_canAdvanceDialog)
            {
                _dialogTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_dialogTimer >= _dialogCooldown)
                {
                    _canAdvanceDialog = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                
                if (_npcBounds.Contains(mousePosition))
                {
                    _showDialog = true;
                    _currentDialogIndex = 0;
                    _canAdvanceDialog = true;
                }
                else if (_showDialog && _canAdvanceDialog)
                {
                    
                    Rectangle dialogBounds = new Rectangle(100, 300, _dialogBoxTexture.Width, _dialogBoxTexture.Height);

                    
                    if (dialogBounds.Contains(mousePosition))
                    {
                        
                        _currentDialogIndex++;
                        if (_currentDialogIndex >= _dialogTexts.Length)
                        {
                            _showDialog = false; 
                            _currentDialogIndex = 0; 
                        }
                        _canAdvanceDialog = false;
                        _dialogTimer = 0f;
                    }
                    else
                    {
                        _showDialog = false;
                    }

                    base.Update(gameTime);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(bekgron, new Rectangle(0, 0, 800, 480), Color.White);
            _spriteBatch.Draw(_npcTexture, _npcPosition, Color.White);

            Texture2D currentFrameTexture = null;
           
            if (_currentDirection == "down")
                currentFrameTexture = _downFrames[_currentDownFrame];
            else if (_currentDirection == "up")
                currentFrameTexture = _upFrames[_currentUpFrame];
            else if (_currentDirection == "left")
                currentFrameTexture = _leftFrames[_currentLeftFrame];
            else if (_currentDirection == "right")
                currentFrameTexture = _rightFrames[_currentRightFrame];
            else
            {
                
                if (_lastDirection == "down")
                    currentFrameTexture = _downFrames[0];
                else if (_lastDirection == "up")
                    currentFrameTexture = _upFrames[0];
                else if (_lastDirection == "left")
                    currentFrameTexture = _leftFrames[0];
                else if (_lastDirection == "right")
                    currentFrameTexture = _rightFrames[0];
            }
           
            if (currentFrameTexture != null)
            {
                _spriteBatch.Draw(currentFrameTexture, _characterPosition, Color.White);
            }

            if (_showDialog)
            {
                _spriteBatch.Draw(_dialogBoxTexture, new Vector2(100, 300), Color.White); 
                _spriteBatch.DrawString(_dialogFont, _dialogTexts[_currentDialogIndex], _dialogTextPosition, Color.Black);
            }

            if (_showInteractText)
            {
                Vector2 interactTextPosition = new Vector2(510, 230); 
                _spriteBatch.DrawString(_dialogFont, "Talk to ....", interactTextPosition, Color.Black); //modify to your fyne shi name
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
