﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Containers;
using osuTK.Input;
using osuTK;
using osu.Framework.Input.Events;

namespace osu.Framework.Graphics.UserInterface
{
    public abstract class SliderBar<T> : Container, IHasCurrentValue<T>
        where T : struct, IComparable, IConvertible
    {
        /// <summary>
        /// Range padding reduces the range of movement a slider bar is allowed to have
        /// while still receiving input in the padded region. This behavior is necessary
        /// for finite-sized nubs and can not be achieved (currently) by existing
        /// scene graph padding / margin functionality.
        /// </summary>
        public float RangePadding;

        public float UsableWidth => DrawWidth - 2 * RangePadding;

        /// <summary>
        /// A custom step value for each key press which actuates a change on this control.
        /// </summary>
        public float KeyboardStep;

        protected readonly BindableNumber<T> CurrentNumber;

        public Bindable<T> Current => CurrentNumber;

        protected SliderBar()
        {
            if (typeof(T) == typeof(int))
                CurrentNumber = new BindableInt() as BindableNumber<T>;
            else if (typeof(T) == typeof(long))
                CurrentNumber = new BindableLong() as BindableNumber<T>;
            else if (typeof(T) == typeof(double))
                CurrentNumber = new BindableDouble() as BindableNumber<T>;
            else if (typeof(T) == typeof(float))
                CurrentNumber = new BindableFloat() as BindableNumber<T>;

            if (CurrentNumber == null)
                throw new NotSupportedException($"We don't support the generic type of {nameof(BindableNumber<T>)}.");
        }

        protected float NormalizedValue
        {
            get
            {
                if (Current == null)
                    return 0;

                if (!CurrentNumber.HasDefinedRange)
                    throw new InvalidOperationException($"A {nameof(SliderBar<T>)}'s {nameof(Current)} must have user-defined {nameof(BindableNumber<T>.MinValue)}"
                                                        + $" and {nameof(BindableNumber<T>.MaxValue)} to produce a valid {nameof(NormalizedValue)}.");

                var min = Convert.ToSingle(CurrentNumber.MinValue);
                var max = Convert.ToSingle(CurrentNumber.MaxValue);

                if (max - min == 0)
                    return 1;

                var val = Convert.ToSingle(CurrentNumber.Value);
                return (val - min) / (max - min);
            }
        }

        /// <summary>
        /// Triggered when the <see cref="Current"/> value has changed. Used to update the displayed value.
        /// </summary>
        /// <param name="value">The normalized <see cref="Current"/> value.</param>
        protected abstract void UpdateValue(float value);

        protected override void LoadComplete()
        {
            base.LoadComplete();

            CurrentNumber.ValueChanged += _ => UpdateValue(NormalizedValue);
            CurrentNumber.MinValueChanged += _ => UpdateValue(NormalizedValue);
            CurrentNumber.MaxValueChanged += _ => UpdateValue(NormalizedValue);

            UpdateValue(NormalizedValue);
        }

        protected override bool OnClick(ClickEvent e)
        {
            handleMouseInput(e);
            return true;
        }

        protected override bool OnDrag(DragEvent e)
        {
            handleMouseInput(e);
            return true;
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            Vector2 posDiff = e.MouseDownPosition - e.MousePosition;

            return Math.Abs(posDiff.X) > Math.Abs(posDiff.Y);
        }

        protected override bool OnDragEnd(DragEndEvent e) => true;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!IsHovered || CurrentNumber.Disabled)
                return false;

            var step = KeyboardStep != 0 ? KeyboardStep : (Convert.ToSingle(CurrentNumber.MaxValue) - Convert.ToSingle(CurrentNumber.MinValue)) / 20;
            if (CurrentNumber.IsInteger) step = (float)Math.Ceiling(step);

            switch (e.Key)
            {
                case Key.Right:
                    CurrentNumber.Add(step);
                    OnUserChange();
                    return true;
                case Key.Left:
                    CurrentNumber.Add(-step);
                    OnUserChange();
                    return true;
                default:
                    return false;
            }
        }

        private void handleMouseInput(UIEvent e)
        {
            var xPosition = ToLocalSpace(e.ScreenSpaceMousePosition).X - RangePadding;

            if (!CurrentNumber.Disabled)
                CurrentNumber.SetProportional(xPosition / UsableWidth, e.ShiftPressed ? KeyboardStep : 0);

            OnUserChange();
        }

        /// <summary>
        /// Triggered when the value is changed based on end-user input to this control.
        /// </summary>
        protected virtual void OnUserChange() { }
    }
}
