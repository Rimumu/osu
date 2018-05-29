﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.Multiplayer;
using osu.Game.Overlays.SearchableList;
using osu.Game.Screens.Multi.Components;

namespace osu.Game.Screens.Multi.Screens.Match
{
    public class Info : Container
    {
        public const float HEIGHT = 128;

        private readonly OsuSpriteText name, availabilityStatus;
        private readonly BeatmapTypeInfo beatmapTypeInfo;

        private OsuColour colours;

        public string Name
        {
            set { name.Text = value; }
        }

        private RoomAvailability availability;
        public RoomAvailability Availability
        {
            set
            {
                if (value == availability) return;
                availability = value;

                if (IsLoaded)
                    updateAvailabilityStatus();
            }
        }

        private RoomStatus status;
        public RoomStatus Status
        {
            set
            {
                if (value == status) return;
                status = value;

                if (IsLoaded)
                    updateAvailabilityStatus();
            }
        }

        public BeatmapInfo Beatmap
        {
            set { beatmapTypeInfo.Beatmap = value; }
        }

        public GameType Type
        {
            set { beatmapTypeInfo.Type = value; }
        }

        public Info()
        {
            RelativeSizeAxes = Axes.X;
            Height = HEIGHT;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = OsuColour.FromHex(@"28242d"),
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Horizontal = SearchableListOverlay.WIDTH_PADDING },
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Y,
                            AutoSizeAxes = Axes.X,
                            Padding = new MarginPadding { Vertical = 20 },
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Children = new Drawable[]
                                    {
                                        name = new OsuSpriteText
                                        {
                                            TextSize = 30,
                                        },
                                        availabilityStatus = new OsuSpriteText
                                        {
                                            TextSize = 14,
                                        },
                                    },
                                },
                                beatmapTypeInfo = new BeatmapTypeInfo
                                {
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                },
                            },
                        },
                        new ReadyButton
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.Y,
                            Width = 200,
                            Padding = new MarginPadding { Vertical = 10 },
                        },
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            this.colours = colours;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            updateAvailabilityStatus();
        }

        private void updateAvailabilityStatus()
        {
            if (status != null)
            {
                availabilityStatus.FadeColour(status.GetAppropriateColour(colours));
                availabilityStatus.Text = $"{availability.GetDescription()}, {status.Message}";
            }
        }

        private class ReadyButton : TriangleButton
        {
            public readonly Bindable<bool> Ready = new Bindable<bool>();

            protected override SpriteText CreateText() => new OsuSpriteText
            {
                Depth = -1,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Font = @"Exo2.0-Light",
                TextSize = 30,
            };

            public ReadyButton()
            {
                Height = 1;
                Text = "Ready";

                Action = () => Ready.Value = !Ready.Value;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                BackgroundColour = OsuColour.FromHex(@"1187aa");
                Triangles.ColourLight = OsuColour.FromHex(@"277b9c");
                Triangles.ColourDark = OsuColour.FromHex(@"1f6682");
                Triangles.TriangleScale = 1.5f;

                // todo: visually select
                Ready.ValueChanged += value =>
                {
                    if (value)
                        Text = "Not Ready";
                    else
                        Text = "Ready";
                };
            }
        }
    }
}
