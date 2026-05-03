### Iteration 1

I have implemented the Iteration 1 backend prototype following the strict guidelines laid out in your specification and the agent persona profile.

Here is a summary of the implementation:

1. Architectural Integrity & Internal Data Model
SubtitleQc.Core Library (.NET 10): Created the root C# project, ensuring zero external dependencies (purely standard System libraries).
Internal Agnostic Data Model: Added the Cue model to represent an abstracted, format-agnostic subtitle unit, along with QcResult and QcReport models. All data representations are straightforward objects that can natively serialize to JSON.
Parsers Module: Built basic SrtParser and WebVttParser implementations complying with IParser. They safely decouple reading string inputs from the validation rules by projecting the data solely into the internal Cue data model.
2. QC Rule Engine (Clean Code & SRP)
Dependency Inversion: Constructed an IQcRule interface that acts as the contract for all rules.
Rule Engine: Evaluates a stream of generic Cues against a list of injected rules.
Rules Implemented:
MaxLinesRule
MaxCplRule (Characters Per Line)
MaxCpsRule (Characters Per Second)
MinDurationRule
OverlapCheckRule
EmptyCueCheckRule
Each rule lives in its own file, fulfilling the Single Responsibility Principle, and no method exceeds 20 lines of code.
3. Test Integrity & Telemetry Protocol
I encountered broken dependencies in the existing SubtitleQc.Tests suite, so I fixed the csproj missing test package versions, without modifying the immutable objective test scenarios.
All 13 tests pass successfully (dotnet test returned a 100% pass rate on the second attempt).
Logging was rigorously maintained, generating a test_metrics.jsonl file in the root structure tracking both the failing build baseline and the finalized, successful validation telemetry matching your exact JSON layout.
The engine is now fully functional, testable, and prepared for future specification iterations!