// Copyright � Conatus Creative, Inc. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE.md in the project root for license terms.

using TypeKitchen.StateMachine;

namespace TypeKitchen.Tests.StateMachine.States
{
    public class DerivedState : StateProvider.State
    {
        public virtual int Sprockets => 10;
        public virtual int Widgets => 5;
    }
}