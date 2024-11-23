using System.Collections.Generic;

public class ChainOfActions
{
    private readonly List<IUnitAction> _actions;

    public ChainOfActions(List<IUnitAction> actions)
    {
        _actions = actions;
    }

    public int Count => _actions.Count;

    public List<IUnitAction> GetActions()
    {
        return _actions;
    }
}
