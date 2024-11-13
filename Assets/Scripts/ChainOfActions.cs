using System.Collections;
using System.Collections.Generic;

public class ChainOfActions
{
    private readonly IEnumerable _actions;

    public ChainOfActions(IEnumerable<IUnitAction> actions)
    {
        _actions = actions;
    }

    public IEnumerable GetActions()
    {
        return _actions;
    }
}
