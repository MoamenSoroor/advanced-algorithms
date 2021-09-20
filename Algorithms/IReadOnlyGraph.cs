using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace AdvancedDSA
{
    public interface IReadOnlyGraph<TNode, TWeight>: IEnumerable<GraphNode<TNode, TWeight>>
    {
        int Count { get; }
        IReadOnlyList<TNode> Nodes { get; }

        Maybe<GraphNode<TNode, TWeight>> this[TNode node] { get; }
        Maybe<IReadOnlyList<(TNode Node, TWeight Weight)>> GetNeighbours(TNode item);
        Result<bool> IsBothConnected(TNode item, TNode item2);
        Result<bool> IsConnected(TNode item, TNode neighbour);
        Result<bool> IsConnected(TNode item, TNode neighbour, TWeight weight);
        Result<bool> IsSelfConnected(TNode item);
        Result<bool> IsSelfConnected(TNode item, TWeight weight);
        IReadOnlyList<TNode> ListAll();
    }
}