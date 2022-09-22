﻿using Avalonia;
using Avalonia.Controls;

namespace Mapsui.Interactivity.UI.Avalonia
{
    public class Interaction
    {
        static Interaction()
        {
            BehaviorProperty.Changed.Subscribe(BehaviorChanged);
        }

        public static readonly AttachedProperty<InteractivityBehavior?> BehaviorProperty =
            AvaloniaProperty.RegisterAttached<Interaction, IAvaloniaObject, InteractivityBehavior?>("Behavior");

        public static InteractivityBehavior? GetBehavior(IAvaloniaObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var behavior = (InteractivityBehavior?)obj.GetValue(BehaviorProperty);
            if (behavior is null)
            {
                behavior = new InteractivityBehavior();
                obj.SetValue(BehaviorProperty, behavior);
                SetVisualTreeEventHandlersInitial(obj);
            }

            return behavior;
        }

        public static void SetBehavior(IAvaloniaObject obj, InteractivityBehavior? value)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.SetValue(BehaviorProperty, value);
        }

        private static void BehaviorChanged(AvaloniaPropertyChangedEventArgs<InteractivityBehavior?> e)
        {
            var oldBehavior = e.OldValue.GetValueOrDefault();
            var newBehavior = e.NewValue.GetValueOrDefault();

            if (oldBehavior == newBehavior)
            {
                return;
            }

            if (oldBehavior is { AssociatedObject: { } })
            {
                oldBehavior.Detach();
            }

            if (newBehavior is { })
            {
                newBehavior.Attach(e.Sender);
                SetVisualTreeEventHandlersRuntime(e.Sender);
            }
        }

        private static void SetVisualTreeEventHandlersInitial(IAvaloniaObject obj)
        {
            if (obj is not Control control)
            {
                return;
            }

            control.AttachedToVisualTree -= Control_AttachedToVisualTreeRuntime;
            control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeRuntime;

            control.AttachedToVisualTree -= Control_AttachedToVisualTreeInitial;
            control.AttachedToVisualTree += Control_AttachedToVisualTreeInitial;

            control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeInitial;
            control.DetachedFromVisualTree += Control_DetachedFromVisualTreeInitial;
        }

        private static void SetVisualTreeEventHandlersRuntime(IAvaloniaObject obj)
        {
            if (obj is not Control control)
            {
                return;
            }

            control.AttachedToVisualTree -= Control_AttachedToVisualTreeInitial;
            control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeInitial;

            control.AttachedToVisualTree -= Control_AttachedToVisualTreeRuntime;
            control.AttachedToVisualTree += Control_AttachedToVisualTreeRuntime;

            control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeRuntime;
            control.DetachedFromVisualTree += Control_DetachedFromVisualTreeRuntime;
        }

        private static void Control_AttachedToVisualTreeInitial(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is IAvaloniaObject d)
            {
                GetBehavior(d)?.Attach(d);
                GetBehavior(d)?.AttachedToVisualTree();
            }
        }

        private static void Control_DetachedFromVisualTreeInitial(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is IAvaloniaObject d)
            {
                GetBehavior(d)?.DetachedFromVisualTree();
                GetBehavior(d)?.Detach();
            }
        }

        private static void Control_AttachedToVisualTreeRuntime(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is IAvaloniaObject d)
            {
                GetBehavior(d)?.AttachedToVisualTree();
            }
        }

        private static void Control_DetachedFromVisualTreeRuntime(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is IAvaloniaObject d)
            {
                GetBehavior(d)?.DetachedFromVisualTree();
            }
        }
    }
}
