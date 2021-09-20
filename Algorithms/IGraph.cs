using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace AdvancedDSA
{
    public interface IGraph<TNode, TWeight> : IEnumerable<GraphNode<TNode, TWeight>>
    {
        Maybe<GraphNode<TNode, TWeight>> this[TNode node] { get; }

        int Count { get; }

        Result Add(TNode item);
        Result Connect(TNode item1, TNode item2, TWeight weight);
        Result ConnectBoth(TNode item1, TNode item2, TWeight weight);
        Result ConnectSelf(TNode item1, TWeight weight);
        bool Contains(TNode item);
        Result Disconnect(TNode item1, TNode item2);
        Result Disconnect(TNode item1, TNode item2, TWeight weight);
        Result DisconnectBoth(TNode item1, TNode item2);
        Result DisconnectBoth(TNode item1, TNode item2, TWeight weight);
        Result DisconnectSelf(TNode item1);
        Result DisconnectSelf(TNode item1, TWeight weight);
        Maybe<IReadOnlyList<(TNode Node, TWeight Weight)>> GetNeighbours(TNode item);
        Result<bool> IsBothConnected(TNode item, TNode item2);
        Result<bool> IsConnected(TNode item, TNode neighbour);
        Result<bool> IsConnected(TNode item, TNode neighbour, TWeight weight);
        Result<bool> IsSelfConnected(TNode item);
        Result<bool> IsSelfConnected(TNode item, TWeight weight);
        IReadOnlyList<TNode> ListAll();
        Result Remove(TNode item);
        void RemoveAll(Func<TNode, bool> predicate);
    }
}