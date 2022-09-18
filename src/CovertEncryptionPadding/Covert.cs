/*
    CovertPadding: An implementation of the Covert Encryption randomised padding scheme.
    Copyright (c) 2022 Samuel Lucas
    
    Permission is hereby granted, free of charge, to any person obtaining a copy of
    this software and associated documentation files (the "Software"), to deal in
    the Software without restriction, including without limitation the rights to
    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    the Software, and to permit persons to whom the Software is furnished to do so,
    subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System.Buffers.Binary;
using System.Security.Cryptography;

namespace CovertEncryptionPadding;

public static class Covert
{
    public const double DefaultProportion = 0.05;
    
    public static long GetPaddingLength(long unpaddedLength, double proportion = DefaultProportion)
    {
        if (unpaddedLength < 0) { throw new ArgumentOutOfRangeException(nameof(unpaddedLength), unpaddedLength, $"{nameof(unpaddedLength)} must be at least 0."); }
        if (proportion is < 0.01 or > 1) { throw new ArgumentOutOfRangeException(nameof(proportion), proportion, $"{nameof(proportion)} must be between 0.01 and 1."); }
        
        long fixedPadding = Math.Max(0, (int)(proportion * 500) - unpaddedLength);
        double effectiveSize = 200 + 1e8 * Math.Log(1 + 1e-8 * (unpaddedLength + fixedPadding));
        Span<byte> randomBytes = stackalloc byte[8];
        RandomNumberGenerator.Fill(randomBytes);
        uint random1 = BinaryPrimitives.ReadUInt32LittleEndian(randomBytes[..4]);
        uint random2 = BinaryPrimitives.ReadUInt32LittleEndian(randomBytes[4..]);
        double coefficient = Math.Log(Math.Pow(2, 32)) - Math.Log(random1 + random2 * Math.Pow(2, -32) + Math.Pow(2, -33));
        return fixedPadding + (long)Math.Round(coefficient * proportion * effectiveSize);
    }

    public static long GetPaddedLength(long unpaddedLength, double proportion = DefaultProportion)
    {
        return checked(GetPaddingLength(unpaddedLength, proportion) + unpaddedLength);
    }
}