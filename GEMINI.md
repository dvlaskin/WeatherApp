## Design Philosophy

The project is built on the following core principles to ensure maintainability and enterprise-grade performance:

* **DDD (Domain-Driven Design):** The core logic is decoupled from infrastructure.
* **SOLID:** Strict adherence to dependency inversion. Every component is replaceable and testable.
* **KISS & DRY:** We prioritize clarity. Abstractions are introduced only when they provide clear value for extensibility.
* **Performance First:** Optimized for low latency and minimal memory footprint using modern .NET features.

---

## High-Performance & Memory Optimization

To achieve "Zero-Allocation" paths and high throughput:

* **Asynchronous Streaming:** Utilizing `IAsyncEnumerable<T>` for real-time response streaming.
* **Memory Management:** * Extensive use of `ReadOnlySpan<char>` for string parsing.
* **ArrayPool<T>** for reuse arrays.
* **ObjectPool<T>** for reuse objects.
* **MemoryCache** for caching.
* **ConcurrentDictionary** for concurrent access.
* **Channel<T>** for decoupling data processing.
* **Tasks** for parallel processing async I/O operations.
* **Parallel** for parallel processing CPU-bound operations.
* **Interlocked** for thread-safe operations.
* **SemaphoreSlim** for limiting concurrent access.

---

## Requirements to AI Agent

* use well documented code - if there is complex operations, add comments and documentation.
* do not write code that is not needed.
* do not write code, if you not sure or don't know how it works.
* ask questions if you need to.
* ask user if you have doubts or concerns.
* if you don't know how to solve a problem, ask for help.
* if you don't know answer to question, say it as is.
* use the same coding style as throughout the solution.
* use the same naming conventions as throughout the solution.
* Always use context7 when I need code generation, setup or configuration steps, or library/API documentation. This means you should automatically use the Context7 MCP tools to resolve library id and get library docs without me having to explicitly ask.


---