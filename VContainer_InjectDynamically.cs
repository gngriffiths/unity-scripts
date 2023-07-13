using System;

public class VContainer_InjectDynamically
{
    [Inject]
    ClassToInject classToInject; 

    // Inject this dynamically instantiated Monobehaviour.
    void Construct()
    {
        var lifetimeScope = FindObjectOfType<LifetimeScope>();      // NTH Change from FindObjectOfType to something less compute intenstive.
        var resolver = lifetimeScope.Container;
        resolver.Inject(this);
    }

}
