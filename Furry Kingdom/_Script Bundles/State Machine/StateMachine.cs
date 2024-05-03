using System.Collections.Generic;
using System;

namespace Furry
{

    public class StateMachine
    {
        private IState _currentState;
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();
        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        /// <summary>
        /// Run the Tick method in the curent state.
        /// </summary>
        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                SetState(transition.To);
            }

            _currentState?.Tick();
        }

        /// <summary>
        /// Set the current state.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(IState state)
        {
            if (state == _currentState)
            {
                return;
            }

            _currentState?.OnExit();
            _currentState = state;
            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);

            if (_currentTransitions == null)
            {
                _currentTransitions = EmptyTransitions;
            }

            _currentState.OnEnter();
        }

        /// <summary>
        /// Add a state transition.
        /// </summary>
        /// <param name="from"></param> Transition from this state.
        /// <param name="to"></param> Transition to this state.
        /// <param name="predicate"></param> Condition to transition.
        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        /// <summary>
        /// Add a state that can be transitioned to from any other state.
        /// </summary>
        /// <param name="state"></param> The state
        /// <param name="predicate"></param> Condition to transition.
        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }


        /// <summary>
        /// A Transition state with a condition.
        /// </summary>
        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        /// <summary>
        /// Return the transition to another state if the conditions are met.
        /// </summary>
        /// <returns></returns>
        private Transition GetTransition()
        { 
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }
            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            return null;
        }
    }
}