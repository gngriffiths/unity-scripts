using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Threading;
using UnityEngine;

public class UniTaskExamples
{
    // UniTaskAsyncEnumberal (same as UniRx Observable) 
    int frameNumber = 1000;
    CancellationTokenSource cts = new CancellationTokenSource();

    void TakeOnEveryFrameNumber()
    {
        UniTaskAsyncEnumerable.IntervalFrame(frameNumber).TakeUntilCanceled(cts.Token).Subscribe(_ => Debug.Log("Take"));
    }

    void StopTakeOnEveryFrameNumber()
    {
        cts.Cancel();
        cts.Dispose();
    }

    // AsyncReactiveProperty (same as UnRx Reactive Properties)
    AsyncReactiveProperty<bool> reactivePropertyBool = new AsyncReactiveProperty<bool>(false);

    void SubscribeToReactiveProperty()
    {
        reactivePropertyBool.Where(_reactivePropertyBool => _reactivePropertyBool == true).Subscribe(_ => Debug.Log("Reactive Property is true"));
    }

    void ChangeReactiveProperty()
    {
        reactivePropertyBool.Value = !reactivePropertyBool.Value;
    }

}
