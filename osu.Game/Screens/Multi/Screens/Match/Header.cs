﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Drawables;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.SearchableList;
using OpenTK.Graphics;

namespace osu.Game.Screens.Multi.Screens.Match
{
    public class Header : Container
    {
        public const float HEIGHT = 200;

        private readonly Box tabStrip;
        private readonly UpdateableBeatmapSetCover cover;

        public readonly OsuClickableContainer BeatmapButton;

        public BeatmapSetInfo BeatmapSet
        {
            set { cover.BeatmapSet = value; }
        }

        public Header()
        {
            RelativeSizeAxes = Axes.X;
            Height = HEIGHT;

            Children = new Drawable[]
            {
                // todo: gradient over the cover
                cover = new UpdateableBeatmapSetCover
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                },
                tabStrip = new Box
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Height = 1,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Horizontal = SearchableListOverlay.WIDTH_PADDING },
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.Y,
                            Width = 200,
                            Padding = new MarginPadding { Vertical = 5 },
                            Child = BeatmapButton = new BeatmapSelectButton
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                        },
                        new PageTabControl<MatchHeaderPage>
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            RelativeSizeAxes = Axes.X,
                        },
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            tabStrip.Colour = colours.Yellow;
        }

        private class BeatmapSelectButton : OsuClickableContainer
        {
            private const float corner_radius = 5;
            private const float bg_opacity = 0.5f;
            private const float transition_duration = 100;

            private readonly Box bg;
            private readonly Container border;

            public BeatmapSelectButton()
            {
                Masking = true;
                CornerRadius = corner_radius;

                Children = new Drawable[]
                {
                    bg = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                        Alpha = bg_opacity,
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = @"Exo2.0-Bold",
                        Text = "Select Beatmap",
                    },
                    border = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = corner_radius,
                        BorderThickness = 4,
                        Alpha = 0f,
                        Child = new Box // needs a child to show the border
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black.Opacity(0),
                        },
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                border.BorderColour = colours.Yellow;
            }

            protected override bool OnHover(InputState state)
            {
                border.FadeIn(transition_duration);
                return base.OnHover(state);
            }

            protected override void OnHoverLost(InputState state)
            {
                base.OnHoverLost(state);
                border.FadeOut(transition_duration);
            }

            protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
            {
                bg.FadeTo(0.75f, 1000, Easing.Out);
                return base.OnMouseDown(state, args);
            }

            protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
            {
                bg.FadeTo(bg_opacity, transition_duration);
                return base.OnMouseUp(state, args);
            }
        }
    }

    public enum MatchHeaderPage
    {
        Settings,
        Room,
    }
}
