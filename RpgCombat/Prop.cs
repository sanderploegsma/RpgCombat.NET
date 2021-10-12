using System;
using System.Numerics;

namespace RpgCombat
{
    public enum PropStatus
    {
        Normal,
        Destroyed,
    }

    public class Prop : ITarget
    {
        private PropState _currentState;

        public Prop(double health, Vector2 position = default)
        {
            Health = health;
            Position = position;
            TransitionToState(new NormalState());
        }

        public double Health { get; private set; }
        public PropStatus Status { get; private set; }
        public Vector2 Position { get; }
        
        void ITarget.ReceiveDamage(Damage damage) => _currentState.ReceiveDamage(this, damage);

        private void TransitionToState(PropState state)
        {
            _currentState = state;
            _currentState.ActivateState(this);
        }

        private abstract class PropState
        {
            public abstract void ActivateState(Prop prop);

            public abstract void ReceiveDamage(Prop prop, Damage damage);
        }

        private class NormalState : PropState
        {
            public override void ActivateState(Prop prop)
            {
                if (prop.Health > 0)
                {
                    prop.Status = PropStatus.Normal;
                }
                else
                {
                    prop.TransitionToState(new DestroyedState());
                }
            }

            public override void ReceiveDamage(Prop prop, Damage damage)
            {
                prop.Health -= Math.Min(prop.Health, damage.Amount);

                if (prop.Health <= 0)
                {
                    prop.TransitionToState(new DestroyedState());
                }
            }
        }

        private class DestroyedState : PropState
        {
            public override void ActivateState(Prop prop)
            {
                prop.Health = 0;
                prop.Status = PropStatus.Destroyed;
            }

            public override void ReceiveDamage(Prop prop, Damage damage)
            {
                throw new InvalidOperationException("Prop is destroyed");
            }
        }
    }
}