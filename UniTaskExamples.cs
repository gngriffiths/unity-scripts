using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Threading;
using UnityEngine;

public class UniTaskExamples
{

    // AsyncReactiveProperty (same as UnRx Reactive Properties)
    AsyncReactiveProperty<bool> reactivePropertyBoolRx = new AsyncReactiveProperty<bool>(false);

    void SubscribeToReactiveProperty()
    {
        reactivePropertyBoolRx.Where(_boolRx => _boolRx == true).Subscribe(_ => Debug.Log("Reactive Property is true"));
    }

    void ChangeReactiveProperty()
    {
        reactivePropertyBoolRx.Value = !reactivePropertyBoolRx.Value;
    }
	
    #region Periodic
    // Do an action every X time.
    CancellationTokenSource cts = new CancellationTokenSource();

    void StartPeriodTask()
    {
            cts = new CancellationTokenSource();
            PeriodicTask(1f, cts.Token).Forget();        
    }

    void CancelPeriodTask()
    {
        cts.Cancel();
        cts.Dispose();
    }

    private async UniTaskVoid PeriodicTask(float seconds, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: cancellationToken);

            // Call your method here.
            Debug.Log("Call method here");
        }
    }
    #endregion

	#region EveryFrames	
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
	
	
    void StartEveryFrames()    
    {
        CancellationTokenSource cts = new CancellationTokenSource();        // Create a new CTS
        IntervalFrame(cts.Token).Forget();                                  // Fire and forget a UniTask with cancellation token. Forget() suppresses warnings.
        //cts.Cancel();                                                     // Cancel the UniTask to stop it firing

        EveryUpdate(cts.Token).Forget();                                    // Fire every frame
        FireOnceTimer(cts.Token).Forget();                                  // Fire after X seconds
    }

    async UniTask IntervalFrame(CancellationToken token)      // IntervalFrame is similar to Update, but fires every X frames
    {
        await foreach (var _ in UniTaskAsyncEnumerable.IntervalFrame(100).WithCancellation(token))              // Set frame count in IntervalFrame()
        {
            Debug.Log("Interval frame: " + Time.frameCount);
        }
    }
    async UniTask EveryUpdate(CancellationToken token)      // EveryUpdate is basically a cancellable Update(). It fires every frame.
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(token))              // Set frame count in IntervalFrame()
        {
            Debug.Log("Every Update: " + Time.frameCount);
        }
    }

    async UniTask FireOnceTimer(CancellationToken token)
    {
        await foreach (var _ in UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(3)).WithCancellation(token))              // Fire once after 3 seconds
        {
            Debug.Log("Timer: " + Time.frameCount);
        }
    }

    #endregion

    #region Delay
    async void UniTaskDelay(int milliseconds)      
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        Debug.Log("Delay started");
        //cts.Cancel();                                                            // To cancel the delay. Note that the method will continue, so "Delay completed" will still be printed.
        await UniTask.Delay(milliseconds, cancellationToken: cts.Token);           // Similar to DelayFrame
        Debug.Log("Delay completed");
    }
    #endregion

}
