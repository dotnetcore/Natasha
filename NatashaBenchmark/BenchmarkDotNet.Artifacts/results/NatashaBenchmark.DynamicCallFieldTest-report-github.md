``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.765 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2531255 Hz, Resolution=395.0609 ns, Timer=TSC
.NET Core SDK=2.2.204
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  Job-RHUWAZ : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT
  Job-YMJGKJ : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  Job-EJKFFU : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT

Runtime=Core  UnrollFactor=64  Categories=Read,Time  

```
| Method |     Toolchain |     Mean |     Error |    StdDev |      Min |      Max |   Median | Ratio | RatioSD | Rank | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |-------------- |---------:|----------:|----------:|---------:|---------:|---------:|------:|--------:|-----:|------:|------:|------:|----------:|
| Origin | netcoreapp2.1 | 444.5 ns |  7.597 ns |  7.107 ns | 434.1 ns | 458.9 ns | 443.2 ns |  1.00 |    0.03 |    1 |     - |     - |     - |         - |
| Origin | netcoreapp2.0 | 447.2 ns |  8.835 ns | 10.175 ns | 433.6 ns | 466.5 ns | 445.0 ns |  1.00 |    0.00 |    1 |     - |     - |     - |         - |
| Origin | netcoreapp2.2 | 885.0 ns | 20.276 ns | 25.642 ns | 850.9 ns | 960.8 ns | 884.5 ns |  1.98 |    0.07 |    2 |     - |     - |     - |         - |
