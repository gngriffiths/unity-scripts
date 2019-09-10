using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Utility class to get data from a coroutine 
// Instructions in comments at bottom of script
// https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}

//// Use the below template to make a coroutine that returns data
//private IEnumerator LoadSomeStuff()
//{
//    WWW www = new WWW("http://someurl");
//    yield return www;
//    if (String.IsNullOrEmpty(www.error) {
//        yield return "success";
//    }
//    else
//    {
//        yield return "fail";
//    }
//}

//// Invoke the coroutine LoadSomeStuff using the coroutine below and receive the data from LoadSomeStuff
//IEnumerator GetLoadedData()
//{
//    coroutinewithdata cd = new coroutinewithdata(this, loadsomestuff());
//    yield return cd.coroutine;
//    debug.log("result is " + cd.result);  //  'success' or 'fail'
//}

