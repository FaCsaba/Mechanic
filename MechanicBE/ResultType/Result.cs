// using System.Diagnostics;
// using FluentValidation.Results;
//

using System.Diagnostics;
using FluentValidation;
using MechanicBE.Errors;
using Microsoft.AspNetCore.Mvc;

namespace MechanicBE.ResultType;

public delegate void Consumer<in T>(T x);

public delegate Task ConsumerAsync<in T>(T x);

public interface IResult;

public abstract record Result<TOk> : IResult
{
    public static OkResult<TOk> Ok(TOk value) => new(value);
    public static ErrResult<TOk> Err(Error error) => new(error);

    public abstract Result<TOtherOk> Bind<TOtherOk>(Func<TOk, TOtherOk> okFn,
        Func<Error, Error> errFn);

    public abstract TResult Match<TResult>(Func<TOk, TResult> okFn, Func<Error, TResult> errFn);

    public Error? Use(Consumer<TOk> fn) => Match<Error?>(value =>
    {
        fn.Invoke(value);
        return default;
    }, err => err);

    public Task<Error?> UseAsync(ConsumerAsync<TOk> fn) => Match<Task<Error?>>(async value =>
    {
        await fn.Invoke(value);
        return default;
    }, async err => err);

    public Result<TOtherOk> Map<TOtherOk>(Func<TOk, TOtherOk> okFn) => Bind(okFn, err => err);

    public async Task<Result<TOtherOk>> MapAsync<TOtherOk>(Func<TOk, Task<TOtherOk>> okFn) =>
        await Match<Task<Result<TOtherOk>>>(async o => await okFn.Invoke(o), async e => e);

    public static implicit operator Result<TOk>(Error error) => Err(error);
    public static implicit operator Result<TOk>(TOk value) => Ok(value);

    public static implicit operator Result<TOk>(Result<Result<TOk>> result)
    {
        if (result is OkResult<Result<TOk>> innerResult) return innerResult.Value;
        if (result is ErrResult<TOk> err) return err;
        throw new UnreachableException();
    }

    public static implicit operator ActionResult<TOk>(Result<TOk> result) =>
        result.Match<ObjectResult>(x => new OkObjectResult(x),
            err => err.ToObjectResult());
}

public record OkResult<TOk>(TOk Value) : Result<TOk>
{
    public override Result<TOtherOk> Bind<TOtherOk>(Func<TOk, TOtherOk> okFn,
        Func<Error, Error> errFn) => okFn.Invoke(Value);

    public override TResult Match<TResult>(Func<TOk, TResult> okFn, Func<Error, TResult> errFn) => okFn.Invoke(Value);
}

public record ErrResult<TOk>(Error Error) : Result<TOk>
{
    public override Result<TOtherOk> Bind<TOtherOk>(Func<TOk, TOtherOk> okFn,
        Func<Error, Error> errFn) => errFn.Invoke(Error);

    public override TResult Match<TResult>(Func<TOk, TResult> okFn, Func<Error, TResult> errFn) => errFn.Invoke(Error);
}