using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.Common
{
    public class Result<T>
    {
        private readonly T _value;
        private readonly Error _error;

        private readonly bool _isSuccess;

        private Result(T value)
        {
            _isSuccess = true;
            _value = value;
            _error = Error.None;
        }

        private Result(Error error)
        {
            _isSuccess = false;
            _value = default;
            _error = error;
        }

        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;

        public T Value => _value;

        public Error Error => _error;

        public static Result<T> Success(T value)
            => new(value);
        public static Result<T> Failure(Error error)
            => new(error);

        public static implicit operator Result<T>(T value) => new(value);

        public static implicit operator Result<T>(Error error) => new(error);
    }
}
