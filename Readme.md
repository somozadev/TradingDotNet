# TradingBacktester (.NET 8)

Mini trading backtesting platform built with .NET 8 focusing on:
- Streaming tick ingestion (CSV + realtime stream simulation)
- Clean architecture (Domain / Application / Infrastructure / Api)
- Dependency Injection, structured logging, unit & integration testing
- Performance awareness (GC pressure / allocations)

## Features
- Read ticks from CSV as `IAsyncEnumerable<Tick>` (streaming, no full-file load)
- Realtime tick stream using `Channel<T>` + `BackgroundService` generator
- Backtest runner (WIP) that consumes ticks and produces results
- ASP.NET Core Web API endpoints for running backtests

## Project structure
- `Domain`: core models (Tick, Order, Trade, etc.)
- `Application`: use cases + abstractions (ITickSource, IBacktestRunner)
- `Infrastructure`: CSV and streaming implementations
- `Api`: ASP.NET Core API + background streaming generator
- `Tests`: xUnit unit tests

## Running (CSV mode)
1. Put a dataset in `Api/datasets/demo.csv`
2. Configure `DatasetsRoot` (optional)
3. Run:
   ```bash
   dotnet run --project Api
