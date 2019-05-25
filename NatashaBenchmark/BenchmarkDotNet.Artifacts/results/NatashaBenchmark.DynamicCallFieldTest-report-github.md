``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.765 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2531254 Hz, Resolution=395.0611 ns, Timer=TSC
.NET Core SDK=2.2.204
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  Job-YFYKOK : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  Job-KFPNPS : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  Job-XENDTJ : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT

Runtime=Core  UnrollFactor=64  Categories=Read,Time  

```
| Method |     Toolchain |      Mean |     Error |    StdDev |    Median |    Min |       Max | Ratio | RatioSD | Rank | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |-------------- |----------:|----------:|----------:|----------:|-------:|----------:|------:|--------:|-----:|------:|------:|------:|----------:|
| Origin | netcoreapp2.2 | 0.0012 ns | 0.0052 ns | 0.0043 ns | 0.0000 ns | 0.0 ns | 0.0156 ns |     ? |       ? |    1 |     - |     - |     - |         - |
| Origin | netcoreapp2.0 | 0.0016 ns | 0.0052 ns | 0.0046 ns | 0.0000 ns | 0.0 ns | 0.0165 ns |     ? |       ? |    1 |     - |     - |     - |         - |
| Origin | netcoreapp2.1 | 0.0135 ns | 0.0224 ns | 0.0210 ns | 0.0026 ns | 0.0 ns | 0.0633 ns |     ? |       ? |    1 |     - |     - |     - |         - |
