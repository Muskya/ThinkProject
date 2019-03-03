﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Think
{
    abstract class GUIMenu
    {
        //Graphics
        protected Texture2D GUIMenuBackground;
        protected Vector2 GUIMenuBackgroundPosition;
        public string MenuName { get; set; }

        //SFX
        private SoundEffect GUIMenuOpeningSound, GUIMenuClosingSound, GUIMenuSlidingSound;
        //Instances déclarées en static car ce seront les mêmes pour toutes les instances de menus
        public static SoundEffectInstance GUIMenuOpeningInstance, GUIMenuClosingInstance, GUIMenuSlidingInstance; 
        public bool isOpened = false; //isOpened passe à true lorsque l'utilisatuer
        //clique sur le bouton d'ouverture du menu (cela peut être depuis le main menu, ou depuis ingame)

        //UI
        public Button closeBtn;
        public SpriteFont debugFont;

        public GUIMenu(string menuName)
        {
            this.MenuName = menuName;
        }

        //Load le content du menu de base. 
        public virtual void LoadContent(ContentManager Content)
        {
            //GUI Menu main background for almost any GUI Menu
            this.GUIMenuBackground = Content.Load<Texture2D>("Graphics/guimenu_background_darksand");
            //Centers the background on the screen
            GUIMenuBackgroundPosition = RandomManager.CenterElementOnScreen(GUIMenuBackground,
                ref GUIMenuBackgroundPosition);

            //Load les SFX de Gui Menus
            this.GUIMenuOpeningSound = Content.Load<SoundEffect>("SFX/gui_menu_opening_1");
            this.GUIMenuClosingSound = Content.Load<SoundEffect>("SFX/gui_menu_closing_1");
            this.GUIMenuSlidingSound = Content.Load<SoundEffect>("SFX/gui_menu_sliding_1");
            //Initialise les instances de SFX
            GUIMenuOpeningInstance = GUIMenuOpeningSound.CreateInstance();
            GUIMenuClosingInstance = GUIMenuClosingSound.CreateInstance();
            GUIMenuSlidingInstance = GUIMenuSlidingSound.CreateInstance();

            closeBtn = new Button("closebtn", new Vector2(
                ((this.GUIMenuBackground.Width - 200) + ((Main.screenWidth - this.GUIMenuBackground.Width)) / 2),
                ((this.GUIMenuBackground.Height - 60) + ((Main.screenHeight - this.GUIMenuBackground.Height)) / 2)),
                Content.Load<Texture2D>("Graphics/Buttons/small_closeBtnNormal"),
                Content.Load<Texture2D>("Graphics/Buttons/small_closeBtnPressed"),
                Content.Load<SoundEffect>("SFX/important_menu_clicksound"));

            debugFont = Content.Load<SpriteFont>("Other/ariaFont");
        }

        //A élaborer
        protected virtual void SwitchMenu()
        {

        }

        //Les méthodes de XNA (Load, Update, Draw etc) doivent toujours être en public. Si je veux les mettre
        //en protected parce-que j'ai une classe fille de GUIMenu, je pourrai pas dans une autre
        //classe utiliser cette méthode sur une instance de la classe mère ou fille.
        //Sachant que ça arrive tout le temps d'appeler les Update() et Draw() de l'instance
        //d'une classe depuis d'autres classes, je dois les laisser en public.
        public virtual void Update(GameTime gameTime)
        {
            if (isOpened)
            {
                closeBtn.Update(gameTime);       
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isOpened) //Si le menu est ouvert (après clic sur bouton correspondant)
            {
                if (!closeBtn.btnClicked) //Si on a pas cliqué sur le bouton de fermeture
                {
                    spriteBatch.Draw(GUIMenuBackground, GUIMenuBackgroundPosition, Color.White);
                    spriteBatch.DrawString(debugFont, this.MenuName, new Vector2(((Main.screenWidth / 2 - GUIMenuBackground.Width / 2) + 10), 115), Color.Black);
                    closeBtn.Draw(gameTime, spriteBatch);
                } else { //Si on a cliqué sur le bouton de fermeture, on ne draw plus rien (on ferme le menu)

                }
                
            }            
        }

        public virtual void DrawFade(GameTime gameTime, SpriteBatch spriteBatch, int r, int g, int b)
        {
            double delayBeforeFade = 0.010; // ~< 1seconde
            delayBeforeFade -= gameTime.ElapsedGameTime.TotalSeconds;

            if (delayBeforeFade <= 0)
            {
                spriteBatch.Draw(GUIMenuBackground, GUIMenuBackgroundPosition, new Color(r, g, b));
            }
        }
    }
}
