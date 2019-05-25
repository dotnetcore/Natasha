
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.765 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2531254 Hz, Resolution=395.0611 ns, Timer=TSC
.NET Core SDK=2.2.204
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  Job-UMHHUD : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  Job-SMNRUP : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT
  Job-OFCJRY : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT

Runtime=Core  Categories=Read,Time  

 Method |     Toolchain |     Mean |     Error |    StdDev |      Min |      Max |   Median | Ratio | RatioSD | Rank | Gen 0 | Gen 1 | Gen 2 | Allocated |
------- |-------------- |---------:|----------:|----------:|---------:|---------:|---------:|------:|--------:|-----:|------:|------:|------:|----------:|
   Emit | netcoreapp2.0 | 3.317 ns | 0.0457 ns | 0.0405 ns | 3.274 ns | 3.399 ns | 3.315 ns |  1.00 |    0.00 |    1 |     - |     - |     - |         - |
   Emit | netcoreapp2.1 | 3.600 ns | 0.1102 ns | 0.2420 ns | 2.682 ns | 3.785 ns | 3.661 ns |  1.01 |    0.13 |    2 |     - |     - |     - |         - |
   Emit | netcoreapp2.2 | 3.632 ns | 0.0493 ns | 0.0437 ns | 3.577 ns | 3.733 ns | 3.619 ns |  1.09 |    0.02 |    2 |     - |     - |     - |         - |
