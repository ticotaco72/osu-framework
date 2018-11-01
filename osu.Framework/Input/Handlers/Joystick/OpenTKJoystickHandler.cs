﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Framework.Input.StateChanges;
using osu.Framework.Logging;
using osu.Framework.MathUtils;
using osu.Framework.Platform;
using osu.Framework.Statistics;
using osu.Framework.Threading;
using osuTK.Input;
using JoystickState = osu.Framework.Input.States.JoystickState;

namespace osu.Framework.Input.Handlers.Joystick
{
    public class osuTKJoystickHandler : InputHandler
    {
        private ScheduledDelegate scheduledPoll;
        private ScheduledDelegate scheduledRefreshDevices;

        private int mostSeenDevices;

        private readonly List<JoystickDevice> devices = new List<JoystickDevice>();

        public override bool Initialize(GameHost host)
        {
            Enabled.BindValueChanged(enabled =>
            {
                if (enabled)
                {
                    host.InputThread.Scheduler.Add(scheduledRefreshDevices = new ScheduledDelegate(refreshDevices, 0, 500));

                    host.InputThread.Scheduler.Add(scheduledPoll = new ScheduledDelegate(delegate
                    {
                        foreach (var device in devices)
                        {
                            if (device.RawState.Equals(device.LastRawState))
                                continue;

                            var newState = new osuTKJoystickState(device);
                            handleState(device, newState);
                            FrameStatistics.Increment(StatisticsCounterType.JoystickEvents);
                        }
                    }, 0, 0));
                }
                else
                {
                    scheduledPoll?.Cancel();
                    scheduledRefreshDevices?.Cancel();

                    foreach (var device in devices)
                    {
                        if (device.LastState != null)
                            handleState(device, new JoystickState());
                    }

                    devices.Clear();
                    mostSeenDevices = 0;
                }
            }, true);

            return true;
        }

        private void handleState(JoystickDevice device, JoystickState newState)
        {
            PendingInputs.Enqueue(new JoystickButtonInput(newState.Buttons, device.LastState?.Buttons));

            device.LastState = newState;
        }

        private void refreshDevices()
        {
            // Update devices and add them to newDevices if still connected
            for (int i = 0; i < devices.Count; i++)
            {
                var dev = devices[i];

                dev.Refresh();

                if (!dev.RawState.IsConnected)
                {
                    mostSeenDevices--;
                    if (dev.LastState != null)
                        handleState(dev, new JoystickState());

                    devices.RemoveAt(i--);
                }
            }

            // Find any newly-connected devices
            while (true)
            {
                if (!osuTK.Input.Joystick.GetCapabilities(mostSeenDevices).IsConnected)
                    break;

                var newDevice = new JoystickDevice(mostSeenDevices);

                Logger.Log($"Connected joystick device: {newDevice.Guid}");

                devices.Add(newDevice);
                mostSeenDevices++;
            }
        }

        public override bool IsActive => true;
        public override int Priority => 0;

        private class osuTKJoystickState : JoystickState
        {
            public osuTKJoystickState(JoystickDevice device)
            {
                // Populate axes
                for (int i = 0; i < JoystickDevice.MAX_AXES; i++)
                {
                    var value = device.RawState.GetAxis(i);
                    if (!Precision.AlmostEquals(value, 0, device.DefaultDeadzones?[i] ?? Precision.FLOAT_EPSILON))
                        Axes.Add(new JoystickAxis(i, value));
                }

                // Populate normal buttons
                for (int i = 0; i < JoystickDevice.MAX_BUTTONS; i++)
                {
                    if (device.RawState.GetButton(i) == ButtonState.Pressed)
                        Buttons.SetPressed(JoystickButton.FirstButton + i, true);
                }

                // Populate hat buttons
                for (int i = 0; i < JoystickDevice.MAX_HATS; i++)
                {
                    foreach (var hatButton in getHatButtons(device, i))
                        Buttons.SetPressed(hatButton, true);
                }

                // Populate axis buttons (each axis has two buttons)
                foreach (var axis in Axes)
                    Buttons.SetPressed((axis.Value < 0 ? JoystickButton.FirstAxisNegative : JoystickButton.FirstAxisPositive) + axis.Axis, true);
            }

            private IEnumerable<JoystickButton> getHatButtons(JoystickDevice device, int hat)
            {
                var state = device.RawState.GetHat(JoystickHat.Hat0 + hat);

                if (state.IsUp)
                    yield return JoystickButton.FirstHatUp + hat;
                else if (state.IsDown)
                    yield return JoystickButton.FirstHatDown + hat;

                if (state.IsLeft)
                    yield return JoystickButton.FirstHatLeft + hat;
                else if (state.IsRight)
                    yield return JoystickButton.FirstHatRight + hat;
            }
        }

        private class JoystickDevice
        {
            /// <summary>
            /// Amount of axes supported by osuTK.
            /// </summary>
            public const int MAX_AXES = 64;

            /// <summary>
            /// Amount of buttons supported by osuTK.
            /// </summary>
            public const int MAX_BUTTONS = 64;

            /// <summary>
            /// Amount of hats supported by osuTK.
            /// </summary>
            public const int MAX_HATS = 4;

            /// <summary>
            /// Amount of movement around the "centre" of the axis that counts as moving within the deadzone.
            /// </summary>
            private const float deadzone_threshold = 0.05f;

            /// <summary>
            /// The last state of this <see cref="JoystickDevice"/>.
            /// This is updated with ever invocation of <see cref="Refresh"/>.
            /// </summary>
            public osuTK.Input.JoystickState? LastRawState { get; private set; }

            public JoystickState LastState { get; set; }

            /// <summary>
            /// The current state of this <see cref="JoystickDevice"/>.
            /// Use <see cref="Refresh"/> to update the state.
            /// </summary>
            public osuTK.Input.JoystickState RawState { get; private set; }

            private readonly Lazy<float[]> defaultDeadZones = new Lazy<float[]>(() => new float[MAX_AXES]);

            /// <summary>
            /// Default deadzones for each axis. Can be null if deadzones have not been found.
            /// </summary>
            public float[] DefaultDeadzones => defaultDeadZones.IsValueCreated ? defaultDeadZones.Value : null;

            /// <summary>
            /// The <see cref="Guid"/> for this <see cref="JoystickDevice"/>.
            /// </summary>
            public readonly Guid Guid;

            private readonly int deviceIndex;

            public JoystickDevice(int deviceIndex)
            {
                this.deviceIndex = deviceIndex;

                Guid = osuTK.Input.Joystick.GetGuid(deviceIndex);

                Refresh();
            }

            /// <summary>
            /// Refreshes the state of this <see cref="JoystickDevice"/>.
            /// </summary>
            public void Refresh()
            {
                LastRawState = RawState;
                RawState = osuTK.Input.Joystick.GetState(deviceIndex);

                if (!defaultDeadZones.IsValueCreated)
                {
                    for (int i = 0; i < MAX_AXES; i++)
                    {
                        var axisValue = Math.Abs(RawState.GetAxis(i));
                        if (Precision.AlmostEquals(0, axisValue))
                            continue;

                        defaultDeadZones.Value[i] = axisValue + deadzone_threshold;
                    }
                }
            }
        }
    }
}
